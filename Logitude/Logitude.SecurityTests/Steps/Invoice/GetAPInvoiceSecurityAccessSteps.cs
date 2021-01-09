using FluentAssertions;
using Logitude.SecurityTests.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.Invoice
{
    [Binding]
    public class GetAPInvoiceSecurityAccessSteps
    {
        protected SecurityAccessStepsContext<APInvoicePM> Context;
        public GetAPInvoiceSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<APInvoicePM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }
        [When(@"The First user gets the first AP Invoice from AP Invoices list")]
        public void WhenFirstUserGetTheFirstAPInvoiceFromAPInvoicesList()
        {
            IEnumerable<APInvoicePM> firstUserAPInvociesList = GetAPInvoiceListForFirstUser();
            Context.FirstUserPMData.Id = firstUserAPInvociesList?.FirstOrDefault()?.Id;
        }
        
        [When(@"Second user get the AP Invoice that requested by first user")]
        public void WhenSecondUserGetTheAPInvoiceThatRequestedByFirstUser()
        {
            IEnumerable<APInvoicePM> firstUserAPInvoicesList = GetAPInvoiceListForFirstUser();
            string singleAPPaymentUrl = "apinvoices/getsingle?id=" + firstUserAPInvoicesList?.FirstOrDefault()?.Id;
            APInvoicePM APPaymentPM = APICaller.CallGet<APInvoicePM>(singleAPPaymentUrl, Context.SecondUser.Token, null);
            Context.SecondUserPMData.Id = APPaymentPM?.Id;
        }
        
        [Then(@"The AP Invoice which is related to the first user tanent is existed")]
        public void ThenAPInvoiceForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        
        [Then(@"AP Invoice for second user should not be exists")]
        public void ThenAPInvoiceForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        protected IEnumerable<APInvoicePM> GetAPInvoiceListForFirstUser()
        {
            string APInvoicesListUrl = "apinvoiceviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=1";
            IEnumerable<APInvoicePM> APInvoicePMs = APICaller.CallGet<IEnumerable<APInvoicePM>>(APInvoicesListUrl, Context.FirstUser.Token, "Result");
            return APInvoicePMs;
        }
    }
}
