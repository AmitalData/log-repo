using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SpecFlow.Models
{
    public class HttpRequest
    {
        protected string ApiUrl = ConfigurationManager.AppSettings["Url"];
        protected string RequestUrl;
        protected string RequestType;
        protected string Token;
        protected StringContent RequestStringContent;

        public HttpRequest(string url, HttpRequestType.NoBodyRequestType requestType, string token)
        {
            RequestUrl = GetRequestUrl(url);
            RequestType = requestType == HttpRequestType.NoBodyRequestType.Get ? "Get" : "Delete";
            Token = token;
        }

        public HttpRequest(string url, HttpRequestType.BodyRequestType requestType, string token, object requestContent)
        {
            RequestUrl = GetRequestUrl(url);
            RequestType = requestType == HttpRequestType.BodyRequestType.Post ? "Post" : "Put";
            Token = token;
            RequestStringContent = new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, "application/json");
        }

        public T GetResponse<T>()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                if (!String.IsNullOrEmpty(Token))
                {
                    httpClient.DefaultRequestHeaders.Add("Token", Token);
                }
                Task<HttpResponseMessage> httpResponse = GetHttpResponse(httpClient);
                httpResponse.Wait();
                if (httpResponse.Result.StatusCode == HttpStatusCode.OK)
                {
                    string httpResponseString = httpResponse.Result.Content.ReadAsStringAsync().Result;
                    T response = JsonConvert.DeserializeObject<T>(httpResponseString);
                    return response;
                }
                else
                {
                    throw new Exception(RequestType + " Request To " + RequestUrl + " Faild With Status Code " + httpResponse.Result.StatusCode.ToString());
                }
            }
        }

        protected string GetRequestUrl(string url)
        {
            if (String.IsNullOrEmpty(url))
            {
                return null;
            }
            return (ApiUrl.EndsWith("/") ? ApiUrl.TrimEnd('/') : ApiUrl) + (!url.StartsWith("/") ? ("/" + url) : url);
        }

        protected Task<HttpResponseMessage> GetHttpResponse(HttpClient httpClient)
        {
            Task<HttpResponseMessage> httpResponse;

            if (RequestType == "Get")
            {
                httpResponse = httpClient.GetAsync(RequestUrl);
            }
            else if (RequestType == "Post")
            {
                httpResponse = httpClient.PostAsync(RequestUrl, RequestStringContent);
            }
            else if (RequestType == "Put")
            {
                httpResponse = httpClient.PutAsync(RequestUrl, RequestStringContent);
            }
            else
            {
                httpResponse = httpClient.DeleteAsync(RequestUrl);
            }

            return httpResponse;
        }
    }
}