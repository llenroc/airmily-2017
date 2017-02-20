using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace airmily.Services.ModelsAppService
{
    public class EntityDataOS
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [Version]
        public string Version { get; set; }
    }
}