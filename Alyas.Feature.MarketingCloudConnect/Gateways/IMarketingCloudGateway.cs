using System.Collections.Specialized;
using Alyas.Feature.MarketingCloudConnect.Models;

namespace Alyas.Feature.MarketingCloudConnect.Gateways
{
    public interface IMarketingCloudGateway
    {
        void UpsertDataExtension(string trackingKey, object eventData);
        void UpdateDataExtension(string trackingKey, object eventData);
        GetDataExtensionsResponse GetDataExtensions(string trackingKey, NameValueCollection filters);
    }
}
