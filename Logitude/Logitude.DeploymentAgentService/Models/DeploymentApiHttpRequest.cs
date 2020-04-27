using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.DeploymentAgentService.Models
{
    public class DeploymentApiHttpRequest
    {
        protected string DeploymentWebApiUrl = ConfigurationManager.AppSettings["DeploymentWebApiUrl"];
        protected string RequestType;
        protected string RequestUrl;
        protected StringContent RequestStringContent;
        
        public DeploymentApiHttpRequest(string url, HttpRequestType.NoBodyRequestType requestUrlType)
        {
            RequestUrl = GetRequestUrl(url);
            RequestType = requestUrlType == HttpRequestType.NoBodyRequestType.Get ? "Get" : "Delete";
        }

        public DeploymentApiHttpRequest(string url, object requestContent, HttpRequestType.BodyRequestType requestContentType)
        {
            RequestUrl = GetRequestUrl(url);
            string serializedObject = JsonConvert.SerializeObject(requestContent);
            RequestStringContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            RequestType = requestContentType == HttpRequestType.BodyRequestType.Post ? "Post" : "Put";
        }

        public void GetResponse()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                Task<HttpResponseMessage> httpResponse = GetHttpResponse(httpClient);
                httpResponse.Wait();
                if(httpResponse.Result.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception(RequestType + " Request To " + RequestUrl + " Faild With Status Code " + httpResponse.Result.StatusCode.ToString());
                }
            }
        }
        
        public T GetResponse<T>()
        {
            using (HttpClient httpClient = new HttpClient())
            {
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
            return (DeploymentWebApiUrl.EndsWith("/") ? DeploymentWebApiUrl.TrimEnd('/') : DeploymentWebApiUrl) + (!url.StartsWith("/") ? ("/" + url) : url);
        }

        protected Task<HttpResponseMessage> GetHttpResponse(HttpClient httpClient)
        {
            Task<HttpResponseMessage> httpResponse;

            if(RequestType == "Get")
            {
                httpResponse = httpClient.GetAsync(RequestUrl);
            }
            else if(RequestType == "Post")
            {
                httpResponse = httpClient.PostAsync(RequestUrl, RequestStringContent);
            }
            else if(RequestType == "Put")
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