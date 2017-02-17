using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using airmily.Services.Azure;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	public class Comment : BaseSchema
	{
		[JsonProperty("imageid")]
		public string ImageID { get; set; }

		[JsonProperty("content")]
		public string Message { get; set; }

		[JsonProperty("userid")]
		public string UserID { get; set; }


		[JsonIgnore]
		public User From { get; set; }

		public Comment()
		{
		}

		[JsonIgnore]
		public string Main
		{
			get { return Message; }
		}

		[JsonIgnore]
		public string Detail
		{
			get { return Created.ToString("HH:mm - dd MMM") ?? DateTime.Now.ToString("HH:mm - dd MMM"); }
		}

		[JsonIgnore]
		public string Value
		{
			get { return From.UserName ?? "Unknown"; }
		}
	}
}
