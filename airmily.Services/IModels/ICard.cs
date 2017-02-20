﻿namespace airmily.Services.IModels
{
	public interface ICard
	{
		string CardID { get; set; }

		string UserID { get; set; }

		string CardHolder { get; set; }

		string Number { get; set; }

		string Currency { get; set; }

		string Balance { get; set; }

		bool Active { get; set; }
	}
}