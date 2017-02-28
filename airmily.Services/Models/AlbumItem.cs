using System;
using System.IO;
using airmily.Services.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace airmily.Services.Models
{
	[JsonObject]
	public class AlbumItem : EntityDataOfflineSync, IAlbumItem
	{
		[JsonProperty]
		public string ImageName { get; set; }
		[JsonProperty]
		public string Album { get; set; }
		[JsonProperty]
		public bool IsReceipt { get; set; }
		[JsonProperty]
		public byte[] Image { get; set; }

	    public AlbumItem()
	    {
		    IsAddButton = false;
	    }

		[JsonIgnore]
		public bool IsAddButton { get; set; }
		[JsonIgnore]
	    public ImageSource ImageSrc
	    {
		    get
		    {
			    if (IsAddButton)
				    return ImageSource.FromFile("AddImageIcon.png");

			    return Image == null
				    ? ImageSource.FromUri(new Uri("https://marvelheroes.com/sites/default/files/styles/character_profile_266x405/public/character/image/Store_Thing_Classic.png"))
				    : ImageSource.FromStream(() => new MemoryStream(Image));
		    }
	    }
	}
}
