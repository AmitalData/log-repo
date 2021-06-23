using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

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
            ApiResponse<CrossDockReleasePM> quoteResponse = APICaller.CallGet<CrossDockReleasePM>(Urls.CrossDockGetSingle(CrossDockReleaseData.Id), UserTenant.Token);
            crossDockContext.CrossDockRelease = quoteResponse.Data;
        }
        
        [Then(@"release cross dock should be avaliable")]
        public void ThenReleaseCrossDockShouldBeAvaliable()
        {
            crossDockContext.CrossDockRelease.Id.Should().NotBeNull();
        }
    }
}
