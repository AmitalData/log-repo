using FluentAssertions;
using Logitude.InvoiceTests.Models.Invoice;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

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
            Context.FirstUserPMData.Id = firstUserAPInvociesList?.FirstOrDefault()?.Id;
        }

        [When(@"The Second user gets the AP Invoice that was requested by the first user")]
        public void WhenSecondUserGetTheAPInvoiceThatRequestedByFirstUser()
        {
            IEnumerable<APInvoicePM> firstUserAPInvoicesList = GetAPInvoiceListForFirstUser();
            string singleAPPaymentUrl = "apinvoices/getsingle?id=" + firstUserAPInvoicesList?.FirstOrDefault()?.Id;
            var response = APICaller.CallGet<APInvoicePM>(singleAPPaymentUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response?.Data.Id;
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

        private IEnumerable<APInvoicePM> GetAPInvoiceListForFirstUser()
        {
            //this method is waiting the CallGetByFilter to be implemented by Abd.M
            //string APInvoicesListUrl = "apinvoiceviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=1";
            //IEnumerable<APInvoicePM> APInvoicePMs = APICaller.CallGet<IEnumerable<APInvoicePM>>(APInvoicesListUrl, Context.FirstUser.Token, "Result");
            //return APInvoicePMs;
            return null;
        }
    }
}
