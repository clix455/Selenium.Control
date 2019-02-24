namespace Clix.Vsts.Powershell.Commands.Build
{
	using System.Collections;
	using System.Management.Automation;
	using System.Text;

	using Model;

	using Newtonsoft.Json.Linq;

	/// <summary>
	/// Queue a build.
	/// </summary>
	[Cmdlet(VerbsLifecycle.Start, "Build")]
	public class StartBuild : WithProjectCmdlet
	{
		/// <summary>
		/// The request uri template.
		/// </summary>
		private static readonly string RequestUri = $"_apis/build/builds?{ApiVersion}";

		/// <summary>
		/// The ID of the definition. This is required.
		/// </summary>
		[Parameter(HelpMessage = "The ID of the definition. This is required.",
			ValueFromPipelineByPropertyName = true,
			Mandatory = true)]
		public int? Id { get; set; }

		/// <summary>
		/// The branch to build. This is optional. If not specified, the default branch for the definition will be used.
		/// </summary>
		[Parameter(HelpMessage = "The branch to build. This is optional. If not specified, the default branch for the definition will be used.")]
		public string SourceBranch { get; set; }

		/// <summary>
		/// Parameters to pass to the build. This is optional. If not specified, the default variables for the definition will be used.
		/// </summary>
		[Parameter(HelpMessage = "Parameters to pass to the build. This is optional. If not specified, the default variables for the definition will be used.")]
		public Hashtable Parameters { get; set; }

		/// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		protected override sealed void ProcessRecord()
		{
			JObject payLoad = new JObject
			{
				{
					"definition", new JObject
					{
						{
							"id", this.Id
						}
					}
				}
			};

			if (this.Parameters != null && this.Parameters.Count > 0)
			{
				StringBuilder builder = new StringBuilder();
				builder.Append("{");
				int parameterCount = this.Parameters.Count;
				int currentIndex = 0;

				foreach (string key in this.Parameters.Keys)
				{
					builder.Append($"\"{key}\":\"{this.Parameters[key]}\"");
					if (++currentIndex < parameterCount)
						builder.Append(",");
				}

				builder.Append("}");

				payLoad.Add("parameters", builder.ToString());
			}
			if (!string.IsNullOrWhiteSpace(this.SourceBranch))
				payLoad.Add("sourceBranch", this.SourceBranch);

			Build result = this.PostAsync<JObject, Build>(this.FormalizedRequest, payLoad).Result;

			this.WriteObject(result, true);
		}

		/// <inheritdoc />
		protected override string FormalizedRequest => this.GetUriWithProject(RequestUri);
	}
}
