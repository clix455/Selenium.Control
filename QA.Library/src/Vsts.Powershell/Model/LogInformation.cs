namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class LogInformation
	{

		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}