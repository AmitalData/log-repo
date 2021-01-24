using FluentAssertions;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class ARPaymentSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ARPaymentPM> Context;

        public ARPaymentSecurityAccessSteps(SecurityAccessStepsContext<ARPaymentPM> context)
        {
            Context = context;
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
            GetARPaymentForTheSecondUserBaseOnFirstUserARPayments();
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

        private void GetARPaymentForTheSecondUserBaseOnFirstUserARPayments()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser();
            string arPaymentsGetSingleUrl = Urls.ARPaymentsGetSingle(firstUserARPayment?.Id);
            var response = APICaller.CallGet<ARPaymentPM>(arPaymentsGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        private ARPaymentPM GetAnARPaymentForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<ARPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<ARPaymentPM>>(Urls.ARPaymentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
    }
}