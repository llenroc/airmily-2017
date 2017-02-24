using System;
using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace airmily.AppService.DataObjects
{
	[JsonObject]
	public class Comment : EntityData, IComment
	{
		[JsonProperty("imageid")]
		public string ImageID { get; set; }

		[JsonProperty("content")]
		public string Message { get; set; }

		[JsonProperty("userid")]
		public string UserID { get; set; }

		[JsonProperty("date")]
		public DateTime? Date { get; set; }
	}
}
