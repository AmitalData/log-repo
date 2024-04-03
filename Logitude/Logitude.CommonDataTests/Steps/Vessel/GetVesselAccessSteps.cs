using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
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
            commonContext.Vessel.Id.Should().NotBeNull();
        }
        #endregion


        #region Get vessel for other tenant
        [When(@"get vessel for other tenant")]
        public void WhenGetVesselForOtherTenant()
        {
            commonContext.Vessel = APICaller.CallGet<VesselPM>(Urls.VesselGetSingle(CommonData.VesselId), UserOtherTenant.Token)?.Data;
        }

        [Then(@"vessel should not available")]
        public void ThenVesselShouldNotAvailable()
        {
            commonContext.Vessel.Id.Should().BeNull();
        }
        #endregion
    }
}
