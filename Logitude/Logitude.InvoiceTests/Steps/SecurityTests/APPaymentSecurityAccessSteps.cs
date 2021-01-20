using Logitude.Test.Base.Models.Login;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Context;
using System.Collections.Generic;
using Logitude.Test.Base.Services;
using System.Linq;
using FluentAssertions;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Constants;

namespace Logitude.InvoiceTests.Steps.SecurityTests
{
    [Binding]
    public class GetAPPaymentSecurityAccessSteps
    {
        private SecurityAccessStepsContext<APPaymentPM> Context;
        public GetAPPaymentSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<APPaymentPM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
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
            string apPaymentsGetSingleUrl = URLs.APPaymentsGetSingle(firstUserAPPaymentsList?.FirstOrDefault()?.Id);
            APIResponse<APPaymentPM> response = APICaller.CallGet<APPaymentPM>(apPaymentsGetSingleUrl, Context.SecondUser.Token);
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

            APIResponse<IEnumerable<APPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<APPaymentPM>>(URLs.APPaymentViewsGetByFilters(), Context.FirstUser.Token, apiQueryFilters);
            return response.Data;
        }
    }
}