using FluentAssertions;
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.FullAccounting.Test.Steps.APInvoice
{
    [Binding]
    public class CreateAPInvoiceSteps
    {
        private readonly FullAccountingContext context;
        private readonly APInvoiceService apInvoiceService;
        public CreateAPInvoiceSteps(FullAccountingContext context, APInvoiceService apInvoiceService)
        {
            this.context = context;
            this.apInvoiceService = apInvoiceService;
        }
        [Given(@"I have the following AP invoice lines:")]
        public void GivenIHaveTheFollowingAPInvoiceLines(Table table)
        {
            context.APInvoiceLinePM = apInvoiceService.CreateLines(table);
        }

        [Given(@"a AP invoice with the following properties")]
        public void GivenAAPInvoiceWithTheFollowingProperties(Table table)
        {
            context.APInvoicePM = apInvoiceService.Create(table, context.APInvoiceLinePM);
        }


        [When(@"create AP invoice")]
        public void WhenCreateAPInvoice()
        {
            context.AddedAPInvoicePM = APICaller.CallPost<APInvoicePM>(context.APInvoicePM, Urls.APInvoicesController, UserTenant.Token)?.Data;
        }

        [Then(@"the AP invoice should create successfully")]
        public void ThenTheAPInvoiceShouldCreateSuccessfully()
        {
            context.AddedAPInvoicePM.Id.Should().NotBeNullOrEmpty();
        }
    }
}
