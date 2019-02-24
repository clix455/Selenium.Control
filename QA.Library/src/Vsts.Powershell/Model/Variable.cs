namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class Variable
	{
		[JsonProperty("allowOverride")]
		public bool AllowOverride { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("isSecret")]
		public bool IsSecret { get; set; }
	}
}