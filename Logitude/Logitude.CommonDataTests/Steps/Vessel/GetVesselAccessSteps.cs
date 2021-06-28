using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.Vessel
{
    [Binding]
    public class GetVesselAccessSteps
    {
        private readonly CommonContext commonContext;

        public GetVesselAccessSteps(CommonContext commonContext)
        {
            this.commonContext = commonContext;
        }

        #region Get vessel for user's tenant
        [When(@"get vessel for user's tenant")]
        public void WhenGetVesselForUserSTenant()
        {
            commonContext.Vessel = APICaller.CallGet<VesselPM>(Urls.VesselGetSingle(CommonData.VesselId), UserTenant.Token)?.Data;
        }

        [Then(@"vessel should available")]
        public void ThenVesselShouldAvailable()
        {
            commonContext.Vessel.Should().NotBeNull();
        }
        #endregion
    }
}
