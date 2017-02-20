using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using airmily.Services.ModelsAppService;

namespace airmily.Services.AppService
{
    public interface IAppService
    {
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();

        Task SaveTaskAsync(TodoItem item);

        Task SyncAsync();
    }
}
