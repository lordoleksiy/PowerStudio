using Newtonsoft.Json;

namespace PowerStudio.Models.Azure
{
    public class AppService
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        [JsonProperty("appServiceName")] public string Name { get; set; }
    }
}
