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
    public class CreateARPaymentSteps
    {
        private readonly FullAccountingContext context;
        private readonly ARPaymentService arPaymentService;
        public CreateARPaymentSteps(FullAccountingContext context, ARPaymentService arPaymentService)
        {
            this.context = context;
            this.arPaymentService = arPaymentService;
        }

        [Given(@"a ar payment with the following properties")]
        public void GivenAArPaymentWithTheFollowingProperties(Table table)
        {
            context.ARPaymentPM = arPaymentService.Create(table);
        }

        [When(@"create ar payment")]
        public void WhenCreateArPayment()
        {
            context.AddedARPaymentPM = APICaller.CallPost<ARPaymentPM>(context.ARPaymentPM, Urls.ARPaymentController, UserTenant.Token)?.Data;
        }

        [Then(@"the ar payment should create successfully")]
        public void ThenTheArPaymentShouldCreateSuccessfully()
        {
            context.AddedARPaymentPM.Id.Should().NotBeNullOrEmpty();
        }
    }
}
