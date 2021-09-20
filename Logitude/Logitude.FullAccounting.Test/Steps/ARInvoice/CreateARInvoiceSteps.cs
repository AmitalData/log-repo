using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.ARInvoice
{
    [Binding]
    public class CreateARInvoiceSteps
    {
        private readonly FullAccountingContext context;
        private readonly ARInvoiceService arInvoiceService;
        public CreateARInvoiceSteps(FullAccountingContext context, ARInvoiceService arInvoiceService)
        {
            this.context = context;
            this.arInvoiceService = arInvoiceService;
        }
        [Given(@"I have the following AR invoice lines:")]
        public void GivenIHaveTheFollowingARInvoiceLines(Table table)
        {
            context.ARInvoiceLinePM = arInvoiceService.CreateLines(table);
        }

        [Given(@"a AR invoice with the following properties")]
        public void GivenAARInvoiceWithTheFollowingProperties(Table table)
        {
            context.ARInvoicePM = arInvoiceService.Create(table, context.ARInvoiceLinePM);
        }
        
        [When(@"create AR invoice")]
        public void WhenCreateARInvoice()
        {
            context.AddedARInvoicePM = APICaller.CallPost<ARInvoicePM>(context.ARInvoicePM, Urls.ARInvoicesController, UserTenant.Token)?.Data;
        }

        [Then(@"the AR invoice should create successfully")]
        public void ThenTheARInvoiceShouldCreateSuccessfully()
        {
            context.AddedARInvoicePM.Id.Should().NotBeNullOrEmpty();
        }
    }
}
