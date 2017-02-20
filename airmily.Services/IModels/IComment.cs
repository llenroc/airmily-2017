using airmily.Services.Models;

namespace airmily.Services.IModels
{
	public interface IComment
	{
		string ImageID { get; set; }

		string Message { get; set; }
		
		string UserID { get; set; }

		User From { get; set; }
	}
}