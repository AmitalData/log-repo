using FluentAssertions;
using Logitude.InvoiceTests.Models.Invoice;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class ARInvoiceSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ARInvoicePM> Context;

        public ARInvoiceSecurityAccessSteps(SecurityAccessStepsContext<ARInvoicePM> context)
        {
            Context = context;
        }

        [When(@"Get AR Invoice request sent for User's Tenant")]
        public void WhenGetARInvoiceRequestSentForUserSTenant()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser();
            Context.FirstUserPMData.Id = firstUserARInvoice?.Id;
        }

        [When(@"Get AR Invoice request sent for other Tenant")]
        public void WhenGetARInvoiceRequestSentForOtherTenant()
        {
            GetARInvoiceForTheSecondUserBaseOnFirstUserARInvoices();
        }

        [Then(@"AR Invoice should be exists")]
        public void ThenARInvoiceShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"AR Invoice should not be exists")]
        public void ThenARInvoiceShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private void GetARInvoiceForTheSecondUserBaseOnFirstUserARInvoices()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser();
            string arInvoicesGetSingleUrl = Urls.ARInvoicesGetSingle(firstUserARInvoice?.Id);
            ApiResponse<ARInvoicePM> response = APICaller.CallGet<ARInvoicePM>(arInvoicesGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        private ARInvoicePM GetAnARInvoiceForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(Urls.ARInvoiceViewsGetByFilters(), UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
    }
}