using Newtonsoft.Json;

namespace airmily.Services.Models
{
	[JsonObject]
	public class Card : BaseSchema
	{
		[JsonProperty("number")]
		public string Number { get; set; }
		[JsonProperty("cardID")]
		public string CardID { get; set; }
		[JsonProperty("balance")]
		public string Balance { get; set; }
		[JsonProperty("currency")]
		public string Currency { get; set; }
		[JsonProperty("user")]
		public string CardHolder { get; set; }
		[JsonProperty("userID")]
		public string UserID { get; set; }
		[JsonProperty("active")]
		public bool Active { get; set; }

		public Card() { }

		[JsonIgnore]
		public string Main { get { return CardHolder; } }
		[JsonIgnore]
		public string Details { get { return Number; } }
		[JsonIgnore]
		public string Value { get { return Currency + Balance; } }
	}
}
