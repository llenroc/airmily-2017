using airmily.Services.Models;
using Newtonsoft.Json;

namespace airmily.Services.ModelsAppService
{
    public class TodoItem : EntityDataOfflineSync, ITodoItem
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }
    }
}