namespace Clix.Vsts.Powershell.Model
{
	using Newtonsoft.Json;

	public class ResponseError
	{
		[JsonProperty("$id")]
		public string Id { get; set; }

		[JsonProperty("errorCode")]
		public int ErrorCode { get; set; }

		[JsonProperty("eventId")]
		public int EventId { get; set; }

		[JsonProperty("innerException")]
		public object InnerException { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("typeKey")]
		public string TypeKey { get; set; }

		[JsonProperty("typeName")]
		public string TypeName { get; set; }
	}
}
