using FluentAssertions;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Partners;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps.AccountingTests
{
    [Binding]
    public class GETARInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        private ARInvoicePM Response;

        public GETARInvoiceSteps(AccountingContext accountingContext)
        {
            AccountingContext = accountingContext;
            AccountingContext.ShipmentARInvoice = new ShipmentARInvoice();
        }

        [When(@"get ARInvoice with ARInvoiceNumber")]
        public void WhenGetARInvoiceWithARInvoiceNumber()
        {
            AccountingContext.ShipmentARInvoice.ARInvoicePM = GetDirectARInvoice();
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(AccountingContext.ShipmentARInvoice.ARInvoicePM.InvoiceNumber);
            ApiResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(Urls.ARInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            Response = response.Data?.FirstOrDefault();
        }

        [Then(@"ARInvoice should be avaliable")]
        public void ThenARInvoiceShouldBeAvaliable()
        {
            Response.Id.Should().NotBeNull();
        }

        private ARInvoicePM GetDirectARInvoice()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("BillToId")
                .Filter1Operator("equals")
                .Filter1Value(PartnersData.CustomerId)
                .Build();

            ApiResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(Urls.ARInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }

        private ApiQueryFilters BuildApiQueryFilters(string invoiceNumber)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("InvoiceNumber")
                .Filter1Operator("equals")
                .Filter1Value(invoiceNumber)
                .Build();
        }
    }
}

