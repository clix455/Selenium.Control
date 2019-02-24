namespace Clix.Vsts.Powershell.Model
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class Repository
	{
		[JsonProperty("checkoutSubmodules")]
		public bool CheckoutSubmodules { get; set; }

		[JsonProperty("clean")]
		public string Clean { get; set; }

		[JsonProperty("defaultBranch")]
		public string DefaultBranch { get; set; }

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("properties")]
		public Dictionary<string, string> Properties { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }
	}
}