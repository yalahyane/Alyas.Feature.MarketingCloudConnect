using System.Net;
using System.Net.Http;
using System.Web.Http;
using Alyas.Feature.MarketingCloudConnect.Models;
using Alyas.Feature.MarketingCloudConnect.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Alyas.Feature.MarketingCloudConnect.Controllers
{
    [RoutePrefix("api/tracking")]
    public class TrackingController : ApiController
    {
        private readonly ITrackingService _trackingService = ServiceLocator.ServiceProvider.GetService<ITrackingService>();
        [Route("TrackEvent")]
        [HttpPost]
        public HttpResponseMessage TrackEvent(TrackEventRequest request)
        {
            foreach (var eventItem in request.EventData)
            {
                _trackingService.TrackEvent(request.TrackingKey, eventItem);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}