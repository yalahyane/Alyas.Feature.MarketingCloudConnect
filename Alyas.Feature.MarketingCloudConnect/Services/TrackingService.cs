using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Alyas.Feature.MarketingCloudConnect.Gateways;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Alyas.Feature.MarketingCloudConnect.Services
{
    public class TrackingService : ITrackingService
    {
        private readonly IMarketingCloudGateway _marketingCloudGateway = ServiceLocator.ServiceProvider.GetService<IMarketingCloudGateway>();
        public void TrackEvent(string trackingKey, object eventData)
        {
            _marketingCloudGateway.UpsertDataExtension(trackingKey, eventData);
        }

        public List<KeyValuePair<string, object>> GetTrackingEventData(string trackingKey, NameValueCollection filters)
        {
            var response = this._marketingCloudGateway.GetDataExtensions(trackingKey, filters);
            return response.Items.Select(i => new KeyValuePair<string, object>(i.Keys.Id, i.Values)).ToList();
        }

        public void UpdateEvent(string trackingKey, object eventData)
        {
            _marketingCloudGateway.UpdateDataExtension(trackingKey, eventData);
        }
    }
}