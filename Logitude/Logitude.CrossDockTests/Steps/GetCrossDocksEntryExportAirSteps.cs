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
    public class GetCrossDocksEntryExportAirSteps
    {
        private readonly CrossDockContext crossDockContext;

        public GetCrossDocksEntryExportAirSteps(CrossDockContext crossDockContext)
        {
            this.crossDockContext = crossDockContext;
        }

        [When(@"get cross docks with CrossDockId")]
        public void WhenGetCrossDocksWithCrossDockId()
        {
            ApiResponse<CrossDockEntryPM> quoteResponse = APICaller.CallGet<CrossDockEntryPM>(Urls.CrossDockGetSingle(CrossDockData.Id), UserTenant.Token);
            crossDockContext.CrossDockEntry = quoteResponse.Data;
        }

        [Then(@"cross dock should be avaliable")]
        public void ThenCrossDockShouldBeAvaliable()
        {
            crossDockContext.CrossDockEntry.Id.Should().NotBeNull();
        }


    }
}
