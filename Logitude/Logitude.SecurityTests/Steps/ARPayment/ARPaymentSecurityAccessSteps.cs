using FluentAssertions;
using Logitude.SecurityTests.Models.ARPayment;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.ARPayment
{
    [Binding]
    public class ARPaymentSecurityAccessSteps
    {
        protected ARPaymenteAccessStepsContext Context;

        public ARPaymentSecurityAccessSteps(MultiUsers multiUsers, ARPaymenteAccessStepsContext context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"Get AR Payment request sent for User's Tenant")]
        public void WhenGetARPaymentRequestSentForUserSTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser(Context.FirstUser.Token);
            Context.FirstUserARPayment.Id = firstUserARPayment.Id;
        }
        
        [When(@"Get AR Payment request sent for other Tenant")]
        public void WhenGetARPaymentRequestSentForOtherTenant()
        {
            ARPaymentPM firstUserARPayment = GetAnARPaymentForFirstUser(Context.FirstUser.Token);
            string singleARPaymentUrl = "arPayments/GetSingle?id=" + firstUserARPayment.Id;
            ARPaymentPM ARPaymentPM = APICaller.CallGet<ARPaymentPM>(singleARPaymentUrl, Context.SecondUser.Token, null);
            Context.SecondUserARPayment.Id = ARPaymentPM?.Id;
        }
        
        [Then(@"AR Payment should be exists")]
        public void ThenARPaymentShouldBeExists()
        {
            Context.FirstUserARPayment.Id.Should().NotBeNull();
        }

        [Then(@"AR Payment should not be exists")]
        public void ThenARPaymentShouldNotBeExists()
        {
            Context.SecondUserARPayment.Id.Should().BeNull();
        }

        protected ARPaymentPM GetAnARPaymentForFirstUser(string Token)
        {
            string ARPaymentsListUrl = "arpaymentviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=22";
            IEnumerable<ARPaymentPM> ARPaymentPMs = APICaller.CallGet<IEnumerable<ARPaymentPM>>(ARPaymentsListUrl, Token, "Result");
            return ARPaymentPMs.FirstOrDefault();
        }
    }
}
