using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.APInvoice;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps.AccountingTests
{
    [Binding]
    public class PUTAPInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        protected readonly ShipmentContext ShipmentContext;
        private APInvoicePM PutResponse;
        private APInvoiceLinePM  SecondAPInvoiceLinePM;

        public PUTAPInvoiceSteps(AccountingContext accountingContext, ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
        }

        [When(@"update APInvoice by adding invoice line with the following properties")]
        public void WhenUpdateAPInvoiceByAddingInvoiceLineWithTheFollowingProperties(Table table)
        {
            AccountingContext.ShipmentAPInvoice.APInvoicePM = CreateAPInvoice(AccountingContext.ShipmentAPInvoice.APInvoicePM, AccountingContext.ShipmentAPInvoice.APInvoiceLinePM);
            SecondAPInvoiceLinePM = CreateAPInvoiceLineInstance(table);
            ApiResponse<APInvoicePM> response = UpdateAPInvoice(AccountingContext.ShipmentAPInvoice.APInvoicePM, SecondAPInvoiceLinePM);
            PutResponse = response.Data;
        }

        [Then(@"the APInvoice should update successfully")]
        public void ThenTheAPInvoiceShouldUpdateSuccessfully()
        {
            PutResponse.Id.Should().NotBeNull();
        }

        private ShipmentPM CreateAndGetShipment(ShipmentPM shipmentPM)
        {
            ApiResponse<ShipmentPM> PostResponse = APICaller.CallPost<ShipmentPM>(shipmentPM, Urls.ShipmentController, UserTenant.Token);
            ShipmentPM shipment = PostResponse.Data;

            string singleShipmentUrl = Urls.ShipmentGetSingle(shipment?.Id);

            ApiResponse<ShipmentPM> GetResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return GetResponse.Data;
        }

        private APInvoicePM CreateAPInvoice(APInvoicePM shipmentAPInvoice, APInvoiceLinePM firstAPInvoiceLinePM)
        {
            shipmentAPInvoice = AddAPInvoiceLine(shipmentAPInvoice, firstAPInvoiceLinePM);
            ApiResponse<APInvoicePM> response = APICaller.CallPost<APInvoicePM>(shipmentAPInvoice, Urls.APInvoicesController, UserTenant.Token);
            return response.Data;
        }

        private ApiResponse<APInvoicePM> UpdateAPInvoice(APInvoicePM shipmentAPInvoice, APInvoiceLinePM secondAPInvoiceLinePM)
        {
            shipmentAPInvoice = AddNewAPInvoiceLine(shipmentAPInvoice, secondAPInvoiceLinePM);
            return APICaller.CallPut<APInvoicePM>(shipmentAPInvoice, Urls.APInvoicesController, UserTenant.Token);
        }

        private APInvoicePM AddAPInvoiceLine(APInvoicePM shipmentAPInvoice, APInvoiceLinePM APInvoiceLinePM)
        {
            APInvoiceLinePM = new APInvoiceLineBuilder().WithModel(APInvoiceLinePM)
                .EntityId(ShipmentContext.DirectShipment.Id)
                .Build();

            return new APInvoiceBuilder().WithModel(shipmentAPInvoice)
                .InvoiceLines(APInvoiceLinePM)
                .ShipmentsNumbers(ShipmentContext.DirectShipment.ShipmentNumber)
                .Build();
        }

        private APInvoicePM AddNewAPInvoiceLine(APInvoicePM shipmentAPInvoice, APInvoiceLinePM APInvoiceLinePM)
        {
            double totalAmount = (double)(100 + SecondAPInvoiceLinePM.InvoiceCurrencyAmount);

            APInvoiceLinePM = new APInvoiceLineBuilder().WithModel(APInvoiceLinePM)
                .EntityId(ShipmentContext.DirectShipment.Id)
                .Build();

            return new APInvoiceBuilder().WithModel(shipmentAPInvoice)
                .InvoiceLines(APInvoiceLinePM)
                .AmountDue(totalAmount)
                .AmountInInvoiceCurrency(totalAmount)
                .InvoiceExpectedAmount(totalAmount)
                .AmountInLocalCurrency(totalAmount)
                .AmountInProfitCurrency(totalAmount)
                .SubTotalInInvoiceCurrency(totalAmount)
                .SubTotalInLocalCurrency(totalAmount)
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
