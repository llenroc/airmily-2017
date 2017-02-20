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
		public string Date { get; set; }

		[JsonIgnore]
		public User From { get; set; }

		public Comment() { }

		[JsonIgnore]
		public string Main
		{
			get { return Message; }
		}

		[JsonIgnore]
		public string Detail
		{
			get { return Date; }
		}

		[JsonIgnore]
		public string Value
		{
			get { return From.UserName ?? "Unknown"; }
		}
	}
}
