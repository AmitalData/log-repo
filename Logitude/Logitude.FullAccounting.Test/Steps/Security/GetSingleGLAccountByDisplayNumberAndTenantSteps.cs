using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class GetSingleGLAccountByDisplayNumberAndTenantSteps
    {
        private readonly FullAccountingContext context;
        public GetSingleGLAccountByDisplayNumberAndTenantSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get  single gl account")]
        public void WhenGetSingleGlAccount()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetSingleGLAccountByDispalyNumberAndTenant("19216811", UserTenant.Tenant),UserOtherTenant.Token);
        }

        [Then(@"The get  single gl account by display number And tenant API should return you have no permissions")]
        public void ThenTheGetSingleGlAccountByDisplayNumberAndTenantAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
