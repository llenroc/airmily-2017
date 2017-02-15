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

		private ImageSource _imageSrc;
	    public ImageSource ImageSrc
	    {
		    get { return _imageSrc == null ? ImageSource.FromStream(() => new MemoryStream(Image)) : _imageSrc; }
		    set { if (_imageSrc != value) _imageSrc = value; }
	    }

	    public AlbumItem()
		{

		}
	}
}
