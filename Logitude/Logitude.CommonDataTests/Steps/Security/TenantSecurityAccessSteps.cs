using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Constants;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
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
            Context.SecondUserPMData = GetTenant(UserTenant.Tenant, UserOtherTenant.Token);
        }

        [Then(@"Tenant should not be exists")]
        public void ThenTenantShouldNotBeExists()
        {
            Context.SecondUserPMData.Should().BeNull();
        }

        private TenantPM GetTenant(int Tenant, string Token)
        {
            string TenantUrl = URLs.TenantsGetSingle(Tenant);
            var tenant = APICaller.CallGet<TenantPM>(TenantUrl, Token);
            return tenant.Data;
        }

    }
}
