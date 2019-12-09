using Logitude.IntegrationTest.Core.Login;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core
{
    public class RestClientService
    {
        private static string mainUrl = IntegrationTestLoginParameters.ServerURL + "api/";
        public static async Task<HttpResponseMessage> GetAsync(string urlControllerAndMethod)
        {
            using (var client = new HttpClient())
            {
                string getDeclarationUrl = GetUrl(urlControllerAndMethod);
                client.DefaultRequestHeaders.Add("Token", IntegrationTestLoginParameters.Token);
                var result = await client.GetAsync(getDeclarationUrl);
                return result;
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(object content,string urlControllerAndMethod)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", IntegrationTestLoginParameters.Token);
                string postURI = GetUrl(urlControllerAndMethod);
                var stringContent = PrepareStringContent(content);
                var result = await client.PostAsync(postURI, stringContent);
                return result;
            }
        }

        public static async Task<HttpResponseMessage> PutAsync(object content, string urlControllerAndMethod)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Token", IntegrationTestLoginParameters.Token);
                string putURI = GetUrl(urlControllerAndMethod);
                var stringContent = PrepareStringContent(content);
                var result = await client.PutAsync(putURI, stringContent);
                return result;
            }
        }

        public static string ParseResponseAndReturnSingleResult(HttpResponseMessage httpResponseMessage)
        {
            var stringResult = httpResponseMessage.Content.ReadAsStringAsync().Result;
            JObject jObject = JObject.Parse(stringResult);
            string result = jObject.SelectToken("Result").Count() > 0 ? (string)jObject.SelectToken("Result")[0].ToString():null;
            return result;
        }
        private static StringContent PrepareStringContent(object content)
        {
            var serializedObject = JsonConvert.SerializeObject(content);
            var stringContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");
            return stringContent;
        }

        private static string GetUrl(string urlControllerAndMethod)
        {
            string url = mainUrl + urlControllerAndMethod;
            return url;
        }
    }
}
