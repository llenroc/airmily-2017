using System;
using airmily.Services.Interfaces;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	public class Comment : EntityDataOfflineSync, IComment
	{
		[JsonProperty("imageid")]
		public string ImageID { get; set; }

		[JsonProperty("content")]
		public string Message { get; set; }

		[JsonProperty("userid")]
		public string UserID { get; set; }

		[JsonProperty("date")]
		public DateTime? Date { get; set; }

		[JsonIgnore]
		public User From { get; set; }

		public Comment() { CurrentType = GalleryType.Comment; }

		[JsonIgnore]
		public AlbumItem Image { get; set; }

		[JsonIgnore]
		public GalleryType CurrentType { get; set; }

		[JsonIgnore]
		public string Main
		{
			get { return Message; }
		}

		[JsonIgnore]
		public string Detail
		{
			get { return Date.HasValue ? Date.Value.ToString("HH:mm - dd MMM") : "Missing Date"; }
		}

		[JsonIgnore]
		public string Value
		{
			get { return From.UserName ?? "Unknown"; }
		}
	}

	public enum GalleryType
	{
		Image, Comment, AddComment
	};
}
