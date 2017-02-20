using Newtonsoft.Json;
using Microsoft.WindowsAzure.MobileServices;

namespace airmily.Services.Interfaces
{
    public interface IEntityDataOfflineSync
    {
        [JsonProperty(PropertyName = "id")]
        string ID { get; set; }

        [Version]
        string Version { get; set; }
    }
}