using Newtonsoft.Json;

namespace Alyas.Feature.MarketingCloudConnect.Models
{
    public class DataExtensionItem
    {
        [JsonProperty("keys")]
        public DataExtensionKeys Keys { get; set; }
        [JsonProperty("values")]
        public object Values { get; set; }
    }
}