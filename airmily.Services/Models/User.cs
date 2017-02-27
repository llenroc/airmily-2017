using airmily.Services.Interfaces;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class User : EntityDataOfflineSync, IUser
	{
		[JsonProperty]
		public string UserName { get; set; }
		[JsonProperty]
		public string UserID { get; set; }
		[JsonProperty]
		public string UnionID { get; set; }
		[JsonProperty]
		public string OpenID { get; set; }
		[JsonProperty]
		public string FairFX { get; set; }
		[JsonProperty]
		public bool Active { get; set; }

		public User() { }
	}
}
