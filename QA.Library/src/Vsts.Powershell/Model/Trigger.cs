namespace Clix.Vsts.Powershell.Model
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class Trigger
	{
		[JsonProperty("batchChanges")]
		public bool BatchChanges { get; set; }

		[JsonProperty("branchFilters")]
		public IList<string> BranchFilters { get; set; }

		[JsonProperty("maxConcurrentBuildsPerBranch")]
		public int MaxConcurrentBuildsPerBranch { get; set; }

		[JsonProperty("triggerType")]
		public string TriggerType { get; set; }
	}
}