using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.ARPayment
{
    [Binding]
    public class GetARPaymentSteps
    {
        private readonly FullAccountingContext context;
        private readonly ARPaymentService aRPaymentService;
        public GetARPaymentSteps(FullAccountingContext context, ARPaymentService aRPaymentService)
        {
            this.context = context;
            this.aRPaymentService = aRPaymentService;
        }

        [When(@"get ar payment with ARPaymentId")]
        public void WhenGetArPaymentWithARPaymentId()
        {
            context.ARPaymentPM = APICaller.CallGet<ARPaymentPM>(Urls.ARPaymentsGetSingle(FullAccountingData.ARPaymenId), UserTenant.Token).Data;
        }

        [Then(@"ar payment should be avaliable")]
        public void ThenArPaymentShouldBeAvaliable()
        {
            context.ARPaymentPM.Should().NotBeNull();
        }
    }
}
