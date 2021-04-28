using FluentAssertions;
using Logitude.CommonTests.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CommonTests.Steps.Security
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

        #region Get information for user's tenant
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
        #endregion

        #region Get information for other tenant
        [When(@"get information for other tenant")]
        public void WhenGetInformationForOtherTenant()
        {
            Context.act = () => GetUserTenant(UserOtherTenant.Token);
        }

        [Then(@"should receive error message say not authenticated to view company info")]
        public void ThenShouldReceiveErrorMessageSayNotAuthenticatedToViewCompanyInfo()
        {
            Context.act.Should().ThrowExactly<AggregateException>()
                .And.InnerExceptions[0].Message.Contains("Sorry you’re not authenticated to view company info");
        }
        #endregion

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