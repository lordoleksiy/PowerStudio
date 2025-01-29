using Newtonsoft.Json;

namespace PowerStudio.Models.Azure
{
    public class Subscription
    {
        public string SubscriptionId { get; set; }
       [JsonProperty("displayName")] public string Name { get; set; }
    }

}
