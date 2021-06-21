using Logitude.CommonTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.CommonDataTests.ExternalServices
{
    public class TenantExternalServices
    {
        public TenantPM GetUserTenantByToken(string token)
        {
            string TenantUrl = Urls.TenantsGetSingle(UserTenant.Tenant);
            ApiResponse<TenantPM> tenant = APICaller.CallGet<TenantPM>(TenantUrl, token);
            return tenant.Data;
        }

        public TenantPM UpdateUsersLocalSettings(string Token, TenantPM localSettings)
        {
            ApiResponse<TenantPM> UpdatedAddressSettings = APICaller.CallPut<TenantPM>(localSettings, Urls.TenantsUpdate(UserTenant.Tenant), Token);
            return UpdatedAddressSettings.Data;
        }

        public TenantPM GetTenantByToken(string token, int tenant)
        {
            string TenantUrl = Urls.TenantsGetSingle(tenant);
            ApiResponse<TenantPM> tenantBM = APICaller.CallGet<TenantPM>(TenantUrl, token);
            return tenantBM.Data;
        }

    }
}
