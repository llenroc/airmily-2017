using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using airmily.Services.Models;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace airmily.Services.Azure
{
	public class Azure : IAzure
	{
		private readonly bool debugging = false;

		//Mobile
		private MobileServiceClient _mobileClient;
		private IMobileServiceSyncTable<User> _usersTable;
		private IMobileServiceSyncTable<Card> _cardsTable;
		private IMobileServiceSyncTable<Transaction> _transTable;
		private IMobileServiceSyncTable<AlbumItem> _albumTable;
		private IMobileServiceSyncTable<Comment> _commsTable;

		//Storage
		private CloudStorageAccount _storageAccount;
		private CloudBlobClient _storageClient;
		private CloudBlobContainer _storageContainer;

		//Ctor
		public Azure()
		{
			try
			{
				_mobileClient = new MobileServiceClient(AzureSettings.ApplicationUrl);

				var store = new MobileServiceSQLiteStore(AzureSettings.LocalSQLiteStore);
				store.DefineTable<User>();
				store.DefineTable<Card>();
				store.DefineTable<Transaction>();
				store.DefineTable<AlbumItem>();
				store.DefineTable<Comment>();
				_mobileClient.SyncContext.InitializeAsync(store);

				_usersTable = _mobileClient.GetSyncTable<User>();
				_cardsTable = _mobileClient.GetSyncTable<Card>();
				_transTable = _mobileClient.GetSyncTable<Transaction>();
				_albumTable = _mobileClient.GetSyncTable<AlbumItem>();
				_commsTable = _mobileClient.GetSyncTable<Comment>();

				_storageAccount = CloudStorageAccount.Parse(AzureSettings.StorageConnectionString);
				_storageClient = _storageAccount.CreateCloudBlobClient();
				_storageContainer = _storageClient.GetContainerReference("images");
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
			}
		}

		//Public Methods
		public async Task<List<User>> GetUser(string userid)
		{
			if (string.IsNullOrEmpty(userid))
				return new List<User>();

			await SyncAsync();

			IMobileServiceTableQuery<User> query = _usersTable.Where(u => u.UserID == userid);
			return await query.ToListAsync();
		}

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
						await _cardsTable.UpdateAsync(oldCards[0]);

					continue;
				}

				await _cardsTable.InsertAsync(new Card(card, credentials.UserID));
				ret = true;
			}
			await SyncAsync();
			return ret;
		}
		public async Task<List<Card>> GetAllCards(string userid, bool all = false)
		{
			if (string.IsNullOrEmpty(userid))
				return new List<Card>();

			await SyncAsync();

			IMobileServiceTableQuery<Card> query = !all ? _cardsTable.Where(c => c.UserID == userid && c.Active == true) : _cardsTable.Where(c => c.UserID == userid);

			return (await query.ToListAsync()).OrderBy(c => c.CardHolder).ToList();
		}

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
				Debug.WriteLineIf(debugging, "Sorting 1 " + DateTime.Now.Millisecond);
				liveList = liveList.OrderBy(t => t.TransDate).ToList();
				Debug.WriteLineIf(debugging, "Sorting 2 " + DateTime.Now.Millisecond);
				oldList = oldList.OrderBy(t => t.TransDate).ToList();
				Debug.WriteLineIf(debugging, "Sorting 3 " + DateTime.Now.Millisecond);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
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

				if (add)    //Debugging here
					toCreate.Add(t);
			}

			// Add the new transactions
			foreach (FFXTransaction t in toCreate)
			{
				Transaction newT = new Transaction(t, cardid, credentials.UserID);
				await _transTable.InsertAsync(newT);
			}
			await SyncAsync();

			return toCreate.Count > 0;
		}
		public async Task<List<Transaction>> GetAllTransactions(string cardid, bool all = false)
		{
			if (string.IsNullOrEmpty(cardid))
				return new List<Transaction>();

			await SyncAsync();

			IMobileServiceTableQuery<Transaction> query = all
				? _transTable.OrderByDescending(t => t.TransDate)
					.ThenByDescending(t => t.InternalDifference)
					.Where(t => t.CardID == cardid)
				: _transTable.OrderByDescending(t => t.TransDate)
					.ThenByDescending(t => t.InternalDifference)
					.Where(t => t.CardID == cardid && t.Description != "Card Load" && !t.Description.StartsWith("Card Transfer"));

			return await query.ToListAsync();
		}

		public async Task<bool> UploadImage(AlbumItem image)
		{
			if (image.Image == null || string.IsNullOrEmpty(image.ImageName))
				return false;

			if (!CrossConnectivity.Current.IsConnected) return false;

			CloudBlockBlob blob = _storageContainer.GetBlockBlobReference(image.ImageName);
			blob.Properties.ContentType = "image/jpeg";
			await blob.UploadFromStreamAsync(new MemoryStream(image.Image));

			image.Address = blob.StorageUri.PrimaryUri.ToString();
			await _albumTable.InsertAsync(image);
			await SyncAsync();

			return true;
		}
		public async Task<bool> DeleteImage(AlbumItem image)
		{
			//image.Deleted = true;
			//await _albumTable.UpdateAsync(image);
			await _albumTable.DeleteAsync(image);
			await SyncAsync();

			//CloudBlockBlob blob = _storageContainer.GetBlockBlobReference(image.ImageName);
			//Task imageTask = blob.DeleteIfExistsAsync();
			//Task albumTask = _albumTable.DeleteAsync(image);

			//await Task.WhenAll(imageTask, albumTask);
			return true;
		}
		public async Task<List<AlbumItem>> GetAllImages(string albumid)
		{
			if (string.IsNullOrEmpty(albumid))
				return new List<AlbumItem>();

			await SyncAsync();

			IMobileServiceTableQuery<AlbumItem> query = _albumTable.Where(a => a.Album == albumid);
			List<AlbumItem> album = await query.ToListAsync();

			return album;
		}
		public async Task<List<AlbumItem>> GetAllImages(string albumid, bool receipts)
		{
			if (string.IsNullOrEmpty(albumid))
				return new List<AlbumItem>();

			await SyncAsync();

			IMobileServiceTableQuery<AlbumItem> query = _albumTable.Where(a => a.Album == albumid && a.IsReceipt == receipts);
			List<AlbumItem> album = await query.ToListAsync();

			return album;
		}

		public async Task AddComment(Comment c)
		{
			if (string.IsNullOrEmpty(c.Message) || string.IsNullOrEmpty(c.UserID) || string.IsNullOrEmpty(c.ImageID))
				return;

			await _commsTable.InsertAsync(c);

			await SyncAsync();
		}
		public async Task<List<Comment>> GetComments(string imageid)
		{
			if (string.IsNullOrEmpty(imageid))
				return new List<Comment>();

			await SyncAsync();

			IMobileServiceTableQuery<Comment> query = _commsTable.Where(c => c.ImageID == imageid);
			List<Comment> ret = await query.ToListAsync();

			foreach (Comment c in ret)
				c.From = (await GetUser(c.UserID)).First();

			return ret;
		}

		public async Task SyncAsync()
		{
			if (!await CrossConnectivity.Current.IsReachable(AzureSettings.ApplicationUrl))
			{
				Debug.WriteLine("skipped sync");
				return;
			}

			ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
			try
			{
				await _mobileClient.SyncContext.PushAsync();
				await _usersTable.PullAsync("allUsers", _usersTable.CreateQuery());
				await _cardsTable.PullAsync("allCards", _cardsTable.CreateQuery());
				await _transTable.PullAsync("allTrans", _transTable.CreateQuery());
				await _albumTable.PullAsync("allAlbum", _albumTable.CreateQuery());
				await _commsTable.PullAsync("allComms", _commsTable.CreateQuery());
			}
			catch (MobileServicePushFailedException exc)
			{
				if (exc.PushResult != null)
				{
					syncErrors = exc.PushResult.Errors;
				}
			}

			// Simple error/conflict handling. A real application would handle the various errors like network conditions,
			// server conflicts and others via the IMobileServiceSyncHandler.
			if (syncErrors != null)
			{
				foreach (var error in syncErrors)
				{
					if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
					{
						//Update failed, reverting to server's copy.
						await error.CancelAndUpdateItemAsync(error.Result);
					}
					else
					{
						// Discard local change.
						await error.CancelAndDiscardItemAsync();
					}

					Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
				}
			}
		}

		//Private Methods
		private async Task<List<FFXTransaction>> GetLiveTransactions(string ffx, string card)
		{
			Debug.WriteLineIf(debugging, "Live 1 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);
			if (!await CrossConnectivity.Current.IsReachable(AzureSettings.FairFXUrl))
			{
				Debug.WriteLine("skipped transactions update");
				return new List<FFXTransaction>();
			}

			string[] creds = DecodeCredentials(ffx);
			if (creds == null) return new List<FFXTransaction>();

			Debug.WriteLineIf(debugging, "Live 2 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			using (HttpClient client = new HttpClient() { BaseAddress = new Uri(AzureSettings.FairFXUrl) })
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("username", creds[0]),
					new KeyValuePair<string, string>("password", creds[1]),
					new KeyValuePair<string, string>("domain", "corporate")
				});

				Debug.WriteLineIf(debugging, "Live 3 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage loginResp = await client.PostAsync("/rest/auth", loginContent);
				if (!loginResp.IsSuccessStatusCode) return new List<FFXTransaction>();

				Debug.WriteLineIf(debugging, "Live 4 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				FFXSession session = JsonConvert.DeserializeObject<FFXSession>(await loginResp.Content.ReadAsStringAsync());

				Debug.WriteLineIf(debugging, "Live 5 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage transResp = await client.GetAsync("/rest/card/transactions/" + card + "/-/" + session.SessionID);     // Lasts at least 7-8 seconds

				Debug.WriteLineIf(debugging, "Live 6 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				List<FFXTransaction> ret = transResp.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<FFXTransaction>>(await transResp.Content.ReadAsStringAsync()) : new List<FFXTransaction>();

				Debug.WriteLineIf(debugging, "Live 7 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				return ret;
			}
		}

		private async Task<List<FFXCard>> GetLiveCards(string ffx)
		{
			Debug.WriteLineIf(debugging, "Live 1 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);
			if (!await CrossConnectivity.Current.IsReachable(AzureSettings.FairFXUrl))
			{
				Debug.WriteLine("skipped cards update");
				return new List<FFXCard>();
			}
			string[] creds = DecodeCredentials(ffx);
			if (creds == null) return new List<FFXCard>();

			Debug.WriteLineIf(debugging, "Live 2 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

			using (HttpClient client = new HttpClient() { BaseAddress = new Uri(AzureSettings.FairFXUrl) })
			{
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				FormUrlEncodedContent loginContent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
					new KeyValuePair<string, string>("username", creds[0]),
					new KeyValuePair<string, string>("password", creds[1]),
					new KeyValuePair<string, string>("domain", "corporate")
				});

				Debug.WriteLineIf(debugging, "Live 3 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage loginResp = await client.PostAsync("/rest/auth", loginContent);
				if (!loginResp.IsSuccessStatusCode) return new List<FFXCard>();

				Debug.WriteLineIf(debugging, "Live 4 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				FFXSession session = JsonConvert.DeserializeObject<FFXSession>(await loginResp.Content.ReadAsStringAsync());

				Debug.WriteLineIf(debugging, "Live 5 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				HttpResponseMessage cardsResp = await client.GetAsync("/rest/card/list/-/" + session.SessionID);

				Debug.WriteLineIf(debugging, "Live 6 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

				List<FFXCard> ret = cardsResp.IsSuccessStatusCode
					? JsonConvert.DeserializeObject<List<FFXCard>>(await cardsResp.Content.ReadAsStringAsync())
					: new List<FFXCard>();

				Debug.WriteLineIf(debugging, "Live 7 " + DateTime.Now.Second + " " + DateTime.Now.Millisecond);

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
