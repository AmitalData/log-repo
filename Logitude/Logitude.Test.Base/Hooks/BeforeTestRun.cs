using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Models.Base;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.Test.Base.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun(Order = 0)]
        public static void SetupBasePreparationVariables()
        {
            GetLoginParameters();
            FillUserTenant();
        }

        private static void GetLoginParameters()
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = BaseConfigurations.Email,
                Password = BaseConfigurations.Password,
                ClientType = "Web",
                GetToken = true
            };

            User user = APICaller.CallPost<User>(loginParameters, URLs.UserAuthentication, null);
            UserTenant.Token = user.Token;
            UserTenant.Tenant = user.Tenant;
            UserTenant.LoginUserId = user.UserId;
            UserTenant.LoginUserName = user.UserName;
        }

        private static void FillUserTenant()
        {
            FillTenant();
            FillUser();
        }

        private static void FillTenant()
        {
            string TenantUrl = URLs.TenantsGetSingle + UserTenant.Tenant;
            TenantPM tenantPM = APICaller.CallGet<TenantPM>(TenantUrl, UserTenant.Token, null);

            UserTenant.Tenant = tenantPM.Id;
            UserTenant.LocalCurrencyId = tenantPM.CurrencyId;
            UserTenant.ProfitCurrencyId = tenantPM.ProfitCurrencyId;
            UserTenant.ProfitCurrencyRate = tenantPM.ProfitCurrencyRate;
        }

        private static void FillUser()
        {
            string UserListUrl = URLs.UserviewsGetbyfilters + BaseConfigurations.Email;
            List<UserList> users = APICaller.CallGet<List<UserList>>(UserListUrl, UserTenant.Token, "Result");
            var user = users.FirstOrDefault();

            UserTenant.BranchId = user.BranchId;
            UserTenant.DepartmentId = user.DepartmentId;
            UserTenant.BusinessUnitId = user.BusinessUnitId;
        }
    }
}
