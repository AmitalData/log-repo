using Logitude.CommonTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

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
