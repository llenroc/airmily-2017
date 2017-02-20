using airmily.Services.Interfaces;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class User : EntityDataOfflineSync, IUser
	{
		[JsonProperty("username")]
		public string UserName { get; set; }
		[JsonProperty("userID")]
		public string UserID { get; set; }
		[JsonProperty("unionID")]
		public string UnionID { get; set; }
		[JsonProperty("openID")]
		public string OpenID { get; set; }
		[JsonProperty("fairfx")]
		public string FairFX { get; set; }
		[JsonProperty("active")]
		public bool Active { get; set; }

		public User() { }
	}
}
