using FluentAssertions;
using Logitude.AccountingTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
namespace Logitude.AccountingTests.Steps
{
    [Binding]
    public class GETAPInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        private APInvoicePM Response;

        public GETAPInvoiceSteps(AccountingContext accountingContext)
        {
            AccountingContext = accountingContext;
        }

        [When(@"get APInvoice with APInvoiceNumber")]
        public void WhenGetAPInvoiceWithAPInvoiceNumber()
        {
            AccountingContext.ShipmentAPInvoice = GetDirectAPInvoice();
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(AccountingContext.ShipmentAPInvoice.InvoiceNumber);
            ApiResponse<IEnumerable<APInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<APInvoicePM>>(Urls.APInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            Response = response.Data?.FirstOrDefault();
        }
        
        [Then(@"APInvoice should be avaliable")]
        public void ThenAPInvoiceShouldBeAvaliable()
        {
            Response.Id.Should().NotBeNull();
        }

        private APInvoicePM GetDirectAPInvoice()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1,
                Filter1Name = "VendorId",
                Filter1Operator = "equals",
                Filter1Value = PartnersData.VendorId,
            };

            ApiResponse<IEnumerable<APInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<APInvoicePM>>(Urls.APInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
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
