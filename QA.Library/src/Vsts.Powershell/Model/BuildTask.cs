namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class BuildTask
	{
		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("versionSpec")]
		public string VersionSpec { get; set; }
	}
}