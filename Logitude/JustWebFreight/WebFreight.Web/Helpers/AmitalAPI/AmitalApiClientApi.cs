using Logitude.BL.GlobalModel.EntityPMs;
using Logitude.BL.GlobalModel.EntityQueries;
using Logitude.BL.GlobalModel.Tools.EntityService;
using Newtonsoft.Json;
using Simplog.Global.Data.GlobalModel;
using System.Net.Http;
using WebFreight.Web.Helpers.AmitalAPI.Structs;

namespace WebFreight.Web.Helpers.AmitalAPI
{
    public class AmitalApiClientApi : AmitalApiCRUDApiBase<AmitalApiClient>
    {
        private static readonly string baseUrl = "clients";
        TenantManagementService tenantManagementService = new TenantManagementService(GlobalContext.GetContext());
        TenantManagementQuery tenantManagementQuery = new TenantManagementQuery();

        public AmitalApiClientApi() : base(baseUrl) { }

        public AmitalApiClient Get(string token, int tenant)
        {
            HttpClienResponse res = AmitalAPIHelper.SendRequest(token, $"{baseUrl}/query?Tenant={tenant}", HttpMethod.Get);
            if (!res.Res.IsSuccessStatusCode)
                return null;

            AmitalApiClient schema = JsonConvert.DeserializeObject<AmitalApiClient>(res.Content);
            return schema;
        }

        public override AmitalApiClient Get(string token, string id) => Get(token, int.Parse(id));


        public override AmitalApiClient Create(string token, AmitalApiClient clientData, string user)
        {
            AmitalApiClient newClient = base.Create(token, clientData, user);
            if (newClient != null)
            {
                TenantManagementPM tenantManagementPM = tenantManagementQuery.GetSinglePM(int.Parse(clientData.Tenant));
                tenantManagementPM.AmitalApiToken = newClient.Token;
                tenantManagementService.Update(tenantManagementPM);
            }

            return newClient;
        }
    }
}
