using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Customs.Data.EntityPOCOs;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    internal static class AmitalAPIHelper
    {
        static readonly string baseUrl = "";// new SettingQuery().GetSinglePMFromCahche().AmitalApiAddress;
        static readonly string xFunctionsKey = "";//new SettingQuery().GetSinglePMFromCahche().AmitalApiXFunctionsKey;

        public static HttpClienResponse SendRequest(string token, string url, HttpMethod httpMethod, object body = null, string user = "")
        {
            HttpResponseMessage res;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("token", token);
            client.DefaultRequestHeaders.Add("x-functions-key", xFunctionsKey);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            url = baseUrl + url;
            StringContent stringContent = body != null ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json") : null;


            switch (httpMethod)
            {
                case HttpMethod m when m == HttpMethod.Post:
                    client.DefaultRequestHeaders.Add("user_id", user);
                    res = client.PostAsync(url, stringContent).Result;
                    break;

                case HttpMethod m when m == HttpMethod.Put:
                        client.DefaultRequestHeaders.Add("user_id", user);
                        res = client.PutAsync(url, stringContent).Result;
                    break;

                case HttpMethod m when m == HttpMethod.Delete:
                        client.DefaultRequestHeaders.Add("user_id", user);
                        res = client.DeleteAsync(url).Result;
                    break;

                case HttpMethod m when m == HttpMethod.Get:
                        res = client.GetAsync(url).Result;
                    break;

                default:
                    throw new Exception($"method {httpMethod.Method} not config");
            }

            string content = res.Content.ReadAsStringAsync().Result;
            client.Dispose();

            return new HttpClienResponse { Content = content, Res = res };
        }

        private static void AddHeadersUser(HttpClient httpClient, string user)
        {
            httpClient.DefaultRequestHeaders.Add("user_id", user);
            //var user = HeaderHelper.GetUserFromToken();
            //if (user != null)
            //{
            //    client.DefaultRequestHeaders.Add("user", user.Id.ToString());
            //    client.DefaultRequestHeaders.Add("tenant", user.TenantId.ToString());
            //}
        }   
    }

    public class HttpClienResponse
    {
        public HttpResponseMessage Res { get; set; }
        public string Content { get; set; }
    }
}
