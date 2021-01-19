using FluentAssertions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.InvoiceTests.Models.Payment;

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
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser(Context.FirstUser.Token);
            Context.FirstUserPMData.Id = firstUserARPayment.Id;
        }

        [When(@"Get AR Payment request sent for other Tenant")]
        public void WhenGetARPaymentRequestSentForOtherTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser(Context.FirstUser.Token);
            string singleARPaymentUrl = "arPayments/GetSingle?id=" + firstUserARPayment.Id;
            var response = APICaller.CallGet<ARPaymentPM>(singleARPaymentUrl, Context.SecondUser.Token);
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
