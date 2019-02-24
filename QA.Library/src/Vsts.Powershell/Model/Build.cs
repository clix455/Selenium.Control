namespace Clix.Vsts.Powershell.Model
{
	using System;
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class Build
	{
		[JsonProperty("tags")]
		public IList<string> Tags { get; set; }

		[JsonProperty("_links")]
		public Dictionary<string, Link> Links { get; set; }

		[JsonProperty("buildNumber")]
		public string BuildNumber { get; set; }

		[JsonProperty("buildNumberRevision")]
		public int BuildNumberRevision { get; set; }

		[JsonProperty("definition")]
		public BuildDefinition Definition { get; set; }

		[JsonProperty("finishTime")]
		public DateTime FinishTime { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("keepForever")]
		public bool KeepForever { get; set; }

		[JsonProperty("lastChangedBy")]
		public Principal LastChangedBy { get; set; }

		[JsonProperty("lastChangedDate")]
		public DateTime LastChangedDate { get; set; }

		[JsonProperty("logs")]
		public LogInformation Logs { get; set; }

		[JsonProperty("orchestrationPlan")]
		public OrchestrationPlan OrchestrationPlan { get; set; }

		[JsonProperty("parameters")]
		public string Parameters { get; set; }

		[JsonProperty("priority")]
		public string Priority { get; set; }

		[JsonProperty("project")]
		public Project Project { get; set; }

		[JsonProperty("queue")]
		public Queue Queue { get; set; }

		[JsonProperty("queueTime")]
		public DateTime QueueTime { get; set; }

		[JsonProperty("reason")]
		public string Reason { get; set; }

		[JsonProperty("repository")]
		public Repository Repository { get; set; }

		[JsonProperty("requestedBy")]
		public Principal RequestedBy { get; set; }

		[JsonProperty("requestedFor")]
		public Principal RequestedFor { get; set; }

		[JsonProperty("result")]
		public string Result { get; set; }

		[JsonProperty("sourceBranch")]
		public string SourceBranch { get; set; }

		[JsonProperty("sourceVersion")]
		public string SourceVersion { get; set; }

		[JsonProperty("startTime")]
		public DateTime StartTime { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("uri")]
		public string Uri { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}
