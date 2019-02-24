namespace Clix.Vsts.Powershell.Commands.Build
{
	using System.Management.Automation;

	using Model;

	/// <summary>
	/// Get a build.
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "Build")]
	public class GetBuild : WithProjectCmdlet
	{
		/// <summary>
		/// The request uri template.
		/// </summary>
		private static readonly string RequestUri = $"_apis/build/builds/{{buildId}}?{ApiVersion}";

		/// <summary>
		/// The ID of the build. This is required.
		/// </summary>
		[Parameter(HelpMessage = "The ID of the build. This is required.",
			ValueFromPipelineByPropertyName = true,
			Mandatory = true)]
		public int? Id { get; set; }

		/// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		protected override sealed void ProcessRecord()
		{
			Build result = this.GetAsync<Build>(this.FormalizedRequest).Result;

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
					result = result.Replace("{buildId}", this.Id.ToString());
				}
				return result;
			}
		}
	}
}
