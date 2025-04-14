using System.Collections.Generic;
using System.Collections.Specialized;

namespace Alyas.Feature.MarketingCloudConnect.Services
{
    public interface ITrackingService
    {
        void TrackEvent(string trackingKey, object eventData);
        List<KeyValuePair<string, object>> GetTrackingEventData(string trackingKey,  NameValueCollection filters);
        void UpdateEvent(string trackingKey, object eventData);
    }
}
