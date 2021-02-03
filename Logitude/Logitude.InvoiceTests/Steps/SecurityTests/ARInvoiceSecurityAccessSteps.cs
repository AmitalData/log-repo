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

        #region Step Region
        [When(@"get a AR Invoice from user's AR Invoices list")]
        public void WhenGetARInvoiceRequestSentForUserSTenant()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserARInvoice?.Id;
        }

        [When(@"get a AR Invoice from Other Tenant")]
        public void WhenGetARInvoiceRequestSentForOtherTenant()
        {
            ApiResponse<ARInvoicePM> response = GetAnARInvoiceForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the AR Invoice should exist")]
        public void ThenARInvoiceShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"the AR Invoice should not exist")]
        public void ThenARInvoiceShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #region Private Function Region
        private ApiResponse<ARInvoicePM> GetAnARInvoiceForFirstUser(string Token)
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceFromFirstUserList();
            string arInvoicesGetSingleUrl = Urls.ARInvoicesGetSingle(firstUserARInvoice?.Id);
            return APICaller.CallGet<ARInvoicePM>(arInvoicesGetSingleUrl, UserOtherTenant.Token);
        }

        private ARInvoicePM GetAnARInvoiceFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(Urls.ARInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}