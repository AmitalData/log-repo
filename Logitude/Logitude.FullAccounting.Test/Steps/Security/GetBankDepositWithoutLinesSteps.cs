using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.BankDeposit.Security
{
    [Binding]
    public class GetBankDepositWithoutLinesCheckSecuritySteps
    {
        private readonly FullAccountingContext context;
        public GetBankDepositWithoutLinesCheckSecuritySteps(FullAccountingContext context)
        {
            this.context = context;
        }


        [When(@"get bank deposit without lines from unauthorizes tenant")]
        public void WhenGetBankDepositWithoutLinesFromUnauthorizesTenant()
        {
            context.Action = () => APICaller.CallGet<object>(Urls.GetBankDepositWithoutLines(FullAccountingData.BankDeposit1), UserOtherTenant.Token);
        }

        [Then(@"The bank deposit without lines API should return you have no permissions")]
        public void ThenTheBankDepositWithoutLinesAPIShouldReturnYouHaveNoPermissions()
        {
            context.Action.Should().Throw<Exception>().And.Message.Should().Contain("have no permission");
        }

    }
}
