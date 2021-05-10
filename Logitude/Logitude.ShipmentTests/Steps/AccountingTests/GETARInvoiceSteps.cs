using FluentAssertions;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.ARInvoice;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
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
        }

        [When(@"get ARInvoice with ARInvoiceNumber")]
        public void WhenGetARInvoiceWithARInvoiceNumber()
        {
            AccountingContext.ShipmentARInvoice = GetDirectARInvoice();
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(AccountingContext.ShipmentARInvoice.InvoiceNumber);
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
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "BillToId",
                Filter1Operator = "equals",
                Filter1Value = PartnersData.CustomerId,
            };

            ApiResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(Urls.ARInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }

        private ApiQueryFilters BuildApiQueryFilters(string invoiceNumber)
        {
            return new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "InvoiceNumber",
                Filter1Operator = "equals",
                Filter1Value = invoiceNumber,
            };
        }
    }
}

