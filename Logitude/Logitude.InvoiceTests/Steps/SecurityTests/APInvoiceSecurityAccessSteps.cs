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

        [When(@"The First user gets the first AP Invoice from AP Invoices list")]
        public void WhenFirstUserGetTheFirstAPInvoiceFromAPInvoicesList()
        {
            IEnumerable<APInvoicePM> firstUserAPInvociesList = GetAPInvoiceListForFirstUser();
            Context.FirstUserPMData.Id = GetAPInvoiceListForFirstUser()?.FirstOrDefault()?.Id;
        }

        [When(@"The Second user gets the AP Invoice that was requested by the first user")]
        public void WhenSecondUserGetTheAPInvoiceThatRequestedByFirstUser()
        {
            GetAPInvoiceForTheSecondUserBaseOnFirstUserAPInvoices();
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

        private void GetAPInvoiceForTheSecondUserBaseOnFirstUserAPInvoices()
        {
            IEnumerable<APInvoicePM> firstUserAPInvoicesList = GetAPInvoiceListForFirstUser();
            string apInvoicesGetSingleUrl = Urls.APInvoicesGetSingle(firstUserAPInvoicesList?.FirstOrDefault()?.Id);
            var response = APICaller.CallGet<APInvoicePM>(apInvoicesGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        private IEnumerable<APInvoicePM> GetAPInvoiceListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<APInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<APInvoicePM>>(Urls.APInvoiceViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data;
        }
    }
}