namespace Clix.Vsts.Powershell.Commands.Build
{
	using System.Management.Automation;

	using Model;

	/// <summary>
	/// Get a list of build definitions.
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "BuildDefinition")]
	public class GetBuildDefinition : WithProjectCmdlet
	{
		/// <summary>
		/// The request uri template.
		/// </summary>
		private static readonly string RequestUri = $"_apis/build/definitions?{ApiVersion}";

		/// <summary>
		/// The type of the build definitions to retrieve. If not specified, all types will be returned.
		/// </summary>
		[Parameter(HelpMessage = "The type of the build definitions to retrieve. If not specified, all types will be returned.")]
		[ValidateSet("build", "xaml")]
		public string BuildType { get; set; }

		/// <summary>
		/// Filters to definitions whose names equal this value. Append a * to filter to definitions whose names start with this value. For example: MS*.
		/// </summary>
		[Parameter(HelpMessage = "Filters to definitions whose names equal this value. Append a * to filter to definitions whose names start with this value. For example: MS*.")]
		public string NameFilter { get; set; }

		/// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		protected override sealed void ProcessRecord()
		{
			QueryResult<BuildDefinition> projects = 
				this.GetAsync<QueryResult<BuildDefinition>>(this.FormalizedRequest)
				.Result;
			this.WriteObject(projects.Value, true);
		}

		/// <inheritdoc />
		protected override string FormalizedRequest
		{
			get
			{
				string result = this.GetUriWithProject(RequestUri);
				if (!string.IsNullOrWhiteSpace(this.NameFilter))
					result += $"&name={this.NameFilter}";
				if (!string.IsNullOrWhiteSpace(this.BuildType))
					result += $"&type={this.BuildType}";
				return result;
			}
		}
	}
}
