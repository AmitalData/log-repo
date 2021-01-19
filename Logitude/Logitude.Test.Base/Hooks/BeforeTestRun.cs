using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Models.Base;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models;

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

            APIResponse<User> user= APICaller.CallPost<User>(loginParameters, URLs.UserAuthentication, null);
            UserTenant.Token = user.Data.Token;
            UserTenant.Tenant = user.Data.Tenant;
            UserTenant.LoginUserId = user.Data.UserId;
            UserTenant.LoginUserName = user.Data.UserName;
        }

        private static void FillUserTenant()
        {
            FillTenant();
            FillUser();
        }

        private static void FillTenant()
        {
            string TenantUrl = URLs.TenantsGetSingle + UserTenant.Tenant;
            APIResponse<TenantPM> tenantPM = APICaller.CallGet<TenantPM>(TenantUrl, UserTenant.Token);

            UserTenant.Tenant = tenantPM.Data.Id;
            UserTenant.LocalCurrencyId = tenantPM.Data.CurrencyId;
            UserTenant.ProfitCurrencyId = tenantPM.Data.ProfitCurrencyId;
            UserTenant.ProfitCurrencyRate = tenantPM.Data.ProfitCurrencyRate;
        }

        private static void FillUser()
        {
            //this method is waiting the CallGetByFilter to be implemented by Abd.M
            //string UserListUrl = URLs.UserviewsGetbyfilters + BaseConfigurations.Email;
            //List<UserList> users = APICaller.CallGetByFilter<List<UserList>>(UserListUrl, UserTenant.Token, "Result");
            //var user = users.FirstOrDefault();

            UserTenant.BranchId = "1-1102";//user.BranchId;
            UserTenant.DepartmentId = "1-2988";//user.DepartmentId;
            UserTenant.BusinessUnitId = "";//user.BusinessUnitId;
        }
    }
}
