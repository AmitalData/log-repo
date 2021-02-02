using FluentAssertions;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.InvoiceTests.Models.Invoice;
using Logitude.Test.Base.Models;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class GetAPInvoiceSecurityAccessSteps
    {
        private SecurityAccessStepsContext<APInvoicePM> Context;

        public GetAPInvoiceSecurityAccessSteps(SecurityAccessStepsContext<APInvoicePM> context)
        {
            Context = context;
        }

        #region Step Region
        [When(@"get a AP Invoice from user's AP Invoices list")]
        public void WhenFirstUserGetTheFirstAPInvoiceFromAPInvoicesList()
        {
            APInvoicePM firstUserAPInvocies = GetAnAPInvoiceFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserAPInvocies?.Id;
        }

        [When(@"get a AP Invoice from Other Tenant")]
        public void WhenSecondUserGetTheAPInvoiceThatRequestedByFirstUser()
        {
            ApiResponse<APInvoicePM> response = GetAnAPInvoiceForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the AP Invoice should exist")]
        public void ThenAPInvoiceForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"the AP Invoice should not exist")]
        public void ThenAPInvoiceForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #region Private Function Region
        private ApiResponse<APInvoicePM> GetAnAPInvoiceForFirstUser(string Token)
        {
            APInvoicePM firstUserAPInvoices = GetAnAPInvoiceFromFirstUserList();
            string apInvoicesGetSingleUrl = Urls.APInvoicesGetSingle(firstUserAPInvoices?.Id);
            return APICaller.CallGet<APInvoicePM>(apInvoicesGetSingleUrl, Token);
        }

        private APInvoicePM GetAnAPInvoiceFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<APInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<APInvoicePM>>(Urls.APInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}