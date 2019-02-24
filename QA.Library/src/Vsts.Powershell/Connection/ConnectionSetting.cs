// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConnectionSettings.cs" company="">
//   
// </copyright>
// <summary>
//   The connection settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Clix.Vsts.Powershell.Connection
{
	using System.Management.Automation;

	/// <summary>
	/// The connection settings.
	/// </summary>
	public class ConnectionSetting
	{
		/// <summary>
		/// Gets or sets the authentication type.
		/// </summary>
		public AuthenticationType AuthenticationType { get; set; }

		/// <summary>
		///     The client id when requesting JWT token
		/// </summary>
		public string ClientId { get; set; }

		/// <summary>
		///     The client secret when requestion JWT token.
		/// </summary>
		public string ClientSecret { get; set; }

		/// <summary>
		/// Gets or sets the instance.
		/// </summary>
		public string Instance { get; set; }

		/// <summary>
		///     The connection name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     The user password when authenticate with <see cref="PSCredential" />
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		///     The personal access token when authenticate using basic authentication.
		/// </summary>
		public string PersonalAccessToken { get; set; }

		/// <summary>
		/// Gets or sets the team collection.
		/// </summary>
		public string TeamCollection { get; set; }

		/// <summary>
		///     The token issuing Server when requestion JWT token.
		/// </summary>
		public string TokenServer { get; set; } = "https://app.vssps.visualstudio.com/oauth2";

		/// <summary>
		///     The user name for the connection.
		/// </summary>
		public string UserName { get; set; }
	}
}