using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.ARPayment
{
    [Binding]
    public class GetARPaymentSteps
    {
        private readonly FullAccountingContext context;
        public GetARPaymentSteps(FullAccountingContext context)
        {
            this.context = context;
        }

        [When(@"get ar payment with ARPaymentId")]
        public void WhenGetArPaymentWithARPaymentId()
        {
            context.ARPaymentPM = APICaller.CallGet<ARPaymentPM>(Urls.ARPaymentsGetSingle(FullAccountingData.ARPaymenId), UserTenant.Token).Data;
        }

        [Then(@"ar payment should be available")]
        public void ThenArPaymentShouldBeAvailable()
        {
            context.ARPaymentPM.Should().NotBeNull();
        }
    }
}
