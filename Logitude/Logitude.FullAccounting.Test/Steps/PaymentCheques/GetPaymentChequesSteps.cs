using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.PaymentCheques
{
    [Binding]
    public class GetPaymentChequesSteps
    {
        private readonly FullAccountingContext context;
        public GetPaymentChequesSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get payment cheques with paymentChequesId")]
        public void WhenGetPaymentChequesWithPaymentChequesId()
        {
            context.PaymentCheque = APICaller.CallGet<PaymentChequePM>(Urls.PaymentChequesGetSingle(FullAccountingData.PaymentCheque1), UserTenant.Token).Data;
        }

        [Then(@"payment cheques should be available")]
        public void ThenPaymentChequesShouldBeAvailable()
        {
            context.PaymentCheque.Should().NotBeNull();
        }
    }
}
