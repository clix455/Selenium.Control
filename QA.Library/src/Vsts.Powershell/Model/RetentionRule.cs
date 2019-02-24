namespace Clix.Vsts.Powershell.Model
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class RetentionRule
	{
		[JsonProperty("artifacts")]
		public IList<string> Artifacts { get; set; }

		[JsonProperty("branches")]
		public IList<string> Branches { get; set; }

		[JsonProperty("daysToKeep")]
		public int DaysToKeep { get; set; }

		[JsonProperty("deleteBuildRecord")]
		public bool DeleteBuildRecord { get; set; }

		[JsonProperty("deleteTestResults")]
		public bool DeleteTestResults { get; set; }

		[JsonProperty("minimumToKeep")]
		public int MinimumToKeep { get; set; }
	}
}