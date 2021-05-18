using FluentAssertions;
using Logitude.ShipmentTests.Models.Accounting;
using Logitude.ShipmentTests.Models.Accounting.APInvoice;
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
    public class GETAPInvoiceSteps
    {
        protected readonly AccountingContext AccountingContext;
        private APInvoicePM Response;

        public GETAPInvoiceSteps(AccountingContext accountingContext)
        {
            AccountingContext = accountingContext;
            AccountingContext.ShipmentAPInvoice = new ShipmentAPInvoice();
        }

        [When(@"get APInvoice with APInvoiceNumber")]
        public void WhenGetAPInvoiceWithAPInvoiceNumber()
        {
            AccountingContext.ShipmentAPInvoice.APInvoicePM = GetDirectAPInvoice();
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(AccountingContext.ShipmentAPInvoice.APInvoicePM.InvoiceNumber);
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
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("VendorId")
                .Filter1Operator("equals")
                .Filter1Value(PartnersData.VendorId)
                .Build();
           
            ApiResponse<IEnumerable<APInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<APInvoicePM>>(Urls.APInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
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
