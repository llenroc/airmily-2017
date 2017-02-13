using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class FFXSession
	{
		[JsonProperty("sessionid")]
		public string SessionID { get; set; }
	}
}
