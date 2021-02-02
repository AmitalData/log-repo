using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.Security
{
    [Binding]
    public class TenantSecurityAccessSteps
    {
        private SecurityAccessStepsContext<TenantPM> Context;

        public TenantSecurityAccessSteps( SecurityAccessStepsContext<TenantPM> context)
        {
            Context = context;
        }

        #region Step Region
        [When(@"get information for user's tenant")]
        public void WhenGetInformationForUsersTenant()
        {
            Context.FirstUserPMData = GetUserTenant(UserTenant.Token);
        }

        [Then(@"tenant information should available")]
        public void ThenTenantInformationShouldAvailable()
        {
            Context.FirstUserPMData.Should().NotBeNull();
        }
        
        [When(@"get information for other tenant")]
        public void WhenGetInformationForOtherTenant()
        {
            Context.act = () => GetUserTenant(UserOtherTenant.Token);
        }

        [Then(@"should receive error message say not authenticated to view company info")]
        public void ThenShouldReceiveErrorMessageSayNotAuthenticatedToViewCompanyInfo()
        {
            Context.act.Should().ThrowExactly<Exception>().Where(m => m.Message.Contains("Sorry you’re not authenticated to view company info"));
        }
        #endregion

        #region Private Function Region
        private TenantPM GetUserTenant(string token)
        {
            string TenantUrl = Urls.TenantsGetSingle(UserTenant.Tenant);
            var tenant = APICaller.CallGet<TenantPM>(TenantUrl, token);
            return tenant.Data;
        }
        #endregion
    }
}