using FluentAssertions;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Base.Context;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using System.Collections.Generic;
using System.Linq;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Models.Shared;

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

        #region Step Region

        #region Get AR Payment from user's tenant
        [When(@"get a AR Payment from user's AR Payment list")]
        public void WhenGetARPaymentRequestSentForUserSTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentFromFirstUserList();
            Context.FirstUserPMData.Id = firstUserARPayment?.Id;
        }

        [Then(@"the AR Payment should exist")]
        public void ThenARPaymentShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        #endregion

        #region Get AR Payment from other tenant 
        [When(@"get a AR Payment from Other Tenant")]
        public void WhenGetARPaymentRequestSentForOtherTenant()
        {
            ApiResponse<ARPaymentPM> response = GetAnARPaymentForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the AR Payment should not exist")]
        public void ThenARPaymentShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #endregion

        #region Private Function Region
        private ApiResponse<ARPaymentPM> GetAnARPaymentForFirstUser(string Token)
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentFromFirstUserList();
            string arPaymentsGetSingleUrl = Urls.ARPaymentsGetSingle(firstUserARPayment?.Id);
            return APICaller.CallGet<ARPaymentPM>(arPaymentsGetSingleUrl, Token);
        }

        private ARPaymentPM GetAnARPaymentFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues().Build();

            ApiResponse<IEnumerable<ARPaymentPM>> response = APICaller.CallGetByFilters<IEnumerable<ARPaymentPM>>(Urls.ARPaymentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}