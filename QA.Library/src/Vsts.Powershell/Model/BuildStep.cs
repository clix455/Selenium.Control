namespace Clix.Vsts.Powershell.Model
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class BuildStep
	{
		[JsonProperty("alwaysRun")]
		public bool AlwaysRun { get; set; }

		[JsonProperty("continueOnError")]
		public bool ContinueOnError { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		[JsonProperty("inputs")]
		public Dictionary<string, string> Inputs { get; set; }

		[JsonProperty("task")]
		public BuildTask Task { get; set; }
	}
}