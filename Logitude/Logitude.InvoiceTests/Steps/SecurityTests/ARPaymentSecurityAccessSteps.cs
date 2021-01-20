using FluentAssertions;
using Logitude.InvoiceTests.Models.Payment;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

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
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = firstUserARPayment.Id;
        }

        [When(@"Get AR Payment request sent for other Tenant")]
        public void WhenGetARPaymentRequestSentForOtherTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser(UserTenant.Token);
            string singleARPaymentUrl = "arPayments/GetSingle?id=" + firstUserARPayment.Id;
            var response = APICaller.CallGet<ARPaymentPM>(singleARPaymentUrl, UserOtherTenant.Token);
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

        private ARPaymentPM GetAnARPaymentForFirstUser(string Token)
        {
            //this method is waiting the CallGetByFilter to be implemented by Abd.M
            //string ARPaymentsListUrl = "arpaymentviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=22";
            //IEnumerable<ARPaymentPM> ARPaymentPMs = APICaller.CallGet<IEnumerable<ARPaymentPM>>(ARPaymentsListUrl, Token, "Result");
            //return ARPaymentPMs.FirstOrDefault();
            return null;
        }
    }
}
