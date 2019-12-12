using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.IntegrationTest.Core.Login;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Core
{
    public class CorePreparationCalls
    {
        public static async Task PrepareVariables()
        {
            await GetAccountingCurriencyTenant();
        }

        private static async Task GetAccountingCurriencyTenant()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Tenants/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            TenantPM tenantPM = JsonConvert.DeserializeObject<TenantPM>(stringResult);
            IntegrationTestLoginParameters.TenantPM = tenantPM;
        }
    }
}
