using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.APPayment
{
    [Binding]
    public class GetAPPaymentSteps
    {
        private readonly FullAccountingContext context;
        public GetAPPaymentSteps(FullAccountingContext context)
        {
            this.context = context;
        }
        [When(@"get ap payment with APPaymentId")]
        public void WhenGetApPaymentWithAPPaymentId()
        {
            context.APPaymentPM = APICaller.CallGet<APPaymentPM>(Urls.APPaymentsGetSingle(FullAccountingData.APPaymenId), UserTenant.Token).Data;
        }

        [Then(@"ap payment should be avaliable")]
        public void ThenApPaymentShouldBeAvaliable()
        {
            context.APPaymentPM.Should().NotBeNull();
        }
    }
}
