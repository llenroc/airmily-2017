using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class Transaction : BaseSchema
	{
		#region IDs
		[JsonProperty("albumID")]
		public string AlbumID { get; set; }
		[JsonProperty("userID")]
		public string UserID { get; set; }
		[JsonProperty("cardID")]
		public string CardID { get; set; }
		#endregion
		#region Desc
		[JsonProperty("description")]
		public string Description { get; set; }
		#endregion
		#region Money
		[JsonProperty("currency")]
		public string Currency { get; set; }
		[JsonProperty("transactionamount")]
		public string InternalDifference { get; set; }
		[JsonProperty("transactionbill")]
		public string Amount { get; set; }
		[JsonProperty("isdebit")]
		public bool NegativeAmount { get; set; }
		#endregion
		#region Dates
		[JsonProperty("transactiondate")]
		public string TransDate { get; set; }
		[JsonProperty("postdate")]
		public string PostDate { get; set; }
		#endregion

		public Transaction() { }
		public Transaction(string desc, string date, string post, string curr, string amou, bool ngtv, string diff, string card, string user, string albm)
		{
			Description = desc;
			TransDate = date;
			PostDate = post;
			Currency = curr;
			Amount = amou;
			NegativeAmount = ngtv;
			InternalDifference = diff;
			CardID = card;
			UserID = user;
			AlbumID = albm;
		}

		[JsonIgnore]
		public string Main { get { return Description; } }
		[JsonIgnore]
		public string Details { get { return TransDate.Split('T')[0]; } }
		[JsonIgnore]
		public string Value
		{
			get
			{
				string ret = NegativeAmount ? "-" : "";
				switch (Currency)
				{
					case "GBP":
					default:
						ret += "£";
						break;
					case "EUR":
						ret += "€";
						break;
					case "USD":
						ret += "$";
						break;
				}
				ret += Amount;
				return ret;
			}
		}
	}
}