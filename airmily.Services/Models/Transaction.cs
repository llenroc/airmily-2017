using System;
using System.Linq;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class Transaction : FFXTransaction
	{
		[JsonProperty("albumID")]
		public string AlbumID { get; set; }
		[JsonProperty("userID")]
		public string UserID { get; set; }
		[JsonProperty("cardID")]
		public string CardID { get; set; }

		public Transaction() { }
		public Transaction(FFXTransaction t, string card, string user)
		{
			Description = t.Description;
			TransDate = t.TransDate;
			PostDate = t.PostDate;
			Currency = t.Currency;
			Amount = t.Amount;
			NegativeAmount = t.NegativeAmount;
			InternalDifference = t.InternalDifference;
			CardID = card;
			UserID = user;
		}
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
		public string Details
		{
			get
			{
				string date = TransDate.Split('T')[0];
				return string.Join("-", date.Split('-').Reverse());
			}
		}

		[JsonIgnore]
		public string Value
		{
			get
			{
				string ret = NegativeAmount ? "-" : "";
				switch (Currency)
				{
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
				ret += Convert.ToDouble(InternalDifference).ToString("F");

				if (InternalDifference != Amount)
					ret += Environment.NewLine + "£" + Convert.ToDouble(Amount).ToString("F");

				return ret;
			}
		}
	}
}