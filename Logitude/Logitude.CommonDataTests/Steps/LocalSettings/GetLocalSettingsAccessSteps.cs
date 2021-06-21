using FluentAssertions;
using Logitude.CommonDataTests.ExternalServices;
using Logitude.CommonTests.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;


namespace Logitude.CommonDataTests.Steps.LocalSettings
{
    [Binding]
    public class GetLocalSettingsAccessSteps
    {
        private SecurityAccessStepsContext<TenantPM> Context;
        private TenantExternalServices tenantDataService;

        public GetLocalSettingsAccessSteps(SecurityAccessStepsContext<TenantPM> context)
        {
            Context = context;
            tenantDataService = new TenantExternalServices();
        }

        #region Get local settings for user's tenant
        [When(@"get local settings for user's tenant")]
        public void WhenGetLocalSettingsForUserSTenant()
        {
            Context.FirstUserPMData = tenantDataService.GetUserTenantByToken(UserTenant.Token);
        }

        [Then(@"local settings should available")]
        public void ThenLocalSettingsShouldAvailable()
        {
            Context.FirstUserPMData.Should().NotBeNull();
        }
        #endregion



        #region Get local settings for other tenant
        [When(@"get local settings for other tenant")]
        public void WhenGetLocalSettingsForOtherTenant()
        {
            Context.act = () => tenantDataService.GetUserTenantByToken(UserOtherTenant.Token);
        }


        [Then(@"local settings should not available")]
        public void ThenLocalSettingsShouldNotAvailable()
        {
            Context.act.Should().ThrowExactly<AggregateException>()
                         .And.InnerExceptions[0].Message.Should().Contain("Sorry you’re not authenticated to view company info");

        }
        #endregion


    }
}
