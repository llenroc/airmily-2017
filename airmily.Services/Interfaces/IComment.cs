﻿namespace airmily.Services.Interfaces
{
	public interface IComment
	{
		string ImageID { get; set; }

		string Message { get; set; }
		
		string UserID { get; set; }

		string Date { get; set; }
	}
}