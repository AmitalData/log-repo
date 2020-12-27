using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.SpecFlow.Services
{
    public class CallAPI
    {
        private static string mainURL = "http://test.logitudeworld.com/test/api/";
        public static T Post<T>(Object model, string url, string token)
        {
            var restCliesnt = new RestClient(mainURL + url);
            var request = new RestRequest(Method.POST);
            if (!string.IsNullOrEmpty(token))
            {
                request.AddHeader("Token", token);

            }
            request.AddJsonBody(JsonConvert.SerializeObject(model));

            var response = restCliesnt.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data;
            throw new Exception("post failed");
        }

        public static T Put<T>(Object model, string url, string token)
        {
            var restCliesnt = new RestClient(mainURL + url);
            var request = new RestRequest(Method.PUT);
            request.AddHeader("Token", token);
            request.AddJsonBody(JsonConvert.SerializeObject(model));

            var response = restCliesnt.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data;
            throw new Exception("Put failed");
        }

        public static T Get<T>(string url, string token)
        {
            var restCliesnt = new RestClient(mainURL + url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("Token", token); 

            var response = restCliesnt.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
                return response.Data;
            throw new Exception("Get failed");
        }
    }
}
