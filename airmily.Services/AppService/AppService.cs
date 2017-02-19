using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync()
        {
            await this.SyncAsync();

            IEnumerable<TodoItem> items = await this._todoTable
                .Where(todoItem => !todoItem.Complete)
                .ToEnumerableAsync();

            return new ObservableCollection<TodoItem>(items);
        }

        public async Task SaveTaskAsync(TodoItem item)
        {
            if (item.Id == null)
            {
                await this._todoTable.InsertAsync(item);
            }
            else
            {
                await this._todoTable.UpdateAsync(item);
            }
        }

        public async Task SyncAsync()
        {
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
    }
}