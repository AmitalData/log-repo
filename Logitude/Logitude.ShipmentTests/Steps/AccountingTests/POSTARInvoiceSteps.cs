using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
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
    public class POSTARInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        protected readonly ShipmentContext ShipmentContext;
        private ReceivablePM receivablePM;
        private ARInvoicePM PutResponse;
        private ARInvoiceLinePM ARInvoiceLinePM;

        public POSTARInvoiceSteps(AccountingContext accountingContext, ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
        }

        [Given(@"a receivable with the following properties")]
        public void GivenAReceivableWithTheFollowingProperties(Table table)
        {
            receivablePM = CreateReceivableInstance(table);
            ShipmentContext.DirectShipment = AddReceivableToShipment(ShipmentContext.DirectShipment, receivablePM);
            ShipmentContext.DirectShipment = APICaller.CallPut<ShipmentPM>(ShipmentContext.DirectShipment, Urls.ShipmentController, UserTenant.Token).Data;
        }

        [Given(@"a receivable receive invoice with the following properties")]
        public void GivenAReceivableReceiveInvoiceWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentARInvoice = CreateARInvoiceInstance(table);

        }

        [Given(@"an receivable invoice line with the following properties")]
        public void GivenAnReceivableInvoiceLineWithTheFollowingProperties(Table table)
        {
            ARInvoiceLinePM = CreateARInvoiceLineInstance(table);

        }

        [When(@"create ARInvoice")]
        public void WhenCreateARInvoice()
        {
            ApiResponse<ARInvoicePM> response = CreateARInvoice(AccountingContext.ShipmentARInvoice, ARInvoiceLinePM);
            AccountingContext.ShipmentARInvoice.Id = response.Data?.Id;
        }

        [Then(@"the ARInvoice should create successfully")]
        public void ThenTheARInvoiceShouldCreateSuccessfully()
        {
            AccountingContext.ShipmentARInvoice.Id.Should().NotBeNull();
        }

        private ShipmentPM AddReceivableToShipment(ShipmentPM shipment, ReceivablePM receivable)
        {
            receivable = new ReceivableBuilder().WithModel(receivable)
                .ShipmentId(shipment.Id)
                .ShipmentNumber(shipment.ShipmentNumber)
                .Build();

            return new ShipmentBuilder().WithModel(shipment)
                .ShipmentReceivable(receivable)
                .Build();
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

        private ReceivablePM CreateReceivableInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ReceivableBuilder().WithDefualtValues()
                .ChargesTypeName((string)dataTable.ChargesTypeName)
                .ChargesTypeCode((string)dataTable.ChargesType)
                .ChargesTypeIdByCode((string)dataTable.ChargesType)
                .MeasurementCode((string)dataTable.Measurement)
                .MeasurementIdByCode((string)dataTable.Measurement)
                .CurrencyCode((string)dataTable.Currency)
                .CurrencyIdByCode((string)dataTable.Currency)
                .Rate((double)dataTable.Rate)
                .Quantity((double)dataTable.Quantity)
                .UnitPrice((double)dataTable.UnitPrice)
                .TotalAmount(100)
                .TotalAmountLocal(380)
                .ShipmentReceivableLineStatusCode((string)dataTable.ShipmentReceivableLineStatusCode)
                .ChangeSetOp(ChangeSetOperation.Insert)
                .Build();
        }

        private ARInvoicePM CreateARInvoiceInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ARInvoiceBuilder().WithDefualtValues()
                .BillToId((string)dataTable.Customer)
                .PartnerId((string)dataTable.Customer)
                .AmountDue((double)dataTable.InvoiceAmount)
                .AmountInInvoiceCurrency((double)dataTable.InvoiceAmount)
                .AmountInLocalCurrency(((double)dataTable.InvoiceAmount) * 3.8)
                .AmountInProfitCurrency(((double)dataTable.InvoiceAmount) * 3.8)
                .InvoiceCurrencyId((string)dataTable.InvoiceCurrency)
                .LocalCurrencyId((string)dataTable.InvoiceCurrency)
                .ProfitCurrencyId((string)dataTable.InvoiceCurrency)
                .InvoiceCurrencyExchangeRate((double)dataTable.ExchangeRate)
                .ProfitCurrencyExchangeRate((double)dataTable.ExchangeRate)
                .InvoiceDate((string)dataTable.InvoiceDate)
                .PaymentTermId((string)dataTable.PaymentTerms)
                .DueDate((string)dataTable.DueDate)
                .VATNumber((string)dataTable.VatNumber)
                .SubTotalInInvoiceCurrency(100)
                .SubTotalInLocalCurrency(380)
                .Build();
        }

        private ARInvoiceLinePM CreateARInvoiceLineInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ARInvoiceLineBuilder().WithDefualtValues()
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
                .ForiegnCurrencyId((string)dataTable.Currency)
                .ForiegnCurrencyCode((string)dataTable.Currency)
                .ForiegnExchangeRate((double)dataTable.ExchangeRate)
                .Quantity((double)dataTable.Quantity)
                .UnitPrice((double)dataTable.UnitPrice)
                .Build();
        }

    }
}
