using System.Collections.Generic;
using Newtonsoft.Json;

namespace Alyas.Feature.MarketingCloudConnect.Models
{
    public class GetDataExtensionsResponse
    {
        [JsonProperty("items")]
        public List<DataExtensionItem> Items = new List<DataExtensionItem>();
    }
}