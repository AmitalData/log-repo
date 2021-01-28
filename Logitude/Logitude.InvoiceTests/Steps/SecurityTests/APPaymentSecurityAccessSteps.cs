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

        #region Step Region
        [When(@"First user get the first AP Payment from AP Payments list")]
        public void WhenFirstUserGetTheFirstAPPaymentFromAPPaymentsList()
        {
            APPaymentPM firstUserAPPayments = GetAPPaymentFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserAPPayments?.Id;
        }

        [When(@"Second user get the AP Payment that requested by first user")]
        public void WhenSecondUserGetTheAPPaymentThatRequestedByFirstUser()
        {
            ApiResponse<APPaymentPM> response = GetAPPaymentForFirstUser(UserOtherTenant.Token);
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
        #endregion

        #region Private Function Region
        private ApiResponse<APPaymentPM> GetAPPaymentForFirstUser(string Token)
        {
            APPaymentPM firstUserAPPaymentsList = GetAPPaymentFromFirstUserList();
            string apPaymentsGetSingleUrl = Urls.APPaymentsGetSingle(firstUserAPPaymentsList?.Id);
            return APICaller.CallGet<APPaymentPM>(apPaymentsGetSingleUrl, Token);
        }

        private APPaymentPM GetAPPaymentFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            ApiResponse<IEnumerable<APPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<APPaymentPM>>(Urls.APPaymentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion

    }
}