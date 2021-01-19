using Logitude.Test.Base.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace Logitude.Test.Base.Services
{
    public class APICaller
    {
        protected static readonly string ApiUrl = ConfigurationManager.AppSettings["Url"];

        public static APIResponse<T> CallPost<T>(object requestBody, string url, string token)
        {
            APIRequestParameters request = new APIRequestParameters() {
                method = Method.POST, 
                requestBody = requestBody, 
                token = token, 
                URL = url };
            return CallAPIProcess<T>(request);
        }

        public static APIResponse<T> CallPut<T>(object requestBody, string url, string token)
        {
            APIRequestParameters request = new APIRequestParameters() { 
                method = Method.PUT, 
                requestBody = requestBody, 
                token = token, 
                URL = url };
            return CallAPIProcess<T>(request);
        }

        public static APIResponse<T> CallGet<T>(string url, string token)
        {
            APIRequestParameters request = new APIRequestParameters() { 
                method = Method.GET, 
                token = token, 
                URL = url };
            return CallAPIProcess<T>(request);
        }

        private static APIResponse<T> CallAPIProcess<T>(APIRequestParameters requestParameters)
        {
            string restClientUrl = GetRequestUrl(requestParameters.URL);
            RestClient restClient = new RestClient(restClientUrl);
            RestRequest restRequest = new RestRequest(requestParameters.method) { RequestFormat = DataFormat.Json };
            var response = new APIResponse<T>();

            if (!string.IsNullOrEmpty(requestParameters.token))
            {
                restRequest.AddHeader("Token", requestParameters.token);
            }
            if (requestParameters.requestBody != null)
            {
                restRequest.AddJsonBody(JsonConvert.SerializeObject(requestParameters.requestBody));
            }

            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);
            response.StatusCode = restResponse.StatusCode;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                response.Data = restResponse.Data;
            }
            else
            {
                dynamic responseException = restResponse.Content;
                response.ErrorMessage = (responseException["ErrorMessage"] as string).Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
            }

            return response;
        }

        public static APIResponse<T> CallGetByFilter<T>(APIRequestParameters requestParameters)
        {
            return new APIResponse<T>();
        }

        protected static string GetRequestUrl(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return null;
            }
            return (ApiUrl.EndsWith("/") ? ApiUrl.TrimEnd('/') : ApiUrl) + (!url.StartsWith("/") ? ("/" + url) : url);
        }
    }
}