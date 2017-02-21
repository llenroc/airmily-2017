using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace airmily.AppService.DataObjects
{
	[JsonObject]
	public class Card : EntityData, ICard
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
	}
}
