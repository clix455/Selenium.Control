namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class BuildDefinition
	{
		[JsonProperty("quality")]
		public string Quality { get; set; }

		[JsonProperty("authoredBy")]
		public Principal AuthoredBy { get; set; }

		[JsonProperty("queue")]
		public Queue Queue { get; set; }

		[JsonProperty("uri")]
		public string Uri { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("revision")]
		public int Revision { get; set; }

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("project")]
		public Project Project { get; set; }
	}
}
