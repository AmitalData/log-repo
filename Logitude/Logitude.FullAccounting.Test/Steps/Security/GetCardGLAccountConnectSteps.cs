using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Security
{
    [Binding]
    public class GetCardGLAccountConnectSteps
    {
        private readonly FullAccountingContext context;
        public GetCardGLAccountConnectSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get bank accounts summary without token")]
        public void WhenGetBankAccountsSummaryWithoutToken()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetCardGLAccountConnect(1),"123");
        }

        [Then(@"The get card glAccount connect API should return you have no permissions")]
        public void ThenTheGetCardGlAccountConnectAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
