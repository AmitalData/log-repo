using FluentAssertions;
using Logitude.InvoiceTests.Models.Invoice;
using Logitude.Base.Context;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Linq;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Models.Shared;

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
        #region Get AR Invoice from user's tenant 
        [When(@"get a AR Invoice from user's AR Invoices list")]
        public void WhenGetARInvoiceRequestSentForUserSTenant()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserARInvoice?.Id;
        }

        [Then(@"the AR Invoice should exist")]
        public void ThenARInvoiceShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        #endregion

        #region Get AR Invoice from other tenant 
        [When(@"get a AR Invoice from Other Tenant")]
        public void WhenGetARInvoiceRequestSentForOtherTenant()
        {
            ApiResponse<ARInvoicePM> response = GetAnARInvoiceForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the AR Invoice should not exist")]
        public void ThenARInvoiceShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

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
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues().Build();

            ApiResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(Urls.ARInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}