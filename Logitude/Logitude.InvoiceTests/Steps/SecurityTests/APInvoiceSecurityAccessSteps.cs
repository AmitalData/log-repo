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
        [When(@"The First user gets the first AP Invoice from AP Invoices list")]
        public void WhenFirstUserGetTheFirstAPInvoiceFromAPInvoicesList()
        {
            APInvoicePM firstUserAPInvocies = GetAnAPInvoiceFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserAPInvocies?.Id;
        }

        [When(@"The Second user gets the AP Invoice that was requested by the first user")]
        public void WhenSecondUserGetTheAPInvoiceThatRequestedByFirstUser()
        {
            ApiResponse<APInvoicePM> response = GetAnAPInvoiceForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"The AP Invoice which is related to the first user tanent is existed")]
        public void ThenAPInvoiceForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"The AP Invoice that was requested by the second user isn't existed")]
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