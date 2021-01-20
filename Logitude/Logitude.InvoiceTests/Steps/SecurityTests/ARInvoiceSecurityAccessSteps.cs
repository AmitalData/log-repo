using FluentAssertions;
using Logitude.InvoiceTests.Models.Invoice;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using Logitude.Test.Base.Context;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Constants;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class ARInvoiceSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ARInvoicePM> Context;

        public ARInvoiceSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<ARInvoicePM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
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
            ARInvoicePM firstUserARInvoice = GetAnARInvoiceForFirstUser();
            string arInvoicesGetSingleUrl = URLs.ARInvoicesGetSingle(firstUserARInvoice?.Id);
            APIResponse<ARInvoicePM> response = APICaller.CallGet<ARInvoicePM>(arInvoicesGetSingleUrl, Context.SecondUser.Token);
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

        private ARInvoicePM GetAnARInvoiceForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            APIResponse<IEnumerable<ARInvoicePM>> response = APICaller.CallGetByFilters<IEnumerable<ARInvoicePM>>(URLs.ARInvoiceViewsGetByFilters(), Context.FirstUser.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
    }
}