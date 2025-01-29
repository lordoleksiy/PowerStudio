using Newtonsoft.Json;
using System.Collections.Generic;

namespace PowerStudio.Models.Azure
{
    public class AppServiceConfigResponse
    {
        [JsonProperty("properties")]
        public IDictionary<string, string> Properties { get; set; }
    }
}
