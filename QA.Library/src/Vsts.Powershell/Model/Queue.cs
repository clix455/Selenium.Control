namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Queue
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("pool")]
		public Pool Pool { get; set; }
	}
}