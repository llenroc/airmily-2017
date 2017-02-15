using System.IO;
using Newtonsoft.Json;
using Xamarin.Forms;

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
		[JsonIgnore]
		public bool IsAddButton { get; set; }

	    public ImageSource ImageSrc
	    {
		    get
		    {
			    return !IsAddButton ? ImageSource.FromStream(() => new MemoryStream(Image)) : ImageSource.FromFile("Icon-76.png");
		    }
	    }

	    public AlbumItem()
	    {
		    IsAddButton = false;
	    }
	}
}
