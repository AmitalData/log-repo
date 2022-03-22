using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.APInvoice;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps.AccountingTests
{
    [Binding]
    public class AccountingSharedSteps
    {
        protected readonly AccountingContext AccountingContext;
        protected readonly ShipmentContext ShipmentContext;

        public AccountingSharedSteps(AccountingContext accountingContext, ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
            AccountingContext.ShipmentARInvoice = new ShipmentARInvoice();
            AccountingContext.ShipmentAPInvoice = new ShipmentAPInvoice();
        }

        [Given(@"a receivable with the following properties")]
        public void GivenAReceivableWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentARInvoice.receivablePM = CreateReceivableInstance(table);
            ShipmentContext.DirectShipment = AddReceivableToShipment(ShipmentContext.DirectShipment, AccountingContext.ShipmentARInvoice.receivablePM);
            ShipmentContext.DirectShipment = APICaller.CallPut<ShipmentPM>(ShipmentContext.DirectShipment, Urls.ShipmentController, UserTenant.Token).Data;
        }

        [Given(@"a payable with the following properties")]
        public void GivenAPayableWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentARInvoice.receivablePM = CreateReceivableInstance(table);
            ShipmentContext.DirectShipment = AddReceivableToShipment(ShipmentContext.DirectShipment, AccountingContext.ShipmentARInvoice.receivablePM);
            ShipmentContext.DirectShipment = APICaller.CallPut<ShipmentPM>(ShipmentContext.DirectShipment, Urls.ShipmentController, UserTenant.Token).Data;
        }


        [Given(@"a receivable receive invoice with the following properties")]
        public void GivenAReceivableReceiveInvoiceWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentARInvoice.ARInvoicePM = CreateARInvoiceInstance(table);

        }

        [Given(@"an receivable invoice line with the following properties")]
        public void GivenAnReceivableInvoiceLineWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentARInvoice.ARInvoiceLinePM = CreateARInvoiceLineInstance(table);

        }

        [Given(@"a payable receive invoice with the following properties")]
        public void GivenAPayableReceiveInvoiceWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentAPInvoice.APInvoicePM = CreateAPInvoiceInstance(table);
        }

        [Given(@"an invoice line with the following properties")]
        public void GivenAnInvoiceLineWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentAPInvoice.APInvoiceLinePM = CreateAPInvoiceLineInstance(table);
        }

        private ShipmentPM AddReceivableToShipment(ShipmentPM shipment, ShipmentReceivablePM receivable)
        {
            receivable = new ReceivableBuilder().WithModel(receivable)
                .ShipmentId(shipment.Id)
                .ShipmentNumber(shipment.ShipmentNumber)
                .Build();

            return new ShipmentBuilder().WithModel(shipment)
                .ShipmentReceivable(receivable)
                .ShipmentReceivableStatusCode("OPEN")
                .ShipmentReceivableStatusName("Open")
                .Build();
        }

        private ShipmentReceivablePM CreateReceivableInstance(Table DataTable)
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
                .MeasurementCode("GRWT")
                .MeasurementId("GRWT")
                .MeasurementIdByCode("GRWT")
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
                .LocalCurrencyAmount((double)dataTable.Amount*3.8)
                .ProfitCurrencyAmount((double)dataTable.Amount*3.8)
                .Description((string)dataTable.Description)
                .ForiegnCurrencyId((string)dataTable.Currency)
                .ForiegnCurrencyCode((string)dataTable.Currency)
                .ForiegnExchangeRate((double)dataTable.ExchangeRate)
                .Quantity((double)dataTable.Quantity)
                .UnitPrice((double)dataTable.UnitPrice)
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
