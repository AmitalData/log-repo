using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Configuration;
using System.Net;

namespace Logitude.Test.Services
{
    public class APICaller
    {
        protected static readonly string ApiUrl = ConfigurationManager.AppSettings["Url"];

        public static T CallPost<T>(object requestBody, string url, string token)
        {
            return CallAPIProcess<T>(Method.POST, requestBody, url, token, null);
        }

        public static T CallPut<T>(object requestBody, string url, string token)
        { 
            return CallAPIProcess<T>(Method.PUT, requestBody, url, token, null);  
        }

        public static T CallGet<T>(string url, string token, string jsonElement)
        {
            return CallAPIProcess<T>(Method.GET, null, url, token, jsonElement);
        }

        protected static T CallAPIProcess<T>(Method method, object requestBody, string url, string token, string jsonElement)
        {
            string restClientUrl = GetRequestUrl(url);
            RestClient restClient = new RestClient(restClientUrl);
            RestRequest restRequest = new RestRequest(method) { RequestFormat = DataFormat.Json };

            if (!string.IsNullOrEmpty(token))
            {
                restRequest.AddHeader("Token", token);
            }

            if (requestBody != null)
            {
                restRequest.AddJsonBody(JsonConvert.SerializeObject(requestBody));
            }

            IRestResponse<T> restResponse = restClient.Execute<T>(restRequest);

            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                if (jsonElement != null)
                {
                    JObject jObject = JObject.Parse(restResponse.Content);
                    string result = jObject[jsonElement].ToString();
                    return JsonConvert.DeserializeObject<T>(result);
                }
                else
                {
                    return restResponse.Data;
                }
            }
            else
            {
                throw new Exception(method.ToString() + " Request To " + restClientUrl + " Faild With Message " + restResponse.Content);
            }
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