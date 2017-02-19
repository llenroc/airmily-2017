using System;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;

namespace airmily.Services.ModelsAppService
{
    public class EntityDataOs
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [CreatedAt]
        public DateTimeOffset CreatedAt { get; set; }

        [UpdatedAt]
        public DateTimeOffset UpdatedAt { get; set; }

        [Version]
        public string Version { get; set; }
    }
}