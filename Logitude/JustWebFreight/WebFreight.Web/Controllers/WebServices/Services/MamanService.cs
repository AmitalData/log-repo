using Logitude.BL.GlobalModel.EntityQueries;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebFreight.Web.Controllers.WebServices.Services
{
    public class MamanService
    {
        static string MamanServiceUrl = new SettingQuery().GetSinglePMFromCahche().MamanServiceUrl;

        public static HttpResponseMessage Send(string path, string body, int tenant, HttpMethod method)
        {
            string url = MamanServiceUrl + path;

            string jwt = new SettingQuery().GetJwtToken(tenant);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            StringContent stringContent = body != null ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json") : null;

            Task<HttpResponseMessage> resTask;

            switch (method.Method.ToUpper())
            {
                case "GET":
                    resTask = client.GetAsync(url);
                    break;
                case "POST":
                    resTask = client.PostAsync(url, stringContent);
                    break;
                case "PUT":
                    resTask = client.PutAsync(url, stringContent);
                    break;
                case "DELETE":
                    resTask = client.DeleteAsync(url);
                    break;
                default:
                    throw new ArgumentException("Invalid HTTP method specified.");
            }

            HttpResponseMessage res = resTask.Result;
            client.Dispose();

            return res;
        }
    }
}