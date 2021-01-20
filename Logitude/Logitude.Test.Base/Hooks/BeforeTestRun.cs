using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Models;
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
            GetUsersLoginParameters();
            FillUserTenant();
        }

        private static void GetUsersLoginParameters()
        {
            GetUserLoginParameters();
            GetOtherUserLoginParameters();
        }

        private static void FillUserTenant()
        {
            FillTenant();
            FillUser();
        }

        private static void GetUserLoginParameters()
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = BaseConfigurations.Email,
                Password = BaseConfigurations.Password,
                ClientType = "Web",
                GetToken = true
            };

            APIResponse<User> user= APICaller.CallPost<User>(loginParameters, URLs.UserAuthentication(), null);
            UserTenant.Token = user.Data.Token;
            UserTenant.Tenant = user.Data.Tenant;
            UserTenant.UserId = user.Data.UserId;
            UserTenant.UserName = user.Data.UserName;
        }

        private static void GetOtherUserLoginParameters()
        {
            LoginParameters loginParameters = new LoginParameters()
            {
                Email = BaseConfigurations.OtherUserEmail,
                Password = BaseConfigurations.OtherUserPassword,
                ClientType = "Web",
                GetToken = true
            };

            APIResponse<User> user = APICaller.CallPost<User>(loginParameters, URLs.UserAuthentication(), null);
            UserOtherTenant.Token = user.Data.Token;
            UserOtherTenant.Tenant = user.Data.Tenant;
            UserOtherTenant.UserId = user.Data.UserId;
            UserOtherTenant.UserName = user.Data.UserName;
        }

        private static void FillTenant()
        {
            string tenantUrl = URLs.TenantsGetSingle(UserTenant.Tenant);
            APIResponse<Tenant> tenantPM = APICaller.CallGet<Tenant>(tenantUrl, UserTenant.Token);

            UserTenant.LocalCurrencyId = tenantPM.Data.CurrencyId;
            UserTenant.ProfitCurrencyId = tenantPM.Data.ProfitCurrencyId;
            UserTenant.ProfitCurrencyRate = tenantPM.Data.ProfitCurrencyRate;
        }

        private static void FillUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "SearchFields",
                Filter1Operator = "Contains",
                Filter1Value = BaseConfigurations.Email
            };

            APIResponse<IEnumerable<UserList>> usersResponse = APICaller.CallGetByFilters<IEnumerable<UserList>>(URLs.UserViewsGetByFilters(), UserTenant.Token, apiQueryFilters);
            UserList user = usersResponse.Data?.FirstOrDefault();

            UserTenant.BranchId = user?.BranchId;
            UserTenant.DepartmentId = user?.DepartmentId;
            UserTenant.BusinessUnitId = user?.BusinessUnitId;
        }
    }
}