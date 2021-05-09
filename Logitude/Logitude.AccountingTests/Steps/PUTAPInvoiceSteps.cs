using FluentAssertions;
using Logitude.AccountingTests.Models;
using Logitude.AccountingTests.Models.APInvoiceBuilders;
using Logitude.AccountingTests.Models.Builders;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.AccountingTests.Steps
{
    [Binding]
    public class PUTAPInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        private APInvoicePM PutResponse;
        private ShipmentPM DirectShipment;
        private APInvoiceLinePM FirstAPInvoiceLinePM , SecondAPInvoiceLinePM;

        public PUTAPInvoiceSteps(AccountingContext accountingContext)
        {
            AccountingContext = accountingContext;
        }

        [Given(@"an first invoice line with the following properties")]
        public void GivenAnFirstInvoiceLineWithTheFollowingProperties(Table table)
        {
            FirstAPInvoiceLinePM = CreateAPInvoiceLineInstance(table);
        }

        [Given(@"a direct export shipment")]
        public void GivenADirectExportShipment()
        {
            DirectShipment = GetValidShipmentPM("D", null);
            DirectShipment = CreateAndGetShipment(DirectShipment);
            ApiResponse<APInvoicePM> response = CreateAPInvoice(AccountingContext.ShipmentAPInvoice, FirstAPInvoiceLinePM);
            AccountingContext.ShipmentAPInvoice = response.Data ;
        }

        [When(@"update APInvoice by adding invoice line the following properties")]
        public void WhenUpdateAPInvoiceByAddingInvoiceLineTheFollowingProperties(Table table)
        {
            SecondAPInvoiceLinePM = CreateAPInvoiceLineInstance(table);
            ApiResponse<APInvoicePM> response = UpdateAPInvoice(AccountingContext.ShipmentAPInvoice, SecondAPInvoiceLinePM);
            PutResponse = response.Data;
        }

        [Then(@"the invoice should update successfully")]
        public void ThenTheInvoiceShouldUpdateSuccessfully()
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

        private ApiResponse<APInvoicePM> CreateAPInvoice(APInvoicePM shipmentAPInvoice, APInvoiceLinePM firstAPInvoiceLinePM)
        {
            shipmentAPInvoice = AddAPInvoiceLine(shipmentAPInvoice, firstAPInvoiceLinePM);
            return APICaller.CallPost<APInvoicePM>(shipmentAPInvoice, Urls.APInvoicesController, UserTenant.Token);
        }

        private ApiResponse<APInvoicePM> UpdateAPInvoice(APInvoicePM shipmentAPInvoice, APInvoiceLinePM secondAPInvoiceLinePM)
        {
            shipmentAPInvoice = AddNewAPInvoiceLine(shipmentAPInvoice, secondAPInvoiceLinePM);
            return APICaller.CallPut<APInvoicePM>(shipmentAPInvoice, Urls.APInvoicesController, UserTenant.Token);
        }

        private APInvoicePM AddAPInvoiceLine(APInvoicePM shipmentAPInvoice, APInvoiceLinePM APInvoiceLinePM)
        {
            APInvoiceLinePM = new APInvoiceLineBuilder().WithModel(APInvoiceLinePM)
                .EntityId(DirectShipment.Id)
                .Build();

            return new APInvoiceBuilder().WithModel(shipmentAPInvoice)
                .InvoiceLines(APInvoiceLinePM)
                .ShipmentsNumbers(DirectShipment.ShipmentNumber)
                .Build();
        }

        private APInvoicePM AddNewAPInvoiceLine(APInvoicePM shipmentAPInvoice, APInvoiceLinePM APInvoiceLinePM)
        {
            double totalAmount = (double)(FirstAPInvoiceLinePM.InvoiceCurrencyAmount + SecondAPInvoiceLinePM.InvoiceCurrencyAmount);

            APInvoiceLinePM = new APInvoiceLineBuilder().WithModel(APInvoiceLinePM)
                .EntityId(DirectShipment.Id)
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

        private ShipmentPM GetValidShipmentPM(string shipmentLevel, string masterShipmentDataId)
        {
            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .ShipmentLevelCode(shipmentLevel)
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("C")
                .MainCarriageToPortIdByCode("LHR")
                .MainCarriageFromPortIdByCode("MIA")
                .MasterShipmentDataId(masterShipmentDataId)
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
