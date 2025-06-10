using Alyas.Feature.MarketingCloudConnect.Gateways;
using Alyas.Feature.MarketingCloudConnect.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Alyas.Feature.MarketingCloudConnect.ServiceConfigurators
{
    public class MarketingCloudConnectServiceConfigurator : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMarketingCloudGateway, MarketingCloudGateway>();
            serviceCollection.AddTransient<ITrackingService, TrackingService>();
        }
    }
}