namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class OrchestrationPlan
	{

		[JsonProperty("planId")]
		public string PlanId { get; set; }
	}
}