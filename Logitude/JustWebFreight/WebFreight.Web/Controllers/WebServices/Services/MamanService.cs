using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.Utils;
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
        public static HttpResponseMessage Send(int tenant, string interfacename, object body)
        {
            CustomsPartnerFtpPM cpftp = new CustomsPartnerFtpQueryService(tenant).GetBy(tenant, interfacename);
            WebApiDefinitionDTO dtoWebApiDefinition = ProxyUtil.JsonConvertDeserializeTyped<WebApiDefinitionDTO>(cpftp.CommunicationDetails);
            string bodyString = JsonConvert.SerializeObject(new 
            {
                Url = dtoWebApiDefinition.WEBAPIURL,
                Username = dtoWebApiDefinition.User,
                dtoWebApiDefinition.Password,
                Body = body
            });

            HttpResponseMessage res = Send(dtoWebApiDefinition.ServiceUrl, bodyString, tenant, HttpMethod.Post);
            return res;
        }

        public static HttpResponseMessage Send(string url, string body, int tenant, HttpMethod method)
        {
            string jwt = new SettingQuery().GetJwtToken(tenant);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            StringContent stringContent = body != null ? new StringContent(body, Encoding.UTF8, "application/json") : null;

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