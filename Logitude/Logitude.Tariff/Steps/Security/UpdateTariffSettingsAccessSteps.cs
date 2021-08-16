using FluentAssertions;
using Logitude.Tariff.Services;
using Logitude.Tariff.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.Tariff.Steps.Security
{
    [Binding]
    public class UpdateTariffSettingsAccessSteps
    {
        private readonly SecurityAccessStepsContext<TariffSettingPM> securityAccessStepsContext;
        private readonly TariffSettingsServices tariffSettingsServices;

        public UpdateTariffSettingsAccessSteps(SecurityAccessStepsContext<TariffSettingPM> securityAccessStepsContext, TariffSettingsServices tariffSettingsServices)
        {
            this.securityAccessStepsContext = securityAccessStepsContext;
            this.tariffSettingsServices = tariffSettingsServices;
        }

        #region Update Tariff settings for user's tenant
        [Given(@"tariff settings for user's tenant")]
        public void GivenTariffSettingsForUserSTenant()
        {
            securityAccessStepsContext.FirstUserPMData = tariffSettingsServices.GetByToken(UserTenant.Token);
        }

        [When(@"update tariff settings for user's tenant")]
        public void WhenUpdateTariffSettingsForUserSTenant()
        {
            securityAccessStepsContext.FirstUserPMData = tariffSettingsServices.UpdateByToken(UserTenant.Token, securityAccessStepsContext.FirstUserPMData);
        }

        [Then(@"tariff settings should update successfully")]
        public void ThenTariffSettingsShouldUpdateSuccessfully()
        {
            securityAccessStepsContext.FirstUserPMData.Should().NotBeNull();
        }
        #endregion

        #region Update Tariff settings for other tenant
        [Given(@"tariff settings for the user's tenant")]
        public void GivenTariffSettingsForTheUserSTenant()
        {
            securityAccessStepsContext.SecondUserPMData = tariffSettingsServices.GetByToken(UserTenant.Token);
        }

        [When(@"update tariff settings for other tenant")]
        public void WhenUpdateTariffSettingsForOtherTenant()
        {
            securityAccessStepsContext.act = () => tariffSettingsServices.UpdateByToken(UserOtherTenant.Token, securityAccessStepsContext.SecondUserPMData);
        }

        [Then(@"should receive error message")]
        public void ThenShouldReceiveErrorMessage()
        {
            securityAccessStepsContext.act.Should().ThrowExactly<AggregateException>()
                .And.InnerExceptions[0].Message.Should().Contain("you have no permission to do this operation");
        }
        #endregion
    }
}
