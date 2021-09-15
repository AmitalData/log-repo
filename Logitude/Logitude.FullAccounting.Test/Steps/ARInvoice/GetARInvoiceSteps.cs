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
    public class GetARInvoiceSteps
    {
        private readonly FullAccountingContext context;
        private readonly ARInvoiceService arInvoiceService;
        public GetARInvoiceSteps(FullAccountingContext context, ARInvoiceService arInvoiceService)
        {
            this.context = context;
            this.arInvoiceService = arInvoiceService;
        }
        [When(@"get ar invoice with ARInvoiceId")]
        public void WhenGetArInvoiceWithARInvoiceId()
        {
            context.ARInvoicePM = APICaller.CallGet<ARInvoicePM>(Urls.ARInvoicesGetSingle(FullAccountingData.ARInvoiceId), UserTenant.Token).Data;
        }

        [Then(@"ar invoice should be avaliable")]
        public void ThenArInvoiceShouldBeAvaliable()
        {
            context.ARInvoicePM.Should().NotBeNull();
        }
    }
}
