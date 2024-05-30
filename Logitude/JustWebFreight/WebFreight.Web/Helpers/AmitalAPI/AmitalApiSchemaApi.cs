using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Http;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    public class AmitalApiSchemaApi : AmitalApiCRUDApiBase<AmitalApiSchema>
    {
        private static readonly string baseUrl = "schemas";

        public AmitalApiSchemaApi() : base(baseUrl) { }

        public AmitalApiSettings GetSettings(string token)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, "settings", HttpMethod.Get);
            if (!res.Res.IsSuccessStatusCode)
                return null;

            AmitalApiSettings settings = JsonConvert.DeserializeObject<AmitalApiSettings>(res.Content);
            return settings;
        }

        public object GetRequestQuery(string token, JObject parameters)
        {
            if(token == null || parameters == null)
                return null;

            string queryString = string.Join("&", parameters.Properties().Select(p => $"{p.Name}={Uri.EscapeDataString(p.Value.ToString())}"));
            string url = "RequestQuery?" + queryString;
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, url, HttpMethod.Get);
            if (!res.Res.IsSuccessStatusCode)
                return null;            

            return JsonConvert.DeserializeObject(res.Content);
        }
    }
}
