using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.PaymentCheques
{
    [Binding]
    public class CreatePaymentChequesSteps
    {
        private readonly FullAccountingContext context;
        private readonly PaymentChequesService paymentChequesService;
        public CreatePaymentChequesSteps(FullAccountingContext context, PaymentChequesService paymentChequesService)
        {
            this.context = context;
            this.paymentChequesService = paymentChequesService;
        }

        [Given(@"a payment cheques with the following properties")]
        public void GivenAPaymentChequesWithTheFollowingProperties(Table table)
        {
            context.PaymentChequeCreated = paymentChequesService.Create(table);
        }

        [When(@"create payment cheques")]
        public void WhenCreatePaymentCheques()
        {
            context.AddedPaymentCheque = APICaller.CallPost<PaymentChequePM>(context.PaymentChequeCreated, Urls.PaymentChequesController, UserTenant.Token)?.Data;
        }

        [Then(@"the payment cheques should create successfully")]
        public void ThenThePaymentChequesShouldCreateSuccessfully()
        {
            context.AddedPaymentCheque.Id.Should().NotBeNullOrEmpty();
        }
    }
}
