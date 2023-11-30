using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.BankDeposit
{
    [Binding]
    public class GetBankDepositSteps
    {
        private readonly FullAccountingContext context;
        public GetBankDepositSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get bank deposit with bankDepositId")]
        public void WhenGetBankDepositWithBankDepositId()
        {
            context.BankDeposit = APICaller.CallGet<BankDepositPM>(Urls.BankDepositsSingle(FullAccountingData.BankDeposit1), UserTenant.Token).Data;
        }

        [Then(@"bank deposit should be available")]
        public void ThenBankDepositShouldBeAvailable()
        {
            context.BankDeposit.Should().NotBeNull();
        }
    }
}
