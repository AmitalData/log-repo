using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps.AccountingTests
{
    [Binding]
    public class PUTARInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        protected readonly ShipmentContext ShipmentContext;
        private ARInvoicePM PutResponse;
        private ARInvoiceLinePM SecondARInvoiceLinePM;

        public PUTARInvoiceSteps(AccountingContext accountingContext, ShipmentContext shipmentContext)
        {
            AccountingContext = accountingContext;
            ShipmentContext = shipmentContext;
        }

        [When(@"update ARInvoice by edit invoice line with the following properties")]
        public void WhenUpdateARInvoiceByEditInvoiceLineWithTheFollowingProperties(Table table)
        {
            //CreateARInvoice(AccountingContext.ShipmentARInvoice.ARInvoicePM,AccountingContext.ShipmentARInvoice.ARInvoiceLinePM);
            //dynamic dataTable = DataTable.CreateDynamicInstance();
            //ApiResponse<ARInvoicePM> response = EditARInvoice(AccountingContext.ShipmentARInvoice.ARInvoicePM, dataTable);
            //AccountingContext.ShipmentARInvoice.ARInvoicePM.Id = response.Data?.Id;
            AccountingContext.ShipmentARInvoice.ARInvoicePM = CreateARInvoicew(AccountingContext.ShipmentARInvoice.ARInvoicePM, AccountingContext.ShipmentARInvoice.ARInvoiceLinePM);
            SecondARInvoiceLinePM = CreateARInvoiceLineInstance(table);
            ApiResponse<ARInvoicePM> response = UpdateARInvoice(AccountingContext.ShipmentARInvoice.ARInvoicePM, SecondARInvoiceLinePM);
            PutResponse = response.Data;
        }

        [Then(@"the ARInvoice should update successfully")]
        public void ThenTheARInvoiceShouldUpdateSuccessfully()
        {
            //AccountingContext.ShipmentARInvoice.ARInvoicePM.Id.Should().NotBeNull();
            PutResponse.Id.Should().NotBeNull();
        }

        private ARInvoicePM CreateARInvoicew(ARInvoicePM shipmentARInvoice, ARInvoiceLinePM firstARInvoiceLinePM)
        {
            shipmentARInvoice = AddARInvoiceLine(shipmentARInvoice, firstARInvoiceLinePM);
            ApiResponse<ARInvoicePM> response = APICaller.CallPost<ARInvoicePM>(shipmentARInvoice, Urls.ARInvoicesController, UserTenant.Token);
            return response.Data;
        }

        private ApiResponse<ARInvoicePM> UpdateARInvoice(ARInvoicePM shipmentARInvoice, ARInvoiceLinePM secondARInvoiceLinePM)
        {
            shipmentARInvoice = AddNewARInvoiceLine(shipmentARInvoice, secondARInvoiceLinePM);
            return APICaller.CallPut<ARInvoicePM>(shipmentARInvoice, Urls.ARInvoicesController, UserTenant.Token , 2);
        }


        private ARInvoicePM AddNewARInvoiceLine(ARInvoicePM shipmentAPInvoice, ARInvoiceLinePM APInvoiceLinePM)
        {
            double totalAmount = (double)(100 + SecondARInvoiceLinePM.InvoiceCurrencyAmount);

            APInvoiceLinePM = new ARInvoiceLineBuilder().WithModel(APInvoiceLinePM)
                .EntityId(ShipmentContext.DirectShipment.Id)
                .Build();

            return new ARInvoiceBuilder().WithModel(shipmentAPInvoice)
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

        private ApiResponse<ARInvoicePM> EditARInvoice(ARInvoicePM shipmentARInvoice, dynamic dataTable)
        {
            shipmentARInvoice = EditARInvoiceLine(shipmentARInvoice, dataTable);
            return APICaller.CallPut<ARInvoicePM>(shipmentARInvoice, Urls.ARInvoicesController, UserTenant.Token);
        }

        private ARInvoicePM EditARInvoiceLine(ARInvoicePM shipmentARInvoice, dynamic dataTable)
        {
            AccountingContext.ShipmentARInvoice.ARInvoiceLinePM = new ARInvoiceLineBuilder().WithModel(AccountingContext.ShipmentARInvoice.ARInvoiceLinePM)
                .Quantity((double)dataTable.Quantity)
                .Description((string)dataTable.Description)
                .ForiegnCurrencyAmount((double)dataTable.Amount)
                .InvoiceCurrencyAmount((double)dataTable.Amount)
                .LocalCurrencyAmount((double)dataTable.Amount * 3.8)
                .ProfitCurrencyAmount((double)dataTable.Amount * 3.8)
                .ChangeSetOp(ChangeSetOperation.Update)
                .Build();

            return new ARInvoiceBuilder().WithModel(shipmentARInvoice)
                .ShipmentsNumbers(ShipmentContext.DirectShipment.ShipmentNumber)
                .InvoiceLinesReplace(AccountingContext.ShipmentARInvoice.ARInvoiceLinePM)
                .AmountDue((double)dataTable.Amount)
                .AmountInInvoiceCurrency((double)dataTable.Amount)
                .AmountInLocalCurrency(((double)dataTable.Amount) * 3.8)
                .AmountInProfitCurrency(((double)dataTable.Amount) * 3.8)
                .VATNumber("zero")
                .Build();
        }

        private void CreateARInvoice(ARInvoicePM shipmentARInvoice, ARInvoiceLinePM ARInvoiceLinePM)
        {
            shipmentARInvoice = AddARInvoiceLine(shipmentARInvoice, ARInvoiceLinePM);
            AccountingContext.ShipmentARInvoice.ARInvoicePM = APICaller.CallPost<ARInvoicePM>(shipmentARInvoice, Urls.ARInvoicesController, UserTenant.Token).Data;
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
                .LocalCurrencyAmount((double)dataTable.Amount * 3.8)
                .ProfitCurrencyAmount((double)dataTable.Amount * 3.8)
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
