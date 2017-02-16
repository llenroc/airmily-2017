using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using airmily.Services.Models;
using Microsoft.WindowsAzure.Storage.Table;

namespace airmily.Services.Azure
{
	public class Azure : IAzure
	{
		private readonly bool debugging = false;

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
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		//Public Methods
		//Cards
		public async Task<bool> UpdateAllCards(User credentials)
		{
			if (!credentials.Active)
				return false;

			Task<List<FFXCard>> liveTask = GetLiveCards(credentials.FairFX);
			Task<List<Card>> oldTask = GetAllCards(credentials.UserID, true);

			await Task.WhenAll(liveTask, oldTask);

			List<FFXCard> liveList = await liveTask;
			List<Card> oldList = await oldTask;

			bool ret = false;
			foreach (FFXCard card in liveList)
			{
				Card[] oldCards = oldList.Where(c => c.CardID == card.CardID).ToArray();
				if (oldCards.Length > 0)
				{
					if (oldCards.Length > 1) throw new Exception("Multiple cards were found with the ID " + card.CardID + " when there should only be one.");

					if (oldCards[0].Update(card))
						await _cardsTable.UpdateAsync(oldCards[0]);		// Not properly tested

					continue;
				}
				
				await _cardsTable.InsertAsync(new Card(card, credentials.UserID));
				ret = true;
			}
			return ret;
		}
		public async Task<List<Card>> GetAllCards(string userid, bool all = false)
		{
			if (string.IsNullOrEmpty(userid))
				return new List<Card>();

			IMobileServiceTableQuery<Card> query = !all ? _cardsTable.Where(c => c.UserID == userid && c.Active == true) : _cardsTable.Where(c => c.UserID == userid);

			return (await query.ToListAsync()).OrderBy(c => c.CardHolder).ToList();
		}

		//Transactions
		public async Task<bool> UpdateAllTransactions(User credentials, string cardid)
		{
			if (!credentials.Active)
				return false;

			// Get the live and database transactions
			Task<List<FFXTransaction>> liveTask = GetLiveTransactions(credentials.FairFX, cardid);
			Task<List<Transaction>> oldTask = GetAllTransactions(cardid, true);

			await Task.WhenAll(liveTask, oldTask);

			List<FFXTransaction> liveList = await liveTask;
			List<Transaction> oldList = await oldTask;

			// Sort them by date
			try
			{
				System.Diagnostics.Debug.WriteLineIf(debugging, "Sorting 1 " + DateTime.Now.Millisecond);
				liveList = liveList.OrderBy(t => t.TransDate).ToList();
				System.Diagnostics.Debug.WriteLineIf(debugging, "Sorting 2 " + DateTime.Now.Millisecond);
				oldList = oldList.OrderBy(t => t.TransDate).ToList();
				System.Diagnostics.Debug.WriteLineIf(debugging, "Sorting 3 " + DateTime.Now.Millisecond);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return false;
			}

			// Get the live transactions after and on that date
			string date = oldList.Count > 0 ? oldList.Last().TransDate.Split('T')[0] : "2017-01-01";
			DateTime latestDate = new DateTime(Convert.ToInt32(date.Split('-')[0]), Convert.ToInt32(date.Split('-')[1]), Convert.ToInt32(date.Split('-')[2]));
			List<FFXTransaction> newerTransactions = liveList.Where(
				t => new DateTime(
						 Convert.ToInt32(t.TransDate.Split('T')[0].Split('-')[0]),
						 Convert.ToInt32(t.TransDate.Split('T')[0].Split('-')[1]),
						 Convert.ToInt32(t.TransDate.Split('T')[0].Split('-')[2])
					 )
					 > latestDate).ToList();
			List<FFXTransaction> samedateTransactions = liveList.Where(
				t => new DateTime(
						 Convert.ToInt32(t.TransDate.Split('T')[0].Split('-')[0]),
						 Convert.ToInt32(t.TransDate.Split('T')[0].Split('-')[1]),
						 Convert.ToInt32(t.TransDate.Split('T')[0].Split('-')[2])
					 ).Date
					 == latestDate.Date).ToList();

			// Check whether the transactions on the same date are on the database or not
			List<FFXTransaction> toCreate = new List<FFXTransaction>();
			toCreate.AddRange(newerTransactions);
			foreach (FFXTransaction t in samedateTransactions)
			{
				bool add = true;
				foreach (FFXTransaction old in oldList)
				{
					if (t == old)
					{
						add = false;
						break;
					}
				}

				if (add)	//Debugging here
					toCreate.Add(t);
			}

			// Add the new transactions
			foreach (FFXTransaction t in toCreate)
			{
				Transaction newT = new Transaction(t, cardid, credentials.UserID);
				await _transTable.InsertAsync(newT);
			}

			return toCreate.Count > 0;
		}
		public async Task<List<Transaction>> GetAllTransactions(string cardid, bool all = false)
		{
			if (string.IsNullOrEmpty(cardid))
				return new List<Transaction>();

			IMobileServiceTableQuery<Transaction> query = !all ? _transTable.Where(t => t.CardID == cardid && t.Description != "Card Load" && !t.Description.StartsWith("Card Transfer") && !t.Deleted) : _transTable.Where(t => t.CardID == cardid && !t.Deleted);

			return (await query.ToListAsync()).OrderByDescending(t => t.TransDate).ToList();
		}

