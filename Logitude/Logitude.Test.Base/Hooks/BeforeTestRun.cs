using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Models.Base;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.TestData;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.Test.Base.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupBasePreparationVariables()
        {
            GetLoginParameters();
            GetTenant();
            GetBasicArgsFromUser();
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
            User user = APICaller.CallPost<User>(loginParameters, "Authentication", null);
            LoginPreparationParameters.Token = user.Token;
            LoginPreparationParameters.LoginUserId = user.UserId;
            LoginPreparationParameters.LoginUserName = user.UserName;
        }

        private static void GetTenant()
        {
            string TenantUrl = "Tenants/GetSingle?id=" + BaseConfigurations.Tenant;
            TenantPM tenantPM = APICaller.CallGet<TenantPM>(TenantUrl, LoginPreparationParameters.Token, null);
            BasePreparationVariables.Tenant = tenantPM.Id;
            BasePreparationVariables.LocalCurrencyId = tenantPM.CurrencyId;
            BasePreparationVariables.ProfitCurrencyId = tenantPM.ProfitCurrencyId;
            BasePreparationVariables.ProfitCurrencyRate = tenantPM.ProfitCurrencyRate;
        }

        private static void GetBasicArgsFromUser()
        {
            string UserListUrl = "Userviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&GetCount=true&PageIndex=0&PageSize=23&Filter1Value=" + BaseConfigurations.Email;
            List<UserList> users = APICaller.CallGet<List<UserList>>(UserListUrl, LoginPreparationParameters.Token, "Result");
            var user = users.FirstOrDefault();

            BasePreparationVariables.UserId = user.Id;
            BasePreparationVariables.BranchId = user.BranchId;
            BasePreparationVariables.DepartmentId = user.DepartmentId;
            BasePreparationVariables.BusinessUnitId = user.BusinessUnitId;
        }
    }
}
