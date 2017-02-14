using System;
using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class Card : BaseSchema
	{
		[JsonProperty("cardID")]
		public string CardID { get; set; }
		[JsonProperty("userID")]
		public string UserID { get; set; }
		[JsonProperty("user")]
		public string CardHolder { get; set; }
		[JsonProperty("number")]
		public string Number { get; set; }
		[JsonProperty("currency")]
		public string Currency { get; set; }
		[JsonProperty("balance")]
		public string Balance { get; set; }
		[JsonProperty("active")]
		public bool Active { get; set; }

		public Card() { }

		public Card(FFXCard card, string user)
		{
			CardID = card.CardID;
			UserID = user;
			CardHolder = card.CardHolder.Firstname + " " + card.CardHolder.Surname;
			Number = card.Number;
			Currency = card.Curr.Code;
			Balance = (Convert.ToDouble(card.Available) - Convert.ToDouble(card.Blocked)).ToString("F");
			Active = card.Status.Current;
		}

		[JsonIgnore]
		public string Main { get { return CardHolder; } }
		[JsonIgnore]
		public string Details { get { return Number; } }
		[JsonIgnore]
		public string Value
		{
			get
			{
				string symbol;
				switch (Currency)
				{
					default:
						symbol = "£";
						break;
					case "EUR":
						symbol = "€";
						break;
					case "USD":
						symbol = "$";
						break;
				}
				return symbol + Balance;
			}
		}
	}
}
