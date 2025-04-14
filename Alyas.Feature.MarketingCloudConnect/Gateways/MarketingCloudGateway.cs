using System.Collections.Specialized;
using System.Linq;
using Alyas.Feature.MarketingCloudConnect.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers.NewtonsoftJson;
using Sitecore.Configuration;
using Sitecore.Diagnostics;

namespace Alyas.Feature.MarketingCloudConnect.Gateways
{
    public class MarketingCloudGateway : IMarketingCloudGateway
    {
        private static readonly RestClient _authRestClient = new RestClient(Settings.GetSetting("MarketingCloudAuthBaseUrl"));
        private static readonly RestClient _restClient = new RestClient(Settings.GetSetting("MarketingCloudBaseUrl"));

        
        public void UpsertDataExtension(string trackingKey, object eventData)
        {
            var accessToken = Authenticate();
            if (!string.IsNullOrEmpty(accessToken))
            {
                _restClient.UseNewtonsoftJson();
                var request = new RestRequest($"data/v1/async/dataextensions/key:{trackingKey}/rows", Method.Post);
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AddHeader("Content-Type", "application/json");
                var requestData = new
                {
                    items = new[]
                    {
                        eventData
                    }
                };
                var jsonRequest = JsonConvert.SerializeObject(requestData);
                request.AddStringBody(jsonRequest, DataFormat.Json);

                var response = _restClient.Execute(request);
                if (!response.IsSuccessful || response.Content == null)
                {
                    Log.Error($"Error inserting data extension : {response.Content}. Tracking Key: {trackingKey}. Event Data: {JsonConvert.SerializeObject(eventData)}", this);
                }
            }
        }

        public void UpdateDataExtension(string trackingKey, object eventData)
        {
            var accessToken = Authenticate();
            if (!string.IsNullOrEmpty(accessToken))
            {
                _restClient.UseNewtonsoftJson();
                var request = new RestRequest($"data/v1/async/dataextensions/key:{trackingKey}/rows", Method.Put);
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                request.AddHeader("Content-Type", "application/json");
                var requestData = new
                {
                    items = new[]
                    {
                        eventData
                    }
                };
                var jsonRequest = JsonConvert.SerializeObject(requestData);
                request.AddStringBody(jsonRequest, DataFormat.Json);

                var response = _restClient.Execute(request);
                if (!response.IsSuccessful || response.Content == null)
                {
                    Log.Error($"Error updating data extension : {response.Content}. Tracking Key: {trackingKey}. Event Data: {JsonConvert.SerializeObject(eventData)}", this);
                }
            }
        }

        public GetDataExtensionsResponse GetDataExtensions(string trackingKey, NameValueCollection filters)
        {
            var filterExpression = string.Empty;
            var firstFilter = true;
            foreach (var filter in filters.AllKeys)
            {
                if (!firstFilter)
                {
                    filterExpression += " and ";
                }

                firstFilter = false;
                filterExpression += $"{filter} eq '{filters[filter]}'";
            }
            var accessToken = Authenticate();
            if (!string.IsNullOrEmpty(accessToken))
            {
                var url = $"data/v1/customobjectdata/key/{trackingKey}/rowset";
                if (filters.AllKeys.Any())
                {
                    url += $"?$filter={filterExpression}";
                }
                var request = new RestRequest(url);
                request.AddHeader("Authorization", $"Bearer {accessToken}");
                var result = _restClient.Execute(request);
                if (!result.IsSuccessful || result.Content == null)
                {
                    Log.SingleError($"MC Data Extensions Api Call failed for Endpoint {url}: {result.ErrorMessage}", this);
                    return new GetDataExtensionsResponse();
                }
                return JsonConvert.DeserializeObject<GetDataExtensionsResponse>(result.Content);
            }

            return new GetDataExtensionsResponse();
        }

        private string Authenticate()
        {
            var request = new RestRequest("v2/token", Method.Post);
            request.AddJsonBody(new
            {
                grant_type = "client_credentials",
                client_id = Settings.GetSetting("MarketingCloudClientId"),
                client_secret = Settings.GetSetting("MarketingCloudClientSecret")
            });

            var response = _authRestClient.Execute(request);
            if (!response.IsSuccessful || response.Content == null)
            {
                Log.Error($"Error obtaining access token: {response.Content}", this);
                return string.Empty;
            }

            var tokenResponse = JsonConvert.DeserializeObject<dynamic>(response.Content);
            return tokenResponse.access_token;
        }
    }
}