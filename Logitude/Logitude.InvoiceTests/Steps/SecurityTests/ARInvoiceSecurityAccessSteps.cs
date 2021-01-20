using FluentAssertions;
using Logitude.InvoiceTests.Models.Invoice;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

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
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = firstUserARInvoice.Id;
        }

        [When(@"Get AR Invoice request sent for other Tenant")]
        public void WhenGetARInvoiceRequestSentForOtherTenant()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser(UserTenant.Token);
            string singleARInvoiceUrl = "arinvoices/GetSingle?id=" + firstUserARInvoice.Id;
            var response = APICaller.CallGet<ARInvoicePM>(singleARInvoiceUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
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

        private ARInvoicePM GetAnARInvoiceForFirstUser(string Token)
        {
            //this method is waiting the CallGetByFilter to be implemented by Abd.M
            //string ARInvoicesListUrl = "arinvoiceviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=22";
            //IEnumerable<ARInvoicePM> ARInvoicePMs = APICaller.CallGet<IEnumerable<ARInvoicePM>>(ARInvoicesListUrl, Token, "Result");
            //return ARInvoicePMs.FirstOrDefault();
            return null;
        }
    }
}
