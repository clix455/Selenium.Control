namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Details
	{

		[JsonProperty("changeId")]
		public int ChangeId { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}