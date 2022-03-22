using FluentAssertions;
using Logitude.CRMTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CRMTests.Steps.Activities
{
    [Binding]
    public class GetPhoneCallSteps
    {
        private readonly CRMContext crmContext;

        public GetPhoneCallSteps(CRMContext crmContext)
        {
            this.crmContext = crmContext;
        }

        [When(@"get phone call with PhoneCallId")]
        public void WhenGetPhoneCallWithPhoneCallId()
        {
            crmContext.ActiviyPhoneCall = APICaller.CallGet<ActivityPM>(Urls.ActivitySingle(CRMData.ActivityPhoneCallId), UserTenant.Token).Data;
        }

        [Then(@"phone call should be avaliable")]
        public void ThenPhoneCallShouldBeAvaliable()
        {
            crmContext.ActiviyPhoneCall.Should().NotBeNull();
            crmContext.ActiviyPhoneCall.Id.Should().NotBeNull();
        }
    }
}
