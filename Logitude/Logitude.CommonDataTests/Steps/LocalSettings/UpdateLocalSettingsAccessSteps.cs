using FluentAssertions;
using Logitude.CommonDataTests.Services;
using Logitude.CommonTests.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Steps.LocalSettings
{
    [Binding]
    public class UpdateLocalSettingsAccessSteps
    {

        private readonly SecurityAccessStepsContext<TenantPM> securityAccessStepsContext;
        private readonly TenantServices tenantServices;

        public UpdateLocalSettingsAccessSteps(SecurityAccessStepsContext<TenantPM> context)
        {
            securityAccessStepsContext = context;
            tenantServices = new TenantServices();
        }


        #region Update Local settings for user's tenant

        [Given(@"local settings for user's tenant")]
        public void GivenLocalSettingsForUserSTenant()
        {
            securityAccessStepsContext.FirstUserPMData = tenantServices.GetByToken(UserTenant.Token);
        }

        [When(@"update Local settings for user's tenant")]
        public void WhenUpdateLocalSettingsForUserSTenant()
        {
            securityAccessStepsContext.FirstUserPMData = tenantServices.UpdateByToken(UserTenant.Token, securityAccessStepsContext.FirstUserPMData);
        }

        [Then(@"Local settings should update successfully")]
        public void ThenLocalSettingsShouldUpdateSuccessfully()
        {
            securityAccessStepsContext.FirstUserPMData.Should().NotBeNull();
        }
        #endregion

        #region Update Local settings for other tenant

        [Given(@"local settings for the user's tenant")]
        public void GivenLocalSettingsForOtherTenant()
        {
            securityAccessStepsContext.FirstUserPMData = tenantServices.GetByToken(UserTenant.Token);
        }

        [When(@"update Local settings for other tenant")]
        public void WhenUpdateLocalSettingsForOtherTenant()
        {
            securityAccessStepsContext.act = () => tenantServices.UpdateByToken(UserOtherTenant.Token, securityAccessStepsContext.FirstUserPMData);
        }


        [Then(@"should receive error message")]
        public void ThenShouldReceiveErrorMessage()
        {
            securityAccessStepsContext.act.Should().ThrowExactly<AggregateException>()
                .And.InnerExceptions[0].Message.Should().Contain("You are not authorized to do this operation");
        }
        #endregion

    }
}
