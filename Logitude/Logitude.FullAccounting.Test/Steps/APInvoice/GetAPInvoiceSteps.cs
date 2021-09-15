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
    public class GetAPInvoiceSteps
    {
        private readonly FullAccountingContext context;
        private readonly APInvoiceService apInvoiceService;
        public GetAPInvoiceSteps(FullAccountingContext context, APInvoiceService apInvoiceService)
        {
            this.context = context;
            this.apInvoiceService = apInvoiceService;
        }
        [When(@"get ap invoice with ARInvoiceId")]
        public void WhenGetApInvoiceWithARInvoiceId()
        {
            context.APInvoicePM = APICaller.CallGet<APInvoicePM>(Urls.APInvoicesGetSingle(FullAccountingData.APInvoiceId), UserTenant.Token).Data;
        }

        [Then(@"ap invoice should be avaliable")]
        public void ThenApInvoiceShouldBeAvaliable()
        {
            context.APInvoicePM.Should().NotBeNull();
        }
    }
}
