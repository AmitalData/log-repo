using System;
using System.Net;
using System.Collections.Generic;
using RestSharp;
using Newtonsoft.Json;
using MicrosoftGraphClient.Models.GraphAPI;

namespace MicrosoftGraphClient.GraphAPI
{
    public static class GraphAPICaller
    {
        public static T Call<T>(GraphAPICallerParameters graphAPICallerParameters)
        {
            RestRequest restRequest = GetRestRequest(graphAPICallerParameters);
            RestResponse<T> restResponse = new RestClient().ExecuteAsync<T>(restRequest).Result;
            if (IsSuccessResponse(restResponse))
            {
                return restResponse.Data;
            }
            throw new Exception(GetResponseErrorMessage(restResponse));
        }

        public static string Call(GraphAPICallerParameters graphAPICallerParameters)
        {
            RestRequest restRequest = GetRestRequest(graphAPICallerParameters);
            RestResponse restResponse = new RestClient().ExecuteAsync(restRequest).Result;
            if (IsSuccessResponse(restResponse))
            {
                return restResponse.Content;
            }
            throw new Exception(GetResponseErrorMessage(restResponse));
        }

        private static RestRequest GetRestRequest(GraphAPICallerParameters graphAPICallerParameters)
        {
            if(graphAPICallerParameters == null)
            {
                throw new Exception("Empty graph API caller parameters");
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            RestRequest restRequest = new RestRequest(graphAPICallerParameters.Url, graphAPICallerParameters.Method) { RequestFormat = DataFormat.Json };
            restRequest = AddAuthorizationHeader(restRequest, graphAPICallerParameters.AccessToken);
            restRequest = AddHeaders(restRequest, graphAPICallerParameters.RequestHeaders);
            restRequest = AddParameters(restRequest, graphAPICallerParameters.RequestParameters);
            restRequest = AddJsonBody(restRequest, graphAPICallerParameters.RequestBody);
            return restRequest;
        }

        private static RestRequest AddAuthorizationHeader(RestRequest restRequest, string accessToken)
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                restRequest.AddHeader("Authorization", "Bearer " + accessToken);
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
                string requestBodyJson = JsonConvert.SerializeObject(requestBody);
                restRequest.AddParameter("application/json", requestBodyJson, ParameterType.RequestBody);
            }
            return restRequest;
        }

        private static bool IsSuccessResponse(RestResponse restResponse)
        {
            return restResponse != null &&
                (restResponse.StatusCode == HttpStatusCode.OK ||
                restResponse.StatusCode == HttpStatusCode.Accepted ||
                restResponse.StatusCode == HttpStatusCode.Created);
        }

        private static string GetResponseErrorMessage(RestResponse restResponse)
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