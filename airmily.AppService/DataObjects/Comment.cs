using System;
using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.DataObjects
{
	public class Comment : EntityData, IComment
	{
		public string ImageID { get; set; }

		public string Message { get; set; }

		public string User { get; set; }

		public DateTime? Date { get; set; }
	}
}
