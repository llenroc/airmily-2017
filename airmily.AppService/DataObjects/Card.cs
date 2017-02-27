using airmily.Services.Interfaces;
using Microsoft.Azure.Mobile.Server;

namespace airmily.AppService.DataObjects
{
	public class Card : EntityData, ICard
	{
		public string CardID { get; set; }

		public string UserID { get; set; }

		public string User { get; set; }

		public string Number { get; set; }

		public string Currency { get; set; }

		public string Balance { get; set; }

		public bool Active { get; set; }
	}
}
