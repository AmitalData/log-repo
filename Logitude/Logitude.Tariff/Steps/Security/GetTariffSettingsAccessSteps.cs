using FluentAssertions;
using Logitude.Tariff.Models;
using Logitude.Tariff.Services;
using Logitude.Base.Context;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;


namespace Logitude.Tariff.Steps.Security
{
    [Binding]
    public class GetTariffSettingsAccessSteps
    {
        private readonly SecurityAccessStepsContext<TariffSettingPM> securityAccessStepsContext;
        private readonly TariffSettingsServices tariffSettingsServices;

        public GetTariffSettingsAccessSteps(SecurityAccessStepsContext<TariffSettingPM> securityAccessStepsContext, TariffSettingsServices tariffSettingsServices)
        {
            this.securityAccessStepsContext = securityAccessStepsContext;
            this.tariffSettingsServices = tariffSettingsServices;
        }
        #region Get tarrif settings for user's tenant

        [When(@"get tariff settings for user's tenant")]
        public void WhenGetTariffSettingsForUserSTenant()
        {
            securityAccessStepsContext.FirstUserPMData = tariffSettingsServices.GetByToken(UserTenant.Token);
        }

        [Then(@"tariff settings should be available")]
        public void ThenTariffSettingsShouldBeAvailable()
        {
            securityAccessStepsContext.FirstUserPMData.Should().NotBeNull();
        }
        #endregion

    }
}
