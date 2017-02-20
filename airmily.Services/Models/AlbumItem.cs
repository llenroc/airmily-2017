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
		[JsonProperty("image")]
		public string ImageName { get; set; }
		[JsonProperty("albumID")]
		public string Album { get; set; }
		[JsonProperty("receipt")]
		public bool IsReceipt { get; set; }
		[JsonProperty("uri")]
		public string Address { get; set; }

		[JsonIgnore]
		public byte[] Image { get; set; }
		[JsonIgnore]
		public bool IsAddButton { get; set; }
		[JsonIgnore]
	    public ImageSource ImageSrc
	    {
		    get
		    {
				return !IsAddButton ? ImageSource.FromUri(new Uri(Address)) : ImageSource.FromFile("AddImageIcon.png");
		    }
	    }

	    public AlbumItem()
	    {
		    IsAddButton = false;
	    }
	}
}
