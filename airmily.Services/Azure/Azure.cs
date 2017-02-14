using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
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
		public async Task<bool> UpdateCards(User credentials)
		{
			if (!credentials.Active)
				return false;

			Task<List<FFXCard>> liveTask = GetLiveCards(credentials.FairFX);
			Task<List<Card>> oldTask = GetCards(credentials.UserID, true);

			await Task.WhenAll(liveTask, oldTask);

			List<FFXCard> liveList = await liveTask;
			List<Card> oldList = await oldTask;

			bool ret = false;
			foreach (FFXCard card in liveList)
			{
				if (oldList.Where(c => c.CardID == card.CardID).ToArray().Length > 0)
					continue;
				
				await _cardsTable.InsertAsync(new Card(card, credentials.UserID));
				ret = true;
			}
			return ret;
		}

		public async Task<List<Card>> GetCards(string userid, bool all = false)
		{
			if (string.IsNullOrEmpty(userid))
				return new List<Card>();

			IMobileServiceTableQuery<Card> query = !all ? _cardsTable.Where(c => c.UserID == userid && c.Active == true) : _cardsTable.Where(c => c.UserID == userid);

			return (await query.ToListAsync()).OrderBy(c => c.CardHolder).ToList();
		}

		public async Task<bool> UpdateTransactions(User credentials, string cardid)
		{
			if (!credentials.Active)
				return false;

			// Get the live and database transactions
			Task<List<FFXTransaction>> liveTask = GetLiveTransactions(credentials.FairFX, cardid);
			Task<List<Transaction>> oldTask = GetTransactions(cardid, true);

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

				if (add)
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

		public async Task<List<Transaction>> GetTransactions(string cardid, bool all = false)
		{
			if (string.IsNullOrEmpty(cardid))
				return new List<Transaction>();

			IMobileServiceTableQuery<Transaction> query = !all ? _transTable.Where(t => t.CardID == cardid && t.Description != "Card Load" && !t.Description.StartsWith("Card Transfer") && !t.Deleted) : _transTable.Where(t => t.CardID == cardid && !t.Deleted);

			return (await query.ToListAsync()).OrderByDescending(t => t.TransDate).ToList();
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

		public async Task<List<AlbumItem>> GetImages(string albumid)
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

		//Temporary Methods
		public async Task AddItem()
		{
			await _usersTable.InsertAsync(new User
			{
				UserID = "588842",
				UserName = "Suzy",
				UnionID = "",
				OpenID = "",
				FairFX = "c3V6eS5waWVyY2VAYmVpZXIzNjAuY29tQEp1TGkyMjM=",
				Active = true
			});
			await _cardsTable.InsertAsync(new Card
			{
				CardID = "542096",
				UserID = "588842",
				Number = "5116********2229",
				CardHolder = "Suzy",
				Currency = "$",
				Balance = "0",
				Active = true
			});
			await _cardsTable.InsertAsync(new Card
			{
				CardID = "465525",
				UserID = "588842",
				Number = "5116********5694",
				CardHolder = "Suzy",
				Currency = "£",
				Balance = "111.52",
				Active = true
			});
		}

		//Private Methods
		private async Task<List<FFXTransaction>> GetLiveTransactions(string ffx, string card)
		{
			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 1 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			string[] creds = Convert.FromBase64String(ffx).Aggregate("", (current, b) => current + (char)b).Split('@');
			if (creds[1] != "beier360.com") return new List<FFXTransaction>();

			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 2 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			using (HttpClient http = new HttpClient() { BaseAddress = new Uri("https://restapi.fairfx.com") })
			{
				http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("username", creds[0] + "@" + creds[1]),
					new KeyValuePair<string, string>("password", creds[2]),
					new KeyValuePair<string, string>("domain", "corporate")
				});

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 3 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage loginResp = await http.PostAsync("/rest/auth", loginContent);
				if (!loginResp.IsSuccessStatusCode) return new List<FFXTransaction>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 4 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				FFXSession session = JsonConvert.DeserializeObject<FFXSession>(await loginResp.Content.ReadAsStringAsync());

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 5 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage transResp = await http.GetAsync("/rest/card/transactions/" + card + "/-/" + session.SessionID);     // Lasts at least 7-8 seconds

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 6 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				List<FFXTransaction> ret = transResp.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<FFXTransaction>>(await transResp.Content.ReadAsStringAsync()) : new List<FFXTransaction>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 7 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				return ret;
			}
		}

		private async Task<List<FFXCard>> GetLiveCards(string ffx)
		{
			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 1 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			string[] creds = Convert.FromBase64String(ffx).Aggregate("", (current, b) => current + (char)b).Split('@');
			if (creds[1] != "beier360.com") return new List<FFXCard>();

			System.Diagnostics.Debug.WriteLineIf(debugging, "Live 2 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			using (HttpClient http = new HttpClient() { BaseAddress = new Uri("https://restapi.fairfx.com") })
			{
				http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("username", creds[0] + "@" + creds[1]),
					new KeyValuePair<string, string>("password", creds[2]),
					new KeyValuePair<string, string>("domain", "corporate")
				});

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 3 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage loginResp = await http.PostAsync("/rest/auth", loginContent);
				if (!loginResp.IsSuccessStatusCode) return new List<FFXCard>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 4 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				FFXSession session = JsonConvert.DeserializeObject<FFXSession>(await loginResp.Content.ReadAsStringAsync());

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 5 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage cardsResp = await http.GetAsync("/rest/card/list/-/" + session.SessionID);

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 6 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				List<FFXCard> ret = cardsResp.IsSuccessStatusCode
					? JsonConvert.DeserializeObject<List<FFXCard>>(await cardsResp.Content.ReadAsStringAsync())
					: new List<FFXCard>();

				System.Diagnostics.Debug.WriteLineIf(debugging, "Live 7 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				return ret;
			}
		}
	}
}
