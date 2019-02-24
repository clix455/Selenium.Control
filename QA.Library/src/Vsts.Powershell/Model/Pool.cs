namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Pool
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }
	}

}