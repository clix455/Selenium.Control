namespace Clix.Vsts.Powershell.Connection
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;

	using Clix.Security;

	using Microsoft.Win32;

	/// <summary>
	///		Quick-and-dirty configuration store backed by the Windows Registry (HKCU).
	/// </summary>
	public static class ConnectionStore
	{
		/// <summary>
		///		Get or create a registry key under HKCU.
		/// </summary>
		/// <param name="keyName">
		///		The hive-relative name (path) of the key to get or create.
		/// </param>
		/// <returns>
		///		A <see cref="RegistryKey"/> representing the registry key.
		/// </returns>
		static RegistryKey GetOrCreateUserKey(string keyName)
		{
			if (string.IsNullOrWhiteSpace(keyName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'keyName'.", nameof(keyName));

			RegistryKey registryKey = null;
			try
			{
				registryKey =
					Registry.CurrentUser.OpenSubKey(keyName, writable: true)
					??
					Registry.CurrentUser.CreateSubKey(keyName);
			}
			catch
			{
				if (registryKey != null)
					registryKey.Dispose();

				throw;
			}

			return registryKey;
		}

		/// <summary>
		///		The names of registry keys used by the connection store.
		/// </summary>
		static class KeyNames
		{
			/// <summary>
			///		The name of the registry key used by the Powershell module to persist VSTS connection information.
			/// </summary>
			public const string VstsConnections = @"Software\Dimension Data\Powershell\VSTS\Connections";
		}

		/// <summary>
		///		The names of registry values used by the configuration store.
		/// </summary>
		static class ValueNames
		{
			public const string Instance = "Instance";

			public const string TeamCollection = "TeamCollection";

			public const string Password = "Password";

			public const string PersonalAccessToken = "PersonalAccessToken";

			public const string UserName = "UserName";

			public const string AuthenticationType = "AuthenticationType";
		}

		/// <summary>
		///		Get the name of the default vsts service connection.
		/// </summary>
		/// <returns>
		///		The connection name, or <c>null</c> if no connection is configured.
		/// </returns>
		public static string GetDefaultConnectionName()
		{
			using (RegistryKey connectionsKey = GetOrCreateUserKey(KeyNames.VstsConnections))
			{
				return (string)connectionsKey.GetValue(name: null);
			}
		}

		/// <summary>
		///	Set the name of the default vsts connection.
		/// </summary>
		/// <param name="connectionName">
		///	The connection name, or <c>null</c> if no connection is configured.
		/// </param>
		public static void SetDefaultConnectionName(string connectionName)
		{
			using (RegistryKey connectionsKey = GetOrCreateUserKey(KeyNames.VstsConnections))
			{
				if (!string.IsNullOrWhiteSpace(connectionName))
				{
					connectionsKey.SetValue(
						name: null,
						value: connectionName
					);
				}
				else
				{
					connectionsKey.DeleteValue(
						name: null,
						throwOnMissingValue: false
					);
				}
			}
		}

		/// <summary>
		///		Add persisted vsts connection settings.
		/// </summary>
		/// <param name="connectionSetting">
		///		A <see cref="ConnectionSetting"/> representing the connection settings to persist.
		/// </param>
		public static void AddVstsConnection(ConnectionSetting connectionSetting)
		{
			if (connectionSetting == null)
				throw new ArgumentNullException(nameof(connectionSetting));

			if (string.IsNullOrEmpty(connectionSetting.Name))
				throw new ArgumentException("Connection name cannot be null", nameof(connectionSetting));

			using (var connectionsKey = GetOrCreateUserKey(KeyNames.VstsConnections))
			{
				connectionsKey.DeleteSubKey(
					connectionSetting.Name,
					throwOnMissingSubKey: false
					);
			}

			string connectionKeyName = Path.Combine(
				KeyNames.VstsConnections,
				connectionSetting.Name
				);

			using (var connectionKey = GetOrCreateUserKey(connectionKeyName))
			{
				connectionKey.SetValue(
					ValueNames.Instance,
					connectionSetting.Instance);
				connectionKey.SetValue(
					ValueNames.TeamCollection,
					connectionSetting.TeamCollection);
				connectionKey.SetValue(
					ValueNames.AuthenticationType,
					connectionSetting.AuthenticationType);
				connectionKey.SetValue(
					ValueNames.UserName,
					connectionSetting.UserName);
				switch (connectionSetting.AuthenticationType)
				{
					case AuthenticationType.PSCredential:
						connectionKey.SetValue(
							ValueNames.Password,
							ProtectedSecret.Protect(connectionSetting.Password));
						break;
					case AuthenticationType.Basic:
						connectionKey.SetValue(
							ValueNames.PersonalAccessToken,
							ProtectedSecret.Protect(connectionSetting.PersonalAccessToken));
						break;
					case AuthenticationType.OAuth:
						// TODO: configure OAuth required settings
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}

		/// <summary>
		///		Remove persisted vsts connection settings.
		/// </summary>
		/// <param name="connectionName">
		///		The name of the connection whose persisted settings are to be removed.
		/// </param>
		public static void RemoveVstsConnection(string connectionName)
		{
			if (string.IsNullOrWhiteSpace(connectionName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'connectionName'.", nameof(connectionName));

			using (RegistryKey emsConnectionsKey = GetOrCreateUserKey(KeyNames.VstsConnections))
			{
				emsConnectionsKey.DeleteSubKey(connectionName, throwOnMissingSubKey: false);
			}
		}

		/// <summary>
		///		Retrieve persisted VSTS Service connection settings.
		/// </summary>
		/// <returns>
		///		A sequence of 0 or more <see cref="ConnectionSetting"/>.
		/// </returns>
		public static IEnumerable<ConnectionSetting> GetPersistedConnections()
		{
			using (RegistryKey vstsConnectionsKey = GetOrCreateUserKey(KeyNames.VstsConnections))
			{
				foreach (var connectionName in vstsConnectionsKey.GetSubKeyNames())
				{
					using (RegistryKey connectionKey = vstsConnectionsKey.OpenSubKey(connectionName))
					{
						yield return ParseServiceConnectionSettings(connectionName, connectionKey);
					}
				}
			}
		}

		/// <summary>
		///		Parse service connection settings from the specified registry key.
		/// </summary>
		/// <param name="connectionName">
		///		The name of the connection being parsed.
		/// </param>
		/// <param name="connectionKey">
		///		The registry holding the connection settings.
		/// </param>
		private static ConnectionSetting ParseServiceConnectionSettings(string connectionName, RegistryKey connectionKey)
		{
			if (string.IsNullOrWhiteSpace(connectionName))
				throw new ArgumentException("Argument cannot be null, empty, or composed entirely of whitespace: 'connectionName'.", nameof(connectionName));

			if (connectionKey == null)
				throw new ArgumentNullException(nameof(connectionKey));
			ConnectionSetting connectionSetting = new ConnectionSetting
			{
				Name = connectionName
			};

			string instance = connectionKey.GetValue(ValueNames.Instance) as string;
			if (string.IsNullOrEmpty(instance))
				throw new FormatException(
					$"Persisted settings for VSTS service connection '{connectionName}' are missing '{ValueNames.Instance}' value.");
			connectionSetting.Instance = instance;
			try
			{
				AuthenticationType authenticationType =
					(AuthenticationType)Enum.Parse(
						typeof(AuthenticationType),
						(string)connectionKey.GetValue(ValueNames.AuthenticationType));
				connectionSetting.AuthenticationType = authenticationType;
			}
			catch (Exception)
			{
				throw new FormatException(
					   $"Persisted settings for VSTS service connection '{connectionName}' are missing '{ValueNames.AuthenticationType}' value, or with invalid value.");
			}

			string teamCollection = connectionKey.GetValue(ValueNames.TeamCollection) as string;
			if (string.IsNullOrEmpty(teamCollection))
				throw new FormatException(
					$"Persisted settings for VSTS service connection '{connectionName}' are missing '{ValueNames.TeamCollection}' value.");
			connectionSetting.TeamCollection = teamCollection;

			string userName = connectionKey.GetValue(ValueNames.UserName) as string;
			if (string.IsNullOrEmpty(userName))
				throw new FormatException(
					$"Persisted settings for VSTS service connection '{connectionName}' are missing '{ValueNames.UserName}' value.");
			connectionSetting.UserName = userName;

			switch (connectionSetting.AuthenticationType)
			{
				case AuthenticationType.PSCredential:
					{
						string password = connectionKey.GetValue(ValueNames.Password) as string;
						if (string.IsNullOrEmpty(userName))
							throw new FormatException(
								$"Persisted settings for VSTS service connection '{connectionName}' are missing '{ValueNames.Password}' value.");
						connectionSetting.Password = ProtectedSecret.UnProtect(password);
						break;
					}
				case AuthenticationType.Basic:
					break;
				case AuthenticationType.OAuth:
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return connectionSetting;
		}

		/// <summary>
		/// Gets the default VSTS service connection settings
		/// </summary>
		/// <returns>Connection String</returns>
		public static ConnectionSetting GetDefaultConnectionSetting()
		{
			var connectionSettings = GetPersistedConnections();
			var defaultConnectionName = GetDefaultConnectionName();

			if (defaultConnectionName != null)
				return connectionSettings.FirstOrDefault(c => c.Name == defaultConnectionName);

			return connectionSettings.FirstOrDefault();
		}

		/// <summary>
		/// Gets the connection settings with the specified name
		/// </summary>
		/// <returns>Connection settings</returns>
		public static ConnectionSetting GetConnectionSetting(string connectionName)
		{
			if (string.IsNullOrWhiteSpace(connectionName))
				throw new ArgumentException("connectionName must not be empty or null.", nameof(connectionName));

			var connectionSettings = GetPersistedConnections();

			return connectionSettings.FirstOrDefault(c => c.Name == connectionName);
		}
	}
}
