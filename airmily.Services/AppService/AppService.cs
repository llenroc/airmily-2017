using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using airmily.Services.ModelsAppService;
using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace airmily.Services.AppService
{
    public class AppService : IAppService
    {
        private const string OfflineDbPath = @"airmilyAppService.db";

        private readonly MobileServiceClient _mobileServiceClient;

        private readonly IMobileServiceSyncTable<TodoItem> _todoTable;

        public AppService()
        {
            this._mobileServiceClient = new MobileServiceClient(AppServiceSettings.ApplicationUrl);

            var store = new MobileServiceSQLiteStore(OfflineDbPath);
            store.DefineTable<TodoItem>();
            this._mobileServiceClient.SyncContext.InitializeAsync(store);

            this._todoTable = this._mobileServiceClient.GetSyncTable<TodoItem>();
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            await this.SyncAsync();

            IEnumerable<TodoItem> items = await this._todoTable
                .Where(todoItem => !todoItem.Complete)
                .ToEnumerableAsync();

            return new List<TodoItem>(items);
        }

        public async Task SaveTaskAsync(TodoItem item)
        {
            if (item.Id == null)
                await this._todoTable.InsertAsync(item);
            else
                await this._todoTable.UpdateAsync(item);

            await this.SyncAsync();
        }

        public async Task SyncAsync()
        {
            var connected = await Plugin.Connectivity.CrossConnectivity.Current.IsReachable(airmily.Services.AppService.AppServiceSettings.ApplicationUrl);
            if (connected == false)
                return;

            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await this._mobileServiceClient.SyncContext.PushAsync();
                await this._todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this._todoTable.CreateQuery());
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
            //if (syncErrors != null)
            //{
            //    foreach (var error in syncErrors)
            //    {
            //        if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
            //        {
            //            //Update failed, reverting to server's copy.
            //            await error.CancelAndUpdateItemAsync(error.Result);
            //        }
            //        else
            //        {
            //            // Discard local change.
            //            await error.CancelAndDiscardItemAsync();
            //        }

            //        Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
            //    }
            //}
        }
    }

    public class MockService : IAppService
    {
        private const string OfflineDbPath = @"airmilyAppService.db";

        private readonly MobileServiceClient _mobileServiceClient;

        private readonly IMobileServiceSyncTable<TodoItem> _todoTable;

        List<TodoItem> Items { get; set; } = new List<TodoItem>();

        public MockService()
        {
            this._mobileServiceClient = new MobileServiceClient(AppServiceSettings.ApplicationUrl);

            var store = new MobileServiceSQLiteStore(OfflineDbPath);
            store.DefineTable<TodoItem>();
            this._mobileServiceClient.SyncContext.InitializeAsync(store);

            this._todoTable = this._mobileServiceClient.GetSyncTable<TodoItem>();

            //if (Items.Count == 0)
            //    Items = ToDos();
        }
        
        List<TodoItem> ToDos()
        {
            var itms = new List<TodoItem>();

            var todo1 = new TodoItem
            {
                Text = "Get my hair cut",
                Complete = false
            };
            itms.Add(todo1);

            var todo2 = new TodoItem
            {
                Text = "Finish up presentation",
                Complete = true
            };
            itms.Add(todo2);

            var todo3 = new TodoItem
            {
                Text = "Build Azure Function",
                Complete = false
            };
            itms.Add(todo3);


            var todo4 = new TodoItem
            {
                Text = "Buy Hololens",
                Complete = false
            };
            itms.Add(todo4);

            return itms;
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            //IEnumerable<TodoItem> todos = Items.AsEnumerable();
            //return Task.FromResult(todos);

            await this.SyncAsync();

            IEnumerable<TodoItem> items = await this._todoTable
                //.Where(todoItem => !todoItem.Complete)
                .ToEnumerableAsync();

            return items;
        }

        public Task SaveTaskAsync(TodoItem item)
        {
            var todo = Items.FirstOrDefault(x => x.Id == item.Id);
            Items.Remove(todo);
            Items.Add(item);
            return Task.FromResult(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task SyncAsync()
        {
            //var connected = await Plugin.Connectivity.CrossConnectivity.Current.IsReachable(airmily.Services.AppService.AppServiceSettings.ApplicationUrl);
            //if (connected == false)
            //    return;

            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;
            try
            {
                await this._mobileServiceClient.SyncContext.PushAsync();
                await this._todoTable.PullAsync(
                    //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                    //Use a different query name for each unique query in your program
                    "allTodoItems",
                    this._todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }
        }
    }
}