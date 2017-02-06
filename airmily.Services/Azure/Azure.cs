using System;
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
		//Singleton
		public static Azure CurrentInstance
		{
			get
			{
				return currentInstance;
			}
			private set
			{
				currentInstance = value;
			}
		}
		private static Azure currentInstance = new Azure();

		//Mobile
		private MobileServiceClient mobileClient;
		private IMobileServiceTable<User> usersTable;
		private IMobileServiceTable<Card> cardsTable;
		private IMobileServiceTable<Transaction> transTable;
		private IMobileServiceTable<AlbumItem> albumTable;

		//Storage
		private CloudStorageAccount storageAccount;
		private CloudBlobClient storageClient;
		private CloudBlobContainer storageContainer;

		//Ctor
		public Azure()
		{
			mobileClient = new MobileServiceClient("https://airmilyapp.azurewebsites.net");

			usersTable = mobileClient.GetTable<User>();
			cardsTable = mobileClient.GetTable<Card>();
			transTable = mobileClient.GetTable<Transaction>();
			albumTable = mobileClient.GetTable<AlbumItem>();

			storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=airmilystorage;AccountKey=" + "RRdg9CkiTZVa6DNI5erUaRaAOiU6yAfUhxu0Hd7yZHAd5XAO/EvUyhvXBcrwUXt4QiHGZfQsbI6cZYeaFnS/2A==");
			storageClient = storageAccount.CreateCloudBlobClient();
			storageContainer = storageClient.GetContainerReference("images");
		}

		//Methods
		public async Task<List<Card>> GetCards(string userid, bool all = false)
		{
			IMobileServiceTableQuery<Card> query;
			if (!all)
				query = cardsTable.Where(c => c.UserID == userid && c.Active == true);
			else
				query = cardsTable.Where(c => c.UserID == userid);

			return await query.ToListAsync();
		}

		public async Task<List<Transaction>> GetTransactions(string cardid, bool all = false)
		{
			IMobileServiceTableQuery<Transaction> query;
			if (!all)
				query = transTable.Where(t => t.CardID == cardid && t.Description != "Card Load" && !t.Description.StartsWith("Card Transfer"));
			else
				query = transTable.Where(t => t.CardID == cardid);

			return await query.ToListAsync();
		}

		public async Task<List<AlbumItem>> GetImages(string albumid)
		{
			IMobileServiceTableQuery<AlbumItem> query = albumTable.Where(a => a.Album == albumid);
			List<AlbumItem> album = await query.ToListAsync();

			#region Download Images
			foreach (AlbumItem item in album)
			{
				if (item.Image == null)
				{
					CloudBlockBlob blob = storageContainer.GetBlockBlobReference(item.ImageName);
					await blob.FetchAttributesAsync();

					item.Image = new byte[blob.Properties.Length];
					await blob.DownloadToByteArrayAsync(item.Image, 0);
				}
			}
			#endregion
			return album;
		}
	}
}
