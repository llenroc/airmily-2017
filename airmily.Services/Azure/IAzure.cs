using System.Threading.Tasks;
using airmily.Services.Models;
using System.Collections.Generic;

namespace airmily.Services.Azure
{
	public interface IAzure
	{
		/// <summary>
		/// Returns a list of cards for a particular user.
		/// </summary>
		/// <param name="userid">The ID of the user</param>
		/// <param name="all">Whether to include users that are not in an Active status</param>
		/// <returns></returns>
		Task<List<Card>>		GetCards(string userid, bool all = false);
		/// <summary>
		/// Returns a list of transactions for a particular card.
		/// </summary>
		/// <param name="cardid">The ID of the card</param>
		/// <param name="all">Whether to include internal transactions or not (Card Load/Transfer)</param>
		/// <returns></returns>
		Task<List<Transaction>>	GetTransactions(string cardid, bool all = false);
		/// <summary>
		/// Returns a list of pictures for a particular transaction.
		/// This gets all of the attached images, which can then be sorted between goods and receipts
		/// </summary>
		/// <param name="albumid">The ID of the album (Should be replaced by the transaction ID?)</param>
		/// <returns></returns>
		Task<List<AlbumItem>>	GetImages(string albumid);
		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		Task<bool> UploadImage(AlbumItem item);
	}
}