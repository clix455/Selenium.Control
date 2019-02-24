namespace Clix.Vsts.Powershell.Model
{
	using System;
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class Record
	{

		[JsonProperty("changeId")]
		public int ChangeId { get; set; }

		[JsonProperty("currentOperation")]
		public object CurrentOperation { get; set; }

		[JsonProperty("details")]
		public Details Details { get; set; }

		[JsonProperty("errorCount")]
		public int ErrorCount { get; set; }

		[JsonProperty("finishTime")]
		public DateTime FinishTime { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("lastModified")]
		public DateTime LastModified { get; set; }

		[JsonProperty("log")]
		public LogInformation Log { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("order")]
		public int Order { get; set; }

		[JsonProperty("parentId")]
		public string ParentId { get; set; }

		[JsonProperty("percentComplete")]
		public int? PercentComplete { get; set; }

		[JsonProperty("result")]
		public string Result { get; set; }

		[JsonProperty("resultCode")]
		public object ResultCode { get; set; }

		[JsonProperty("startTime")]
		public DateTime StartTime { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("url")]
		public object Url { get; set; }

		[JsonProperty("warningCount")]
		public int WarningCount { get; set; }

		[JsonProperty("workerName")]
		public string WorkerName { get; set; }

		[JsonProperty("issues")]
		public IList<Issue> Issues { get; set; }
	}
}