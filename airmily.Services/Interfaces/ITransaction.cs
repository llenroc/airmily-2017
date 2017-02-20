namespace airmily.Services.Interfaces
{
	public interface ITransaction
	{
		string UserID { get; set; }

		string CardID { get; set; }
	}
}