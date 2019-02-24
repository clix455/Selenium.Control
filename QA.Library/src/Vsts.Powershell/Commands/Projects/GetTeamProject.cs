namespace Clix.Vsts.Powershell.Commands.Projects
{
	using System.Management.Automation;

	using Model;

	/// <summary>
	/// Get a list of team project
	/// </summary>
	[Cmdlet(VerbsCommon.Get, "TeamProject")]
	public class GetTeamProject : VstsCmdlet
	{
		/// <summary>
		/// The request uri template.
		/// </summary>
		private static readonly string RequestUri = $"_apis/projects?{ApiVersion}";

		/// <summary>
		/// Return team projects in a specific team project state.
		/// </summary>
		[Parameter]
		[ValidateSet("WellFormed", "CreatePending", "Deleting", "New", "All")]
		public string StateFilter { get; set; }

		/// <summary>
		/// Define the number of team projects to return.
		/// </summary>
		[Parameter]
		public int? Top { get; set; }

		/// <summary>
		/// Define the number of team projects to skip.
		/// </summary>
		[Parameter]
		public int? Skip { get; set; }


		/// <summary>
		///		Asynchronously perform Cmdlet processing.
		/// </summary>
		protected override sealed void ProcessRecord()
		{
			var projects =
				this.GetAsync<QueryResult<Project>>(this.FormalizedRequest)
					.Result;
			this.WriteObject(projects.Value, true);
		}

		/// <summary>
		/// Formalize the request uri from parameters.
		/// </summary>
		protected override string FormalizedRequest
		{
			get
			{
				string result = RequestUri;
				if (!string.IsNullOrWhiteSpace(this.StateFilter))
					result += $"&stateFilter={this.StateFilter}";
				if (this.Top != null)
					result += $"&$top={this.Top}";
				if (this.Skip != null)
					result += $"&$skip={this.Skip}";
				return result;
			}
		}
	}
}
