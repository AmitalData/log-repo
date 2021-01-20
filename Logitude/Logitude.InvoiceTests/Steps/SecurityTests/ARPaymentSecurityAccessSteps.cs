using FluentAssertions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Constants;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class ARPaymentSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ARPaymentPM> Context;

        public ARPaymentSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<ARPaymentPM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"Get AR Payment request sent for User's Tenant")]
        public void WhenGetARPaymentRequestSentForUserSTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser();
            Context.FirstUserPMData.Id = firstUserARPayment?.Id;
        }

        [When(@"Get AR Payment request sent for other Tenant")]
        public void WhenGetARPaymentRequestSentForOtherTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser();
            string arPaymentsGetSingleUrl = URLs.ARPaymentsGetSingle(firstUserARPayment?.Id);
            var response = APICaller.CallGet<ARPaymentPM>(arPaymentsGetSingleUrl, Context.SecondUser.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"AR Payment should be exists")]
        public void ThenARPaymentShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"AR Payment should not be exists")]
        public void ThenARPaymentShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private ARPaymentPM GetAnARPaymentForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            APIResponse<IEnumerable<ARPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<ARPaymentPM>>(URLs.ARPaymentViewsGetByFilters(), Context.FirstUser.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
    }
}