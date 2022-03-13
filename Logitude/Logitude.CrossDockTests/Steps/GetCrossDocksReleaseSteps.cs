using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using TechTalk.SpecFlow;
using System.Collections.Generic;

namespace Logitude.CrossDockTests.Steps
{
    [Binding]
    public class GetCrossDocksReleaseSteps
    {

        private readonly CrossDockContext crossDockContext;

        public GetCrossDocksReleaseSteps(CrossDockContext crossDockContext)
        {
            this.crossDockContext = crossDockContext;
        }

        [When(@"get release cross docks with CrossDockId")]
        public void WhenGetReleaseCrossDocksWithCrossDockId()
        {
            ApiResponse<CrossDockReleasePM> quoteResponse = APICaller.CallGet<CrossDockReleasePM>(Urls.CrossDockReleaseGetSingle(CrossDockData.CrossDockReleaseId), UserTenant.Token);
            crossDockContext.CrossDockRelease = quoteResponse.Data;
        }

        [Then(@"release cross dock should be avaliable")]
        public void ThenReleaseCrossDockShouldBeAvaliable()
        {
            crossDockContext.CrossDockRelease.Should().NotBeNull();
            crossDockContext.CrossDockRelease.Id.Should().NotBeNull();
        }


    }
}
