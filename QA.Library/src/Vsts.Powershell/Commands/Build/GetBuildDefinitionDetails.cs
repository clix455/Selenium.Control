namespace Clix.Vsts.Powershell.Commands.Build
{
	using System.Management.Automation;

	using Model;

	/// <summary>
	/// Get a build definition with details.
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "BuildDefinitionDetails")]
	public class GetBuildDefinitionDetails : WithProjectCmdlet
	{
		/// <summary>
		/// The request uri template.
		/// </summary>
		private static readonly string RequestUri = $"_apis/build/definitions/{{definitionId}}?{ApiVersion}";

		/// <summary>
		/// ID of the build definition.
		/// </summary>
		[Parameter(HelpMessage = "ID of the build definition.", Mandatory = true, ValueFromPipelineByPropertyName = true)]
		public int? Id { get; set; }

		/// <summary>
		/// The specific revision number of the definition to retrieve.
		/// </summary>
		[Parameter(HelpMessage = "The specific revision number of the definition to retrieve.")]
		public int? Revision { get; set; }

		/// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		protected override sealed void ProcessRecord()
		{
			BuildDefinitionDetails result = 
				this.GetAsync<BuildDefinitionDetails>(this.FormalizedRequest)
				.Result;
			this.WriteObject(result, true);
		}

		/// <inheritdoc />
		protected override string FormalizedRequest
		{
			get
			{
				string result = this.GetUriWithProject(RequestUri);
				if (this.Id != null)
				{
					result = result.Replace("{definitionId}", this.Id.ToString());
				}
				if (this.Revision != null)
				{
					result += $"&revision={this.Revision}";
				}
				return result;
			}
		}
	}
}
