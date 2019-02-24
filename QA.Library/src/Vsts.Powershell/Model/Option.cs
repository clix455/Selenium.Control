namespace Clix.Vsts.Powershell.Model
{
	using System.Collections.Generic;

	using Newtonsoft.Json;

	public class Option
	{
		[JsonProperty("definition")]
		public Definition Definition { get; set; }

		[JsonProperty("enabled")]
		public bool Enabled { get; set; }

		[JsonProperty("inputs")]
		public Dictionary<string, string> Inputs { get; set; }
	}
}