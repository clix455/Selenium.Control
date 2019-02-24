namespace Clix.Vsts.Powershell.Commands.Build
{
	using System.Management.Automation;

	using Model;

	/// <summary>
	/// Get the log .
	/// </summary>
	[Cmdlet(VerbsCommon.Show, "Log")]
	public class GetLog : WithProjectCmdlet
	{
		/// <summary>
		/// The request uri template.
		/// </summary>
		private static readonly string RequestUri = $"_apis/build/builds/{{buildId}}?{ApiVersion}";

		/// <summary>
		/// The log record.
		/// </summary>
		[Parameter(HelpMessage = "The log record.",
			ValueFromPipelineByPropertyName = true,
			Mandatory = true)]
		public LogInformation Log { get; set; }

		/// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		protected override sealed void ProcessRecord()
		{
			var result = this.GetAsync<QueryResult<string>>(this.FormalizedRequest).Result;

			this.WriteObject(result, true);
		}

		/// <inheritdoc />
		protected override string FormalizedRequest
		{
			get
			{
				return this.Log.Url;
			}
		}
	}
}
