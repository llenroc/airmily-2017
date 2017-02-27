using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.DataObjects
{
	public class User : EntityData, IUser
	{
		public string UserName { get; set; }
		public string UserID { get; set; }
		public string UnionID { get; set; }
		public string OpenID { get; set; }
		public string FairFX { get; set; }
		public bool Active { get; set; }
	}
}
