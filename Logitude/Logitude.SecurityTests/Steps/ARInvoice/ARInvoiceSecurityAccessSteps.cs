using FluentAssertions;
using Logitude.SecurityTests.Models.ARInvoice;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.ARInvoice
{
    [Binding]
    public class ARInvoiceSecurityAccessSteps
    {
        protected ARInvoiceAccessStepsContext Context;

        public ARInvoiceSecurityAccessSteps(MultiUsers multiUsers, ARInvoiceAccessStepsContext context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"Get AR Invoice request sent for User's Tenant")]
        public void WhenGetARInvoiceRequestSentForUserSTenant()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser(Context.FirstUser.Token);
            Context.FirstUserARInvoice.Id = firstUserARInvoice.Id;
        }
        
        [When(@"Get AR Invoice request sent for other Tenant")]
        public void WhenGetARInvoiceRequestSentForOtherTenant()
        {
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser(Context.FirstUser.Token);
            string singleARInvoiceUrl = "arinvoices/GetSingle?id=" + firstUserARInvoice.Id;
            ARInvoicePM ARInvoicePM = APICaller.CallGet<ARInvoicePM>(singleARInvoiceUrl, Context.SecondUser.Token, null);
            Context.SecondUserARInvoice.Id = ARInvoicePM?.Id;
        }
        
        [Then(@"AR Invoice should be exists")]
        public void ThenARInvoiceShouldBeExists()
        {
            Context.FirstUserARInvoice.Id.Should().NotBeNull();
        }
        
        [Then(@"AR Invoice should not be exists")]
        public void ThenARInvoiceShouldNotBeExists()
        {
            Context.SecondUserARInvoice.Id.Should().BeNull();
        }

        protected ARInvoicePM GetAnARInvoiceForFirstUser(string Token)
        {
            string ARInvoicesListUrl = "arinvoiceviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=22";
            IEnumerable<ARInvoicePM> ARInvoicePMs = APICaller.CallGet<IEnumerable<ARInvoicePM>>(ARInvoicesListUrl, Token, "Result");
            return ARInvoicePMs.FirstOrDefault();
        }
    }
}
