using System;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class BaseSchema
	{
		[JsonProperty("id")]
		public string ID { get; set; }
		[JsonProperty("createdAt")]
		public DateTime Created { get; set; }
		[JsonProperty("updatedAt")]
		public DateTime LastUpdate { get; set; }
		[JsonProperty("deleted")]
		public bool Deleted { get; set; }
	}
}
