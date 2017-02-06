using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class AlbumItem : BaseSchema
	{
		[JsonProperty("image")]
		public string ImageName { get; set; }
		[JsonProperty("albumID")]
		public string Album { get; set; }
		[JsonProperty("receipt")]
		public bool IsReceipt { get; set; }
		[JsonIgnore]
		public byte[] Image { get; set; }

		public AlbumItem()
		{

		}
	}
}
