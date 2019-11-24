using Logitude.IntegrationTest.Core.Login;
using Newtonsoft.Json;
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
        public static async Task<HttpResponseMessage> GetAsync(string uriParams)
        {
            using (var client = new HttpClient())
            {
                string getDeclarationUrl = mainUrl + uriParams;
                client.DefaultRequestHeaders.Add("Token", IntegrationTestLoginParameters.Token);
                var result = await client.GetAsync(getDeclarationUrl);
                return result;
            }
        }

        public static async Task<HttpResponseMessage> PostAsync(object content,string uriControllerAndMethod)
        {
            using (var client = new HttpClient())
            {
                string AuthURI = mainUrl + uriControllerAndMethod;
                var serializedObject = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(AuthURI, stringContent);
                return result;
            }
        }

        public static async Task<HttpResponseMessage> PutAsync(object content, string uriControllerAndMethod)
        {
            using (var client = new HttpClient())
            {
                string AuthURI = mainUrl + uriControllerAndMethod;
                var serializedObject = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(serializedObject, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(AuthURI, stringContent);
                return result;
            }
        }
    }
}
