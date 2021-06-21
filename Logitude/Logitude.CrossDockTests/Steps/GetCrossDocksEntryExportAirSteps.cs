using FluentAssertions;
using Logitude.CrossDockTests.ExternalServices;
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
    public class GetCrossDocksEntryExportAirSteps
    {
        private readonly CrossDockContext CrossDockContext;

        public GetCrossDocksEntryExportAirSteps(CrossDockContext crossDockContext)
        {
            CrossDockContext = crossDockContext;
        }


        [When(@"get cross docks with CrossDockId")]
        public void WhenGetCrossDocksWithCrossDockId()
        {
            string quotesGetSingleUrl = Urls.CrossDockGetSingle(CrossDockData.Id);
            ApiResponse<CrossDockPM> quoteResponse = APICaller.CallGet<CrossDockPM>(quotesGetSingleUrl, UserTenant.Token);
            CrossDockContext.EntriesCrossDock = quoteResponse.Data;
        }
        
        [Then(@"cross dock should be avaliable")]
        public void ThenCrossDockShouldBeAvaliable()
        {
            CrossDockContext.EntriesCrossDock.Id.Should().NotBeNull();
        }


    }
}
