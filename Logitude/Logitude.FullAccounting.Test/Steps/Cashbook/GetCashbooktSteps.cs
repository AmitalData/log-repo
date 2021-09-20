using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.Cashbook
{
    [Binding]
    public class GetCashbooktSteps
    {
        private readonly FullAccountingContext context;
        public GetCashbooktSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get cashbookt with cashbooktId")]
        public void WhenGetCashbooktWithARPaymentId()
        {
            context.Cashbook = APICaller.CallGet<CashBookPM>(Urls.CashbooksSingle(FullAccountingData.CashBookCash1), UserTenant.Token).Data;
        }

        [Then(@"cashbookt should be avaliable")]
        public void ThenCashbooktShouldBeAvaliable()
        {
            context.Cashbook.Should().NotBeNull();
        }
    }
}
