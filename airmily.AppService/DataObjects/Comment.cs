using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.DataObjects
{
	public class Comment : EntityData, IComment
	{
		public string ImageID { get; set; }

		public string Message { get; set; }

		public string UserID { get; set; }

		public string Date { get; set; }
	}
}
