namespace Clix.Vsts.Powershell.Commands.Connection
{
	using System.Collections.Generic;
	using System.Management.Automation;

	using Powershell.Connection;

	[Cmdlet(VerbsCommon.Remove, "VstsConnection")]
	public class RemoveConnection : PSCmdlet
	{

		/// <summary>
		/// Specify this to remove all persisted connections
		/// </summary>
		[Parameter(Mandatory = true, HelpMessage = "Specify this to remove all persisted connections", ParameterSetName = "RemoveAll")]
		public SwitchParameter RemoveAll
		{
			get; set;
		}

		/// <summary>
		///		The name of the connection to retrieve.
		/// </summary>
		[ValidateNotNullOrEmpty]
		[Parameter(Mandatory = true, Position = 0, HelpMessage = "The name of the VSTS service connection to remove", ParameterSetName = "RemoveByName")]
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
			ProgressRecord progress = 
				new ProgressRecord(1, this.MyInvocation.MyCommand.Name + ": Close VSTS service connection", "Start closing connection...");
			this.WriteProgress(progress);

			if (this.RemoveAll)
			{
				IEnumerable<ConnectionSetting> connections = ConnectionStore.GetPersistedConnections();
				foreach (ConnectionSetting connection in connections)
				{
					ConnectionStore.RemoveVstsConnection(connection.Name);
				}

				ConnectionStore.SetDefaultConnectionName(null);
			}
			else
			{
				ConnectionStore.RemoveVstsConnection(this.Name);
				if (this.Name == ConnectionStore.GetDefaultConnectionName())
				{
					ConnectionStore.SetDefaultConnectionName(null);
				}
			}

			progress.RecordType = ProgressRecordType.Completed;
			this.WriteProgress(progress);
		}
	}
}
