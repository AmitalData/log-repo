using FluentAssertions;
using Logitude.CommonDataTests.ExternalServices;
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

        private readonly SecurityAccessStepsContext<TenantPM> Context;
        private readonly TenantExternalServices tenantExternalServices;

        public UpdateLocalSettingsAccessSteps(SecurityAccessStepsContext<TenantPM> context)
        {
            Context = context;
            tenantExternalServices = new TenantExternalServices();
        }


        #region Update Local settings for user's tenant

        [Given(@"local settings for user's tenant")]
        public void GivenLocalSettingsForUserSTenant()
        {
            Context.FirstUserPMData = tenantExternalServices.GetTenantByToken(UserTenant.Token, UserTenant.Tenant);
        }

        [When(@"update Local settings for user's tenant")]
        public void WhenUpdateLocalSettingsForUserSTenant()
        {
            Context.FirstUserPMData = tenantExternalServices.UpdateUsersLocalSettings(UserTenant.Token, Context.FirstUserPMData);
        }

        [Then(@"Local settings should update successfully")]
        public void ThenLocalSettingsShouldUpdateSuccessfully()
        {
            Context.FirstUserPMData.Should().NotBeNull();
        }
        #endregion

        #region Update Local settings for other tenant

        [Given(@"local settings for the user's tenant")]
        public void GivenLocalSettingsForOtherTenant()
        {
            Context.FirstUserPMData = tenantExternalServices.GetTenantByToken(UserTenant.Token, UserTenant.Tenant);
        }

        [When(@"update Local settings for other tenant")]
        public void WhenUpdateLocalSettingsForOtherTenant()
        {
            Context.act = () => tenantExternalServices.UpdateUsersLocalSettings(UserOtherTenant.Token, Context.FirstUserPMData);
        }


        [Then(@"should receive error message")]
        public void ThenShouldReceiveErrorMessage()
        {
            Context.act.Should().ThrowExactly<AggregateException>()
                .And.InnerExceptions[0].Message.Should().Contain("You are not authorized to do this operation");
        }
        #endregion

    }
}
