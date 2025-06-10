using System.Collections.Generic;

namespace Alyas.Feature.MarketingCloudConnect.Models
{
    public class TrackEventRequest
    {
        public string TrackingKey { get; set; }
        public List<object> EventData { get; set; }
    }
}