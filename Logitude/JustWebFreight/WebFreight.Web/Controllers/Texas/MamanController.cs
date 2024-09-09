using System;
using System.Net.Http;
using System.Net;
using System.Web.Http;
using WebFreight.Web.Helpers;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.GlobalModel.EntityQueries;
using Newtonsoft.Json;

namespace WebFreight.Web.Controllers.Texas
{
    public class MamanController : ApiController
    {        
        string MamanServiceUrl = new SettingQuery().GetSinglePMFromCahche().AmitalTaxesUrl + "/maman";

        [TokenAutherize]
        [HttpGet]
        [HttpPut]
        [HttpDelete]
        [HttpPost]        
        public HttpResponseMessage Test()
        {
            int tenant = HeaderHelper.Authenticate().Tenant;

            string path = Request.RequestUri.AbsolutePath.Replace("api/maman/", "");            
            string body = Request.Content.ReadAsStringAsync().Result;

            string url = "https://test-core-6-il-01-bgajc9hncvgjc4bp.israelcentral-01.azurewebsites.net/maman/api" + path;
            //string url = MamanServiceUrl + path;

            string jwt = new SettingQuery().GetJwtToken(tenant);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            StringContent stringContent = body != null ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json") : null;

            Task<HttpResponseMessage> resTask;

            switch (Request.Method.Method.ToUpper())
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