using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.BankAccount
{
    [Binding]
    public class GetBankAccountSteps
    {
        private readonly FullAccountingContext context;
        public GetBankAccountSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get bank account with bankAccountId")]
        public void WhenGetBankAccountWithBankAccountId()
        {
            context.BankAccountPM = APICaller.CallGet<BankAccountPM>(Urls.BankAccountsGetSingle(FullAccountingData.BankAccountId), UserTenant.Token).Data;
        }

        [Then(@"bank account should be available")]
        public void ThenBankAccountShouldBeAvailable()
        {
            context.BankAccountPM.Should().NotBeNull();
        }
    }
}
