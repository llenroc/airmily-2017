using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using airmily.Services.Models;

namespace airmily.Services.Azure
{
	public class Azure : IAzure
	{
		//Mobile
		private MobileServiceClient _mobileClient;
		private IMobileServiceTable<User> _usersTable;
		private IMobileServiceTable<Card> _cardsTable;
		private IMobileServiceTable<Transaction> _transTable;
		private IMobileServiceTable<AlbumItem> _albumTable;

		//Storage
		private CloudStorageAccount _storageAccount;
		private CloudBlobClient _storageClient;
		private CloudBlobContainer _storageContainer;

		//Ctor
		public Azure()
		{
            try
			{
				_mobileClient = new MobileServiceClient("https://airmilyapp.azurewebsites.net");

				_usersTable = _mobileClient.GetTable<User>();
				_cardsTable = _mobileClient.GetTable<Card>();
				_transTable = _mobileClient.GetTable<Transaction>();
				_albumTable = _mobileClient.GetTable<AlbumItem>();

				_storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=airmilystorage;AccountKey=" + "RRdg9CkiTZVa6DNI5erUaRaAOiU6yAfUhxu0Hd7yZHAd5XAO/EvUyhvXBcrwUXt4QiHGZfQsbI6cZYeaFnS/2A==");
				_storageClient = _storageAccount.CreateCloudBlobClient();
				_storageContainer = _storageClient.GetContainerReference("images");
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		//Methods
		public async Task<List<Card>> GetCards(string userid, bool all = false)
		{
			if (string.IsNullOrEmpty(userid))
				return new List<Card>();

			IMobileServiceTableQuery<Card> query = !all ? _cardsTable.Where(c => c.UserID == userid && c.Active == true) : _cardsTable.Where(c => c.UserID == userid);

			return await query.ToListAsync();
		}

		public async Task<List<Transaction>> GetTransactions(string cardid, bool all = false)
		{
			if (string.IsNullOrEmpty(cardid))
				return new List<Transaction>();

			IMobileServiceTableQuery<Transaction> query = !all ? _transTable.Where(t => t.CardID == cardid && t.Description != "Card Load" && !t.Description.StartsWith("Card Transfer")) : _transTable.Where(t => t.CardID == cardid);

			return await query.ToListAsync();
		}

		public async Task<List<AlbumItem>> GetImages(string albumid)
		{
			if (string.IsNullOrEmpty(albumid))
				return new List<AlbumItem>();

			IMobileServiceTableQuery<AlbumItem> query = _albumTable.Where(a => a.Album == albumid);
			List<AlbumItem> album = await query.ToListAsync();

			#region Download Images
			foreach (AlbumItem item in album)
			{
				if (item.Image != null)
				{
					CloudBlockBlob blob = _storageContainer.GetBlockBlobReference(item.ImageName);
					await blob.FetchAttributesAsync();

					item.Image = new byte[blob.Properties.Length];
					await blob.DownloadToByteArrayAsync(item.Image, 0);
				}
			}
			#endregion
			return album;
		}

		public async Task<bool> UploadImage(AlbumItem image)
		{
			if (image.Image == null || string.IsNullOrEmpty(image.ImageName))
				return false;

			CloudBlockBlob blob = _storageContainer.GetBlockBlobReference(image.ImageName);
			await blob.UploadFromStreamAsync(new MemoryStream(image.Image));
			await _albumTable.InsertAsync(image);
			return true;
		}
	}
}
