using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace airmily.AppService.DataObjects
{
	[JsonObject]
	public class AlbumItem : EntityData, IAlbumItem
	{
		[JsonProperty("image")]
		public string ImageName { get; set; }

		[JsonProperty("albumID")]
		public string Album { get; set; }

		[JsonProperty("receipt")]
		public bool IsReceipt { get; set; }

		[JsonProperty("uri")]
		public string Address { get; set; }
	}
}
