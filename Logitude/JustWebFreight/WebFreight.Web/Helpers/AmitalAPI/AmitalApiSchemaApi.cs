using Newtonsoft.Json;
using System.Net.Http;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    public class AmitalApiSchemaApi : AmitalApiCRUDApiBase<AmitalApiSchema>
    {
        private static readonly string baseUrl = "schemas";

        public AmitalApiSchemaApi() : base(baseUrl) { }

        public AmitalApiSettings GetSettings(string token, int tenant)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, "settings", HttpMethod.Get);
            if (!res.Res.IsSuccessStatusCode)
                return null;

            AmitalApiSettings settings = JsonConvert.DeserializeObject<AmitalApiSettings>(res.Content);
            return settings;
        }
    }
}
