using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps.AccountingTests
{
    [Binding]
    public class POSTARInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        protected readonly ShipmentContext ShipmentContext;

        public POSTARInvoiceSteps(AccountingContext accountingContext, ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
        }

        [When(@"create ARInvoice")]
        public void WhenCreateARInvoice()
        {
            ApiResponse<ARInvoicePM> response = CreateARInvoice(AccountingContext.ShipmentARInvoice.ARInvoicePM, AccountingContext.ShipmentARInvoice.ARInvoiceLinePM);
            AccountingContext.ShipmentARInvoice.ARInvoicePM.Id = response.Data?.Id;
        }

        [Then(@"the ARInvoice should create successfully")]
        public void ThenTheARInvoiceShouldCreateSuccessfully()
        {
            AccountingContext.ShipmentARInvoice.ARInvoicePM.Id.Should().NotBeNull();
        }

        private ApiResponse<ARInvoicePM> CreateARInvoice(ARInvoicePM shipmentARInvoice, ARInvoiceLinePM ARInvoiceLinePM)
        {
            shipmentARInvoice = AddARInvoiceLine(shipmentARInvoice, ARInvoiceLinePM);
            return APICaller.CallPost<ARInvoicePM>(shipmentARInvoice, Urls.ARInvoicesController, UserTenant.Token);
        }

        private ARInvoicePM AddARInvoiceLine(ARInvoicePM shipmentARInvoice, ARInvoiceLinePM ARInvoiceLinePM)
        {
            ARInvoiceLinePM = new ARInvoiceLineBuilder().WithModel(ARInvoiceLinePM)
                .EntityId(ShipmentContext.DirectShipment.Id)
                .Build();

            return new ARInvoiceBuilder().WithModel(shipmentARInvoice)
                .InvoiceLines(ARInvoiceLinePM)
                .ShipmentsNumbers(ShipmentContext.DirectShipment.ShipmentNumber)
                .Build();
        }
    }
}
