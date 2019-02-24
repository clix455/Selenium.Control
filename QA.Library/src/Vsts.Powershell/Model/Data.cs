namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Data
	{

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("TaskId")]
		public string TaskId { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }
	}
}