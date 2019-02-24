// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NewConnection.cs" company="">
//   
// </copyright>
// <summary>
//   The new connection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clix.Vsts.Powershell.Commands.Connection
{
	using System.Management.Automation;

	using Powershell.Connection;

	/// <summary>
	/// The new connection.
	/// </summary>
	[Cmdlet(VerbsCommon.New, "VstsConnection")]
	public class NewConnection : PSCmdlet
	{
		/// <summary>
		/// Gets or sets the basic.
		/// </summary>
		[Parameter(Mandatory = true, 
			ParameterSetName = ParameterSetNames.Basic, 
			Position = 2, 
			HelpMessage = "Use basic authentication?")]
		public SwitchParameter Basic { get; set; }

		/// <summary>
		/// Gets or sets the instance.
		/// </summary>
		[Parameter]
		public string Instance { get; set; }

		/// <summary>
		///     A descriptive name for the connection to create.
		/// </summary>
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "A descriptive name for the connection to create")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		[Parameter(Mandatory = true, 
			ParameterSetName = ParameterSetNames.PSCredential, 
			HelpMessage = "User's password for creating PSCredential.")]
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the personal access token.
		/// </summary>
		[Parameter(Mandatory = true, 
			ParameterSetName = ParameterSetNames.Basic, 
			HelpMessage = "User's personal access token used for basic authentication method.")]
		public string PersonalAccessToken { get; set; }

		/// <summary>
		/// Gets or sets if the connection is using <see cref="PSCredential"/> authentication method.
		/// </summary>
		[Parameter(Mandatory = true, 
			Position = 2, 
			ParameterSetName = ParameterSetNames.PSCredential)]
		public SwitchParameter PSCredential { get; set; }

		/// <summary>
		///     Set the new connection as the default connection to use?
		/// </summary>
		[Parameter(HelpMessage = "Set the new connection as the default connection to use?")]
		public SwitchParameter SetDefault { get; set; }

		/// <summary>
		/// Gets or sets the team collection.
		/// </summary>
		[Parameter]
		public string TeamCollection { get; set; } = "DefaultCollection";

		/// <summary>
		/// Gets or sets the user name.
		/// </summary>
		[Parameter(Mandatory = true, Position = 1, HelpMessage = "A user name for the connection to create")]
		public string UserName { get; set; }

		/// <summary>
		/// The process record.
		/// </summary>
		/// <exception cref="PSArgumentException">
		/// </exception>
		protected override void ProcessRecord()
		{
			ConnectionSetting connectionSettings =
				new ConnectionSetting
				{
					Name = this.Name, 
					UserName = this.UserName, 
					Instance = this.Instance, 
					TeamCollection = this.TeamCollection
				};

			switch (this.ParameterSetName)
			{
				case ParameterSetNames.PSCredential:
					{
						connectionSettings.AuthenticationType = AuthenticationType.PSCredential;
						if (string.IsNullOrEmpty(this.Password))
							throw new PSArgumentException("password cannot be null or empty.", nameof(this.Password));
						connectionSettings.Password = this.Password;
						break;
					}

				case ParameterSetNames.Basic:
					{
						connectionSettings.AuthenticationType = AuthenticationType.Basic;
						if (string.IsNullOrEmpty(this.PersonalAccessToken))
							throw new PSArgumentException("Personal access token cannot be null or empty.", nameof(this.PersonalAccessToken));
						connectionSettings.PersonalAccessToken = this.PersonalAccessToken;
						break;
					}

				default:
					goto case ParameterSetNames.PSCredential;
			}

			ConnectionStore.AddVstsConnection(connectionSettings);

			if (this.SetDefault.IsPresent)
				ConnectionStore.SetDefaultConnectionName(this.Name);
			this.WriteObject(connectionSettings);
		}

		/// <summary>
		/// The parameter set names.
		/// </summary>
		private static class ParameterSetNames
		{
			/// <summary>
			/// The basic.
			/// </summary>
			public const string Basic = "Basic";

			/// <summary>
			/// The ps credential.
			/// </summary>
			public const string PSCredential = "PSCredential";
		}
	}
}