		//Images
		public async Task<bool> UploadImage(AlbumItem image)
		{
			if (image.Image == null || string.IsNullOrEmpty(image.ImageName))
				return false;

			CloudBlockBlob blob = _storageContainer.GetBlockBlobReference(image.ImageName);
			blob.Properties.ContentType = "image/jpeg";	//Might need to fetch

			Task imageTask = blob.UploadFromStreamAsync(new MemoryStream(image.Image));
			Task albumTask = _albumTable.InsertAsync(image);

			await Task.WhenAll(imageTask, albumTask);
			return true;
		}
		public async Task<List<AlbumItem>> GetAllImages(string albumid)
		{
			if (string.IsNullOrEmpty(albumid))
				return new List<AlbumItem>();

			IMobileServiceTableQuery<AlbumItem> query = _albumTable.Where(a => a.Album == albumid);
			List<AlbumItem> album = await query.ToListAsync();

			#region Download Images
			foreach (AlbumItem item in album)
			{
				if (item.Image == null)
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
		public async Task<List<AlbumItem>> GetAllImages(string albumid, bool receipts)
		{
			if (string.IsNullOrEmpty(albumid))
				return new List<AlbumItem>();

			IMobileServiceTableQuery<AlbumItem> query = _albumTable.Where(a => a.Album == albumid && a.IsReceipt == receipts);
			List<AlbumItem> album = await query.ToListAsync();

			#region Download Images
			foreach (AlbumItem item in album)
			{
				if (item.Image == null)
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

		//Temporary Methods
		/*public async Task AddItem()
		{
			//await _usersTable.InsertAsync(new User
			//{
			//	UserID = "588842",
			//	UserName = "Suzy",
			//	UnionID = "",
			//	OpenID = "",
			//	FairFX = "c3V6eS5waWVyY2VAYmVpZXIzNjAuY29tQEp1TGkyMjM=",
			//	Active = true
			//});
			//await _albumTable.InsertAsync(new AlbumItem
			//{
			//	Album = "98C597C2-7322-4D87-A95F-974F513DBFC4",
			//	ImageName = "Doxie 0124.jpg",
			//	IsReceipt = true
			//});
		}*/

		//Private Methods
		private async Task<List<FFXTransaction>> GetLiveTransactions(string ffx, string card)
		{
			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 1 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			string[] creds = DecodeCredentials(ffx);
			if (creds == null) return new List<FFXTransaction>();

			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 2 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			using (HttpClient client = new HttpClient() { BaseAddress = new Uri("https://restapi.fairfx.com") })
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("username", creds[0]),
					new KeyValuePair<string, string>("password", creds[1]),
					new KeyValuePair<string, string>("domain", "corporate")
				});

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 3 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage loginResp = await client.PostAsync("/rest/auth", loginContent);
				if (!loginResp.IsSuccessStatusCode) return new List<FFXTransaction>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 4 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				FFXSession session = JsonConvert.DeserializeObject<FFXSession>(await loginResp.Content.ReadAsStringAsync());

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 5 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage transResp = await client.GetAsync("/rest/card/transactions/" + card + "/-/" + session.SessionID);     // Lasts at least 7-8 seconds

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 6 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				List<FFXTransaction> ret = transResp.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<FFXTransaction>>(await transResp.Content.ReadAsStringAsync()) : new List<FFXTransaction>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 7 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				return ret;
			}
		}

		private async Task<List<FFXCard>> GetLiveCards(string ffx)
		{
			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 1 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			string[] creds = DecodeCredentials(ffx);
			if (creds == null) return new List<FFXCard>();

			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 2 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			using (HttpClient client = new HttpClient() { BaseAddress = new Uri("https://restapi.fairfx.com") })
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("username", creds[0]),
					new KeyValuePair<string, string>("password", creds[1]),
					new KeyValuePair<string, string>("domain", "corporate")
				});

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 3 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage loginResp = await client.PostAsync("/rest/auth", loginContent);
				if (!loginResp.IsSuccessStatusCode) return new List<FFXCard>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 4 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				FFXSession session = JsonConvert.DeserializeObject<FFXSession>(await loginResp.Content.ReadAsStringAsync());

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 5 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage cardsResp = await client.GetAsync("/rest/card/list/-/" + session.SessionID);

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 6 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				List<FFXCard> ret = cardsResp.IsSuccessStatusCode
					? JsonConvert.DeserializeObject<List<FFXCard>>(await cardsResp.Content.ReadAsStringAsync())
					: new List<FFXCard>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 7 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				return ret;
			}
		}

		private static string EncodeCredentials(string email, string pass)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(email + "@" + pass));
		}

		private static string[] DecodeCredentials(string encrypted)
		{
			string[] unencrypted = Convert.FromBase64String(encrypted).Aggregate("", (current, b) => current + (char)b).Split('@');
			return (unencrypted[1] != "beier360.com" || unencrypted.Length != 3) ? null : new[] { unencrypted[0] + "@" + unencrypted[1], unencrypted[2] };
			//return (unencrypted[0] != SALT || unencrypted[2] != "beier360.com" || unencrypted.Length != 4) ? null : unencrypted;
		}
	}
}
