using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;
using Newtonsoft.Json;

namespace airmily.AppService.DataObjects
{
	[JsonObject]
	public class Transaction : EntityData, ITransaction, IFFXTransaction
	{
		[JsonProperty("userID")]
		public string UserID { get; set; }

		[JsonProperty("cardID")]
		public string CardID { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("currency")]
		public string Currency { get; set; }

		[JsonProperty("transactionamount")]
		public string InternalDifference { get; set; }

		[JsonProperty("transactionbill")]
		public string Amount { get; set; }

		[JsonProperty("isdebit")]
		public bool NegativeAmount { get; set; }

		[JsonProperty("transactiondate")]
		public string TransDate { get; set; }

		[JsonProperty("postdate")]
		public string PostDate { get; set; }
	}
}