using Newtonsoft.Json;
using System;
using System.Net.Http;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    public class AmitalApiClientApi : AmitalApiCRUDApiBase<AmitalApiClient>
    {
        private static readonly string baseUrl = "clients";

        public AmitalApiClientApi() : base(baseUrl) { }

        public AmitalApiClient Get(string token, int tenant)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, $"{baseUrl}/query?Tenant={tenant}", HttpMethod.Get);
            if (!res.Res.IsSuccessStatusCode)
                return null;

            AmitalApiClient schema = JsonConvert.DeserializeObject<AmitalApiClient>(res.Content);
            return schema;
        }

        //public override AmitalApiClient Get(string token, string id) => throw new NotImplementedException();
        public override AmitalApiClient Get(string token, string id) => Get(token, int.Parse(id));

    }
}
