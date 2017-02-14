using System;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class FFXCard
	{
		[JsonProperty("id")]
		public string CardID { get; set; }

		[JsonProperty("obscured")]
		public string Number { get; set; }

		[JsonProperty("cardholder")]
		public FFXCardHolder CardHolder { get; set; }

		[JsonProperty("currency")]
		public FFXCurrency Curr { get; set; }

		[JsonProperty("balance")]
		public string Available { get; set; }

		[JsonProperty("blocked")]
		public string Blocked { get; set; }

		[JsonProperty("status")]
		public FFXStatus Status { get; set; }

	}

	public class FFXCardHolder
	{
		[JsonProperty("firstname")]
		public string Firstname { get; set; }
		[JsonProperty("surname")]
		public string Surname { get; set; }

		public override string ToString()
		{
			return Firstname + " " + Surname;
		}
	}

	public class FFXStatus
	{
		[JsonProperty("name")]
		public string Status { get; set; }
		[JsonIgnore]
		public bool Current { get { return Status.ToUpper() == "ACTIVE"; } }
	}

	public class FFXCurrency
	{
		[JsonProperty("symbol")]
		public string Symbol { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }
	}
}
