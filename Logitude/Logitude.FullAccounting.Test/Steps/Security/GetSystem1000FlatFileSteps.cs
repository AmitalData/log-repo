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
    public class GetSystem1000FlatFileSteps
    {
        private readonly FullAccountingContext context;
        public GetSystem1000FlatFileSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"Get System (.*) Flat File by not login user")]
        public void WhenGetSystemFlatFileByNotLoginUser(int p0)
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetGenerate1000InAccountingOp("test@test.test"), "0");
        }

        [When(@"Get System (.*) Flat File by not authorize user")]
        public void WhenGetSystemFlatFileByNotAuthenticationUser(int p0)
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetGenerate1000InAccountingOp("test@test.test"), UserEmptyTenant.Token);
        }

        [Then(@"The Get System (.*) Flat File API should return you have no permissions")]
        public void ThenTheGetSystemFlatFileAPIShouldReturnYouHaveNoPermissions(int p0)
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
