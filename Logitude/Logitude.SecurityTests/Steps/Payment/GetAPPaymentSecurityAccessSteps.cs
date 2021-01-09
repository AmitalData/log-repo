using Logitude.SecurityTests.Models;
using Logitude.Test.Base.Models.Login;
using TechTalk.SpecFlow;
using System;
using System.Collections.Generic;
using Logitude.Test.Base.Services;
using System.Linq;
using FluentAssertions;

namespace Logitude.SecurityTests.Steps.Payment
{
    [Binding]
    public class GetAPPaymentSecurityAccessSteps
    {
        protected SecurityAccessStepsContext<APPaymentPM> Context;
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
            string singleAPPaymentUrl = "appayments/getsingle?id=" + firstUserAPPaymentsList?.FirstOrDefault()?.Id;
            APPaymentPM APPaymentPM = APICaller.CallGet<APPaymentPM>(singleAPPaymentUrl, Context.SecondUser.Token, null);
            Context.SecondUserPMData.Id = APPaymentPM?.Id;
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

        protected IEnumerable<APPaymentPM> GetAPPaymentListForFirstUser()
        {
            string APPaymentsListUrl = "appaymentviews/getbyfilters?ForceCacheRefresh=false&GetAll=false&Filter1Name=SearchFields&Filter1Operator=Contains&Filter1Value=&PageIndex=0&PageSize=1";
            IEnumerable<APPaymentPM> APPaymentPMs = APICaller.CallGet<IEnumerable<APPaymentPM>>(APPaymentsListUrl, Context.FirstUser.Token, "Result");
            return APPaymentPMs;
        }
    }
}
