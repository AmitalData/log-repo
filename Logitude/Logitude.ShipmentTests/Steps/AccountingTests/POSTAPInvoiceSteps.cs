using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.APInvoice;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps.AccountingTests
{
    [Binding]
    public class POSTAPInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        protected readonly ShipmentContext ShipmentContext;

        public POSTAPInvoiceSteps(AccountingContext accountingContext , ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
        }

        [When(@"create APInvoice")]
        public void WhenCreateAPInvoice()
        {
            ApiResponse<APInvoicePM> response = CreateAPInvoice(AccountingContext.ShipmentAPInvoice.APInvoicePM, AccountingContext.ShipmentAPInvoice.APInvoiceLinePM);
            AccountingContext.ShipmentAPInvoice.APInvoicePM.Id = response.Data?.Id;
        }

        [Then(@"the APInvoice should create successfully")]
        public void ThenTheAPInvoiceShouldCreateSuccessfully()
        {
            AccountingContext.ShipmentAPInvoice.APInvoicePM.Id.Should().NotBeNull();
        }

        private ApiResponse<APInvoicePM> CreateAPInvoice(APInvoicePM shipmentAPInvoice, APInvoiceLinePM aPInvoiceLinePM)
        {
            shipmentAPInvoice = AddAPInvoiceLine(shipmentAPInvoice , aPInvoiceLinePM);
            return APICaller.CallPost<APInvoicePM>(shipmentAPInvoice, Urls.APInvoicesController, UserTenant.Token);
        }

        private APInvoicePM AddAPInvoiceLine(APInvoicePM shipmentAPInvoice, APInvoiceLinePM APInvoiceLinePM)
        {
            APInvoiceLinePM = new APInvoiceLineBuilder().WithModel(APInvoiceLinePM)
                .EntityId(ShipmentContext.DirectShipment.Id)
                .Build();

            return new APInvoiceBuilder().WithModel(shipmentAPInvoice)
                .InvoiceLines(APInvoiceLinePM)
                .Build();
        }

    }
}
