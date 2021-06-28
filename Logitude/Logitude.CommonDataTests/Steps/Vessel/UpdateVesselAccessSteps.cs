using FluentAssertions;
using Logitude.CommonDataTests.Models;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.Vessel
{
    [Binding]
    public class UpdateVesselAccessSteps
    {

        private readonly CommonContext commonContext;

        public UpdateVesselAccessSteps(CommonContext commonContext)
        {
            this.commonContext = commonContext;
        }

        #region Update Vessel for user's tenant
        [Given(@"vessel for user's tenant")]
        public void GivenVesselForUserSTenant()
        {
            commonContext.Vessel = APICaller.CallGet<VesselPM>(Urls.VesselGetSingle(CommonData.VesselId), UserTenant.Token).Data;
        }
        
        [When(@"update vessel for user's tenant")]
        public void WhenUpdateVesselForUserSTenant()
        {
            commonContext.Vessel = APICaller.CallPut<VesselPM>(commonContext.Vessel, Urls.VesselsController, UserTenant.Token).Data;
        }
        
        [Then(@"vessel should update successfully")]
        public void ThenVesselShouldUpdateSuccessfully()
        {
            commonContext.Vessel.Should().NotBeNull();
        }
        #endregion


        #region Update Vessel for other tenant
        [Given(@"vessel for the user's tenant")]
        public void GivenVesselForTheUserSTenant()
        {
            commonContext.Vessel = APICaller.CallGet<VesselPM>(Urls.VesselGetSingle(CommonData.VesselId), UserTenant.Token).Data;
        }

        [When(@"update vessel for other tenant")]
        public void WhenUpdateVesselForOtherTenant()
        {
            commonContext.act = () => APICaller.CallPut<VesselPM>(commonContext.Vessel, Urls.VesselsController, UserOtherTenant.Token);
        }

        [Then(@"update should receive error message")]
        public void ThenUpdateShouldReceiveErrorMessage()
        {
            commonContext.act.Should().ThrowExactly<AggregateException>()
                    .And.InnerExceptions[0].Message.Should().Contain("Sorry! you have no permission to do this operation on Tenant");
        }
        #endregion

    }
}
