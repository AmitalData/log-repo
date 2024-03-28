using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    public class AmitalApiClientapiApi : AmitalApiCRUDApiBase<AmitalApiClientapi>
    {
        private static readonly string baseUrl = "clientapis";

        public AmitalApiClientapiApi() : base(baseUrl) { }
    }
}
