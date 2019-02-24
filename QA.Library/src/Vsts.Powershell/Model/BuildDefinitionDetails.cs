namespace Clix.Vsts.Powershell.Model
{
	using System;
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class BuildDefinitionDetails
	{
		[JsonProperty("_links")]
		public Dictionary<string, Link> Links { get; set; }

		[JsonProperty("authoredBy")]
		public Principal AuthoredBy { get; set; }

		[JsonProperty("build")]
		public IList<BuildStep> BuildSteps { get; set; }

		[JsonProperty("buildNumberFormat")]
		public string BuildNumberFormat { get; set; }

		[JsonProperty("createdDate")]
		public DateTime CreatedDate { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("jobAuthorizationScope")]
		public string JobAuthorizationScope { get; set; }

		[JsonProperty("jobTimeoutInMinutes")]
		public int JobTimeoutInMinutes { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("options")]
		public IList<Option> Options { get; set; }

		[JsonProperty("project")]
		public Project Project { get; set; }

		[JsonProperty("quality")]
		public string Quality { get; set; }

		[JsonProperty("queue")]
		public Queue Queue { get; set; }

		[JsonProperty("repository")]
		public Repository Repository { get; set; }

		[JsonProperty("retentionRules")]
		public IList<RetentionRule> RetentionRules { get; set; }

		[JsonProperty("revision")]
		public int Revision { get; set; }

		[JsonProperty("triggers")]
		public IList<Trigger> Triggers { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("uri")]
		public string Uri { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("variables")]
		public Dictionary<string, Variable> Variables { get; set; }
	}

}
