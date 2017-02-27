using System;
using airmily.Services.Interfaces;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace airmily.Services.Models
{
	[JsonObject]
	public class Card : EntityDataOfflineSync, ICard
	{
		[JsonProperty]
		public string CardID { get; set; }
		[JsonProperty]
		public string UserID { get; set; }
		[JsonProperty]
		public string User { get; set; }
		[JsonProperty]
		public string Number { get; set; }
		[JsonProperty]
		public string Currency { get; set; }
		[JsonProperty]
		public string Balance { get; set; }
		[JsonProperty]
		public bool Active { get; set; }

		public Card() { }
		public Card(FFXCard card, string user)
		{
			CardID = card.CardID;
			UserID = user;
			User = card.CardHolder.Firstname + " " + card.CardHolder.Surname;
			Number = card.Number;
			Currency = card.Currency.Code;
			Balance = (Convert.ToDouble(card.Available) - Convert.ToDouble(card.Blocked)).ToString("F");
			Active = card.CardStatus.Current;
		}
		public bool Update(FFXCard c)
		{
			bool changed = false;

			string newBalance = (Convert.ToDouble(c.Available) - Convert.ToDouble(c.Blocked)).ToString("F");
			bool newActive = c.CardStatus.Current;

			if (Active != newActive)
			{
				Active = newActive;
				changed = true;
			}
			if (Balance != newBalance)
			{
				Balance = newBalance;
				changed = true;
			}

			return changed;
		}

		[JsonIgnore]
		public string Main { get { return User; } }
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
