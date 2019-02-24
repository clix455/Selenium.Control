namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Issue
	{

		[JsonProperty("category")]
		public string Category { get; set; }

		[JsonProperty("data")]
		public Data Data { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }
	}
}