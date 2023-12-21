using Logitude.BL.GlobalModel.EntityQueries;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace WebFreight.Web.Controllers.WebServices.Services
{
    public class ShaamService
    {
        string AmitalTaxesUrl = new SettingQuery().GetSinglePMFromCahche().AmitalTaxesUrl;

        public HttpClienResponse LinkToCodeForToken(int tenant, string user)
        {
            string url = $"{AmitalTaxesUrl}taxes/linkToCodeForNewToken?user={user}";
            HttpClienResponse res = SendShaamApiHttpRequest(tenant, url, HttpMethod.Get);
            return res;
        }

        public HttpClienResponse Tokens(int tenant, int pageSize, int page)
        {
            string url = $"taxes/tokens?pageSize={pageSize}&page={page}";
            HttpClienResponse res = SendShaamApiHttpRequest(tenant, url, HttpMethod.Get);
            return res;
        }

        public HttpClienResponse NewRefreshToken(int tenant, string user, string code)
        {
            string url = $"taxes/createNewRefreshToken?user={user}&code={code}";
            HttpClienResponse res = SendShaamApiHttpRequest(tenant, url, HttpMethod.Post);
            return res;
        }

        public HttpClienResponse CreateConfirmationNumber(string invoiceJson, int tenant, int? confirmationTokenLogId = null)
        {
            string url = $"taxes/createConfirmationNumber?confirmationTokenLogId={confirmationTokenLogId}";
            HttpClienResponse res = SendShaamApiHttpRequest(tenant, url, HttpMethod.Post, invoiceJson);
            return res;
        }

        private HttpClienResponse SendShaamApiHttpRequest(int tenant, string url, HttpMethod httpMethod, object body = null)
        {
            HttpResponseMessage res;

            string jwt = new SettingQuery().GetJwtToken(tenant);
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            url = AmitalTaxesUrl + url;
            
            if (httpMethod == HttpMethod.Post)
            {
                StringContent stringContent = body != null ? new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json") : null;
                res = client.PostAsync(url, stringContent).Result;
            }
            else
                res = client.GetAsync(url).Result;

            string content = res.Content.ReadAsStringAsync().Result;
            client.Dispose();

            return new HttpClienResponse { Content = content, Res = res };
        }
    }

    public class HttpClienResponse
    {
        public HttpResponseMessage Res { get; set; }
        public string Content { get; set; }
    }

    public class ApiToShaamRes
    {
        public virtual bool approved { get; set; }
        public virtual int status { get; set; }
        public virtual ErrorCodeApiToShaamRes? errorCode { get; set; }
        public string message { get; set; }
    }

    public enum ErrorCodeApiToShaamRes { GENERAL = 1, REFRESH_TOKEN_NOT_VALID, INVOICE_SCHEMA, SOME_REFRESH_TOKEN_NOT_VLID }
}