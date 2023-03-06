using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;

namespace Logitude.Workflow.BL.ExternalAPI
{
    public static class APICaller
    {
        public static T CallApi<T>(string url, object o, Method m, List<KeyValuePair<string, string>> headers = null)
        {
            RestClient restClient = new RestClient();
            RestRequest restRequest = new RestRequest(url, m) { RequestFormat = DataFormat.Json };
            if (headers != null)
            {
                foreach (var item in headers)
                {
                    restRequest.AddHeader(item.Key, item.Value);
                }
            }

            if (o != null)
            {
                restRequest.AddJsonBody(o);
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var restResponse = restClient.ExecuteAsync<T>(restRequest).Result;
            if (restResponse.StatusCode == HttpStatusCode.OK || restResponse.StatusCode == HttpStatusCode.Accepted)
            {
                return restResponse.Data;
            }
            else if (restResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new Exception("URL NotFound");
            }
            else
            {
                JObject jObject = JObject.Parse(restResponse.Content);
                if (jObject["ErrorMessage"] != null)
                {
                    var ErrorMessage = jObject["ErrorMessage"].ToString().Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
                    throw new Exception(ErrorMessage);
                }
                else
                {
                    var ErrorMessage = "Response Status Code: " + restResponse.StatusCode.ToString() + "\n" + jObject.ToString();
                    throw new Exception(ErrorMessage);
                }

            }
        }
    }
}