namespace Clix.Vsts.Powershell.Commands.Connection
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Management.Automation;

	using Powershell.Connection;

	[Cmdlet(VerbsCommon.Get, "VstsConnection")]
	public class GetConnection : PSCmdlet
	{

		/// <summary>
		///		The name of the connection to retrieve.
		/// </summary>
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = false, Position = 0, HelpMessage = "The name of the Ems API service connection to retrieve")]
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		///		Perform Cmdlet processing.
		/// </summary>
		protected override void ProcessRecord()
		{
			base.ProcessRecord();

			IEnumerable<ConnectionSetting> connections = ConnectionStore.GetPersistedConnections();
			IEnumerable<ConnectionSetting> maskedConnections =
				connections.Select(
					connection =>
					{
						ConnectionSetting result = connection;
						result.Password = string.Empty;
						result.PersonalAccessToken = string.Empty;
						result.ClientSecret = string.Empty;
						return result;
					});

			if (string.IsNullOrWhiteSpace(this.Name))
			{
				this.WriteObject(maskedConnections, true);
			}
			else
			{
				ConnectionSetting connection = 
					maskedConnections
					.FirstOrDefault(c => c.Name.Equals(this.Name, StringComparison.CurrentCultureIgnoreCase));
				this.WriteObject(connection);
			}
		}
	}
}
