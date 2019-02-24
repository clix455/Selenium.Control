namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Principal
	{
		[JsonProperty("displayName")]
		public string DisplayName { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("imageUrl")]
		public string ImageUrl { get; set; }

		[JsonProperty("uniqueName")]
		public string UniqueName { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}