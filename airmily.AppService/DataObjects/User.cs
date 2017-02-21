using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace airmily.AppService.DataObjects
{
	[JsonObject]
	public class User : EntityData, IUser
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
	}
}
