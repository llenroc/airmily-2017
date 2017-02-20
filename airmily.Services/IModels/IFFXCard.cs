using airmily.Services.Models;

namespace airmily.Services.IModels
{
	public interface IFFXCard
	{
		string CardID { get; set; }

		string Number { get; set; }

		FFXCardHolder CardHolder { get; set; }

		FFXCardCurrency Currency { get; set; }
		
		string Available { get; set; }

		string Blocked { get; set; }

		FFXCardStatus CardStatus { get; set; }
	}
}