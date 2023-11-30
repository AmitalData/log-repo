using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.APPayment
{
    [Binding]
    public class CreateAPPaymentSteps
    {
        private readonly FullAccountingContext context;
        private readonly APPaymentService aPPaymentService;
        public CreateAPPaymentSteps(FullAccountingContext context, APPaymentService aPPaymentService)
        {
            this.context = context;
            this.aPPaymentService = aPPaymentService;
        }
        [Given(@"a ap payment with the following properties")]
        public void GivenAApPaymentWithTheFollowingProperties(Table table)
        {
            context.APPaymentPM = aPPaymentService.Create(table);
        }

        [When(@"create ap payment")]
        public void WhenCreateApPayment()
        {
            context.AddedAPPaymentPM = APICaller.CallPost<APPaymentPM>(context.APPaymentPM, Urls.APPaymentsController, UserTenant.Token)?.Data;
        }

        [Then(@"the ap payment should create successfully")]
        public void ThenTheApPaymentShouldCreateSuccessfully()
        {
            context.AddedAPPaymentPM.Id.Should().NotBeNullOrEmpty();
        }
    }
}
