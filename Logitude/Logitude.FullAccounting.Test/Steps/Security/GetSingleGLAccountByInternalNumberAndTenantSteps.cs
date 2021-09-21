using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class GetSingleGLAccountByInternalNumberAndTenantSteps
    {
        private readonly FullAccountingContext context;
        public GetSingleGLAccountByInternalNumberAndTenantSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get  single gl account by internal number")]
        public void WhenGetSingleGlAccountByInternalNumber()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetSingleGLAccountByInternalNumberAndTenant(FullAccountingData.GLAccountInternalNumber, UserTenant.Tenant), UserOtherTenant.Token);
        }

        [Then(@"The get  single gl account by internal number And tenant API should return you have no permissions")]
        public void ThenTheGetSingleGlAccountByInternalNumberAndTenantAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
