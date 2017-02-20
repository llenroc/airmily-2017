using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace airmily.Services.ModelsAppService
{
    public interface IEntityDataOS
    {
        [JsonProperty(PropertyName = "id")]
        string Id { get; set; }

        [Version]
        string Version { get; set; }
    }
}