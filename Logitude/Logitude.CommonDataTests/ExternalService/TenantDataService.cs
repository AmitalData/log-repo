using Logitude.CommonTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;

namespace Logitude.CommonDataTests.DataService
{
    public class TenantDataService
    {
        public TenantPM GetUserTenantByToken(string token)
        {
            string TenantUrl = Urls.TenantsGetSingle(UserTenant.Tenant);
            var tenant = APICaller.CallGet<TenantPM>(TenantUrl, token);
            return tenant.Data;
        }

    }
}
