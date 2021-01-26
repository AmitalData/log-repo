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
        private Action act;

        public TenantSecurityAccessSteps( SecurityAccessStepsContext<TenantPM> context)
        {
            Context = context;
        }

        [When(@"Get Tenant request sent for User's Tenant")]
        public void WhenGetTenantRequestSentForUserSTenant()
        {
            Context.FirstUserPMData = GetTenant(UserTenant.Tenant, UserTenant.Token);
        }

        [Then(@"Tenant should be exists")]
        public void ThenTenantShouldBeExists()
        {
            Context.FirstUserPMData.Should().NotBeNull();
            Context.FirstUserPMData.Id.Should().Be(UserTenant.Tenant);
        }

        [When(@"Get Tenant request sent for other Tenant")]
        public void WhenGetTenantRequestSentForOtherTenant()
        {
            act = () => GetTenant(UserTenant.Tenant, UserOtherTenant.Token);
        }

        [Then(@"Tenant should not be exists")]
        public void ThenTenantShouldNotBeExists()
        {
            act.Should().ThrowExactly<Exception>()
                .Where(m => m.Message.Contains("Sorry you’re not authenticated to view company info"));
        }

        private TenantPM GetTenant(int Tenant, string Token)
        {
            string TenantUrl = Urls.TenantsGetSingle(Tenant);
            var tenant = APICaller.CallGet<TenantPM>(TenantUrl, Token);
            return tenant.Data;
        }

    }
}