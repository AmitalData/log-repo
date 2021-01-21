using FluentAssertions;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class GetAPPaymentSecurityAccessSteps
    {
        private SecurityAccessStepsContext<APPaymentPM> Context;
        public GetAPPaymentSecurityAccessSteps(SecurityAccessStepsContext<APPaymentPM> context)
        {
            Context = context;
        }

        [When(@"First user get the first AP Payment from AP Payments list")]
        public void WhenFirstUserGetTheFirstAPPaymentFromAPPaymentsList()
        {
            IEnumerable<APPaymentPM> firstUserAPPaymentsList = GetAPPaymentListForFirstUser();
            Context.FirstUserPMData.Id = firstUserAPPaymentsList?.FirstOrDefault()?.Id;
        }

        [When(@"Second user get the AP Payment that requested by first user")]
        public void WhenSecondUserGetTheAPPaymentThatRequestedByFirstUser()
        {
            IEnumerable<APPaymentPM> firstUserAPPaymentsList = GetAPPaymentListForFirstUser();
            string apPaymentsGetSingleUrl = Urls.APPaymentsGetSingle(firstUserAPPaymentsList?.FirstOrDefault()?.Id);
            ApiResponse<APPaymentPM> response = APICaller.CallGet<APPaymentPM>(apPaymentsGetSingleUrl, UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"AP Payment for first user should be exists")]
        public void ThenAPPaymentForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"AP Payment for second user should not be exists")]
        public void ThenAPPaymentForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private IEnumerable<APPaymentPM> GetAPPaymentListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<APPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<APPaymentPM>>(Urls.APPaymentViewsGetByFilters(), UserTenant.Token, apiQueryFilters);
            return response.Data;
        }
    }
}