using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.BankAccount.Security
{
    [Binding]
    public class GetBankAccountsSummaryCheckSecuritySteps
    {
        private readonly FullAccountingContext context;
        public GetBankAccountsSummaryCheckSecuritySteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get bank accounts summary")]
        public void WhenGetBankAccountsSummary()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetBankAccountsSummary, UserEmptyTenant.Token);
        }

        [Then(@"The bank accounts summary API should return you have no permissions")]
        public void ThenTheBankAccountsSummaryAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }
    }
}
