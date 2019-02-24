namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Project
	{
		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("revision")]
		public int Revision { get; set; }

		[JsonProperty("state")]
		public string State { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}
