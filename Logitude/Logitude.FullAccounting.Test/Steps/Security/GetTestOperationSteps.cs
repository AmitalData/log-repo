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
    public class GetTestOperationSteps
    {
        private readonly FullAccountingContext context;
        public GetTestOperationSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get test operation by not login user")]
        public void WhenGetTestOperationByNotLoginUser()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetTestOperation("1", "M"), "0");
        }

        [When(@"get test operation by not authorize user")]
        public void WhenGetTestOperationByNotAuthorizeUser()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetTestOperation("1", "M"), UserEmptyTenant.Token);
        }

        [Then(@"the get test operation API should return you have no permissions")]
        public void ThenTheGetTestOperationAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }

    }
}
