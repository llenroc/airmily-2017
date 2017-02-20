using airmily.Services.Interfaces;

namespace airmily.Services.Models
{
    public class EntityDataOfflineSync : IEntityDataOfflineSync
    {
        public string ID { get; set; }

        public string Version { get; set; }
    }
}