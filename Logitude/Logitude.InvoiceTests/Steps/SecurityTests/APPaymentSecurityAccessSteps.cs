using FluentAssertions;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Models.Shared;

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

        #region Get AP Payment from user's tenant 
        [When(@"get a AP Payment from user's AP Payment list")]
        public void WhenFirstUserGetTheFirstAPPaymentFromAPPaymentsList()
        {
            APPaymentPM firstUserAPPayments = GetAPPaymentFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserAPPayments?.Id;
        }

        [Then(@"the AP Payment should exist")]
        public void ThenAPPaymentForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        #endregion

        #region Get AP Payment from other tenant  
        [When(@"get a AP Payment from Other Tenant")]
        public void WhenSecondUserGetTheAPPaymentThatRequestedByFirstUser()
        {
            ApiResponse<APPaymentPM> response = GetAPPaymentForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the AP Payment should not exist")]
        public void ThenAPPaymentForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

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
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues().Build();

            ApiResponse<IEnumerable<APPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<APPaymentPM>>(Urls.APPaymentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion

    }
}