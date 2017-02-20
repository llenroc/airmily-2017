using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.DataObjects
{
	public class Transaction : EntityData, ITransaction, IFFXTransaction
	{
		public string UserID { get; set; }

		public string CardID { get; set; }

		public string Description { get; set; }

		public string Currency { get; set; }

		public string InternalDifference { get; set; }

		public string Amount { get; set; }

		public bool NegativeAmount { get; set; }

		public string TransDate { get; set; }

		public string PostDate { get; set; }
	}
}