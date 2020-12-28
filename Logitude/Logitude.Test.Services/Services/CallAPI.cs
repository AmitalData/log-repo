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
            return CallAPIProcess<T>(Method.POST, model, url, token);
        }

        public static T Put<T>(Object model, string url, string token)
        { 
            return CallAPIProcess<T>(Method.PUT, model, url, token);  
        }

       

        public static T Get<T>(string url, string token)
        {
            return CallAPIProcess<T>(Method.GET, null, url, token);
        }

        private static T CallAPIProcess<T>(Method method, Object model, string url, string token)
        {
            var restClient = new RestClient(mainURL + url);
            var request = new RestRequest(method);
            request.AddHeader("Token", token);
            if (method != Method.GET)
            {
                request.AddJsonBody(JsonConvert.SerializeObject(model));
            }
            
            var response = restClient.Execute<T>(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return response.Data;
            }
            else
            {
                throw new Exception(method.ToString() + " Request To " + url + " Faild With the message " + response.Content);
            }
        }

    }
}
