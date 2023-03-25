using System;
using System.Net;
using RestSharp;
using Newtonsoft.Json.Linq;

namespace MicrosoftGraphClient.GraphAPI
{
    public static class GraphAPICaller
    {
        public static T Call<T>(string token, string apiUrl, Method method, object requestBody = null)
        {
            try
            {
                if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(apiUrl))
                {
                    throw new Exception("Invalid token or url");
                }

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                RestRequest restRequest = GetRestRequest(token, apiUrl, method, requestBody);
                RestClient restClient = new RestClient();
                IRestResponse<T> restResponse = restClient.ExecuteAsync<T>(restRequest).Result;
                if (restResponse.StatusCode == HttpStatusCode.OK || restResponse.StatusCode == HttpStatusCode.Accepted)
                {
                    return restResponse.Data;
                }
                else if (restResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    throw new Exception(apiUrl + " not found");
                }
                else
                {
                    throw new Exception(GetResponseErrorMessage(restResponse));
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        private static RestRequest GetRestRequest(string token, string apiUrl, Method method, object requestBody)
        {
            RestRequest restRequest = new RestRequest(apiUrl, method) { RequestFormat = DataFormat.Json };
            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader("Authorization", "Bearer " + token);
            }
            if (requestBody != null)
            {
                restRequest.AddJsonBody(requestBody);
            }
            return restRequest;
        }

        private static string GetResponseErrorMessage(IRestResponse restResponse)
        {
            if(restResponse != null)
            {
                JObject responseContentObject = JObject.Parse(restResponse.Content);
                string errorMessage;
                if (responseContentObject["error"] != null && responseContentObject["error"]["message"] != null)
                {
                    errorMessage = responseContentObject["error"]["message"].ToString();
                }
                else
                {
                    errorMessage = "Error with status code " + restResponse.StatusCode.ToString() + "\n" + responseContentObject.ToString();
                }
                return errorMessage;
            }
            return null;
        }
    }
}