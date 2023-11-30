using Logitude.CommonTests.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;

namespace Logitude.CommonDataTests.Services
{
    public class TenantServices
    {
        public TenantPM GetByToken(string token)
        {
            string TenantUrl = Urls.TenantsGetSingle(UserTenant.Tenant);
            ApiResponse<TenantPM> tenant = APICaller.CallGet<TenantPM>(TenantUrl, token);
            return tenant.Data;
        }

        public TenantPM UpdateByToken(string token, TenantPM tenantPM)
        {
            ApiResponse<TenantPM> UpdatedAddressSettings = APICaller.CallPut<TenantPM>(tenantPM, Urls.TenantsUpdate(UserTenant.Tenant), token);
            return UpdatedAddressSettings.Data;
        }

    }
}
