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
        private APInvoiceLinePM APInvoiceLinePM;

        public POSTAPInvoiceSteps(AccountingContext accountingContext , ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
        }

        [Given(@"a payable receive invoice with the following properties")]
        public void GivenAPayableReceiveInvoiceWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentAPInvoice = CreateAPInvoiceInstance(table);
        }

        [Given(@"an invoice line with the following properties")]
        public void GivenAnInvoiceLineWithTheFollowingProperties(Table table)
        {
            APInvoiceLinePM = CreateAPInvoiceLineInstance(table);
        }

        [When(@"create APInvoice")]
        public void WhenCreateAPInvoice()
        {
            ApiResponse<APInvoicePM> response = CreateAPInvoice(AccountingContext.ShipmentAPInvoice, APInvoiceLinePM);
            AccountingContext.ShipmentAPInvoice.Id = response.Data?.Id;
        }

        [Then(@"the APInvoice should create successfully")]
        public void ThenTheAPInvoiceShouldCreateSuccessfully()
        {
            AccountingContext.ShipmentAPInvoice.Id.Should().NotBeNull();
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

        private APInvoicePM CreateAPInvoiceInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new APInvoiceBuilder().WithDefualtValues()
                .VendorId((string)dataTable.Vendor)
                .InvoiceNumber((int)dataTable.InvoiceNumber)
                .AmountDue((double)dataTable.InvoiceAmount)
                .AmountInInvoiceCurrency((double)dataTable.InvoiceAmount)
                .InvoiceExpectedAmount((double)dataTable.InvoiceAmount)
                .AmountInLocalCurrency((double)dataTable.InvoiceAmount)
                .AmountInProfitCurrency((double)dataTable.InvoiceAmount)
                .InvoiceCurrencyId((string)dataTable.InvoiceCurrency)
                .LocalCurrencyId((string)dataTable.InvoiceCurrency)
                .ProfitCurrencyId((string)dataTable.InvoiceCurrency)
                .InvoiceCurrencyExchangeRate((double)dataTable.ExchangeRate)
                .ProfitCurrencyExchangeRate((double)dataTable.ExchangeRate)
                .InvoiceDate((string)dataTable.InvoiceDate)
                .PaymentTermId((string)dataTable.PaymentTerms)
                .DueDate((string)dataTable.DueDate)
                .VATNumber((string)dataTable.VatNumber)
                .Build();
        }

        private APInvoiceLinePM CreateAPInvoiceLineInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new APInvoiceLineBuilder().WithDefualtValues()
                .ChargesTypeName((string)dataTable.ChargesTypeName)
                .ChargesTypeCode((string)dataTable.ChargesType)
                .ChargesTypeId((string)dataTable.ChargesType)
                .VatTypeId((string)dataTable.VatType)
                .VatTypeName((string)dataTable.VatType)
                .VatPercentage((double)dataTable.VatPrecentage)
                .ForiegnCurrencyAmount((double)dataTable.Amount)
                .InvoiceCurrencyAmount((double)dataTable.Amount)
                .LocalCurrencyAmount((double)dataTable.Amount)
                .ProfitCurrencyAmount((double)dataTable.Amount)
                .Description((string)dataTable.Description)
                .Build();
        }

    }
}
