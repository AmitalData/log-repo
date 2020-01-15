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
            await GetTenant();
            await GetBasicArgsFromUser();
        }

        private static async Task GetTenant()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Tenants/GetSingle?id=" + IntegrationTestLoginParameters.Tenant);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            TenantPM tenantPM = JsonConvert.DeserializeObject<TenantPM>(stringResult);
            IntegrationTestLoginParameters.TenantPM = tenantPM;
            CorePreparationVariables.TenantPM = tenantPM;
        }


        public static async Task GetBasicArgsFromUser()
        {
            HttpResponseMessage response = await RestClientService.GetAsync("Userviews/getbyfilters?Filter1Value?=" + IntegrationTestLoginParameters.Email);
            var stringResult = response.Content.ReadAsStringAsync().Result;
            UserPM user = JsonConvert.DeserializeObject<UserPM>(stringResult);
            CorePreparationVariables.BranchId = user.BranchId;
            CorePreparationVariables.DepartmentId = user.DepartmentId;
            CorePreparationVariables.BusinessUnitId = user.BusinessUnitId;
        }
    }
}
