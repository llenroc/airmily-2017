using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using airmily.Services.ModelsAppService;

namespace airmily.Services.AppService
{
    public interface IAppService
    {
        Task<ObservableCollection<TodoItem>> GetTodoItemsAsync();

        Task SaveTaskAsync(TodoItem item);

        Task SyncAsync();
    }
}
