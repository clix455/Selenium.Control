namespace Clix.Vsts.Powershell.Model
{
	using System;
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class BuildTimelineDetails
	{

		[JsonProperty("changeId")]
		public int ChangeId { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("lastChangedBy")]
		public string LastChangedBy { get; set; }

		[JsonProperty("lastChangedOn")]
		public DateTime LastChangedOn { get; set; }

		[JsonProperty("records")]
		public IList<Record> Records { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}
