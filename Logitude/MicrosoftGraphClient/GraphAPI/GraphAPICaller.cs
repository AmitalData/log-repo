using System;
using System.Net;
using RestSharp;
using System.Collections.Generic;
using MicrosoftGraphClient.Models.GraphAPI;

namespace MicrosoftGraphClient.GraphAPI
{
    public static class GraphAPICaller
    {
        public static T Call<T>(GraphAPICallerParams graphAPICallerParams)
        {
            RestRequest restRequest = GetRestRequest(graphAPICallerParams);
            IRestResponse<T> restResponse = new RestClient().ExecuteAsync<T>(restRequest).Result;
            if (IsSuccessResponse(restResponse))
            {
                return restResponse.Data;
            }
            throw new Exception(GetResponseErrorMessage(restResponse));
        }

        public static string Call(GraphAPICallerParams graphAPICallerParams)
        {
            RestRequest restRequest = GetRestRequest(graphAPICallerParams);
            IRestResponse restResponse = new RestClient().ExecuteAsync(restRequest).Result;
            if (IsSuccessResponse(restResponse))
            {
                return restResponse.Content;
            }
            throw new Exception(GetResponseErrorMessage(restResponse));
        }

        private static RestRequest GetRestRequest(GraphAPICallerParams graphAPICallerParams)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            RestRequest restRequest = new RestRequest(graphAPICallerParams?.Url, graphAPICallerParams?.Method ?? Method.GET) { RequestFormat = DataFormat.Json };
            restRequest = AddAuthorizationHeader(restRequest, graphAPICallerParams?.Token);
            restRequest = AddHeaders(restRequest, graphAPICallerParams?.RequestHeaders);
            restRequest = AddParameters(restRequest, graphAPICallerParams?.RequestParameters);
            restRequest = AddJsonBody(restRequest, graphAPICallerParams?.RequestBody);
            return restRequest;
        }

        private static RestRequest AddAuthorizationHeader(RestRequest restRequest, string token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader("Authorization", "Bearer " + token);
            }
            return restRequest;
        }

        private static RestRequest AddHeaders(RestRequest restRequest, Dictionary<string, string> requestHeaders)
        {
            if (requestHeaders != null)
            {
                foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
                {
                    restRequest.AddHeader(requestHeader.Key, requestHeader.Value);
                }
            }
            return restRequest;
        }

        private static RestRequest AddParameters(RestRequest restRequest, Dictionary<string, string> requestParameters)
        {
            if (requestParameters != null)
            {
                foreach (KeyValuePair<string, string> requestParameter in requestParameters)
                {
                    restRequest.AddParameter(requestParameter.Key, requestParameter.Value);
                }
            }
            return restRequest;
        }

        private static RestRequest AddJsonBody(RestRequest restRequest, object requestBody)
        {
            if (requestBody != null)
            {
                restRequest.AddJsonBody(requestBody);
            }
            return restRequest;
        }

        private static bool IsSuccessResponse(IRestResponse restResponse)
        {
            return restResponse != null && (restResponse.StatusCode == HttpStatusCode.OK || restResponse.StatusCode == HttpStatusCode.Accepted);
        }

        private static string GetResponseErrorMessage(IRestResponse restResponse)
        {
            string defaultErrorMessage = "Error";
            if (restResponse != null)
            {
                if (restResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    return "Not found";
                }
                return restResponse.Content ?? defaultErrorMessage;
            }
            return defaultErrorMessage;
        }
    }
}