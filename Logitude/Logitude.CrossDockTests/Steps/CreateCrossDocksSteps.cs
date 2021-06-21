using FluentAssertions;
using Logitude.CrossDockTests.ExternalServices;
using Logitude.CrossDockTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CrossDockTests.Steps
{
    [Binding]
    public class CreateCrossDocksSteps
    {
        private readonly CrossDockContext crossDockContext;
        private readonly CrossDockExternalServices crossDockExternalServices;

        public CreateCrossDocksSteps(CrossDockContext crossDockContext, CrossDockExternalServices crossDockExternalServices)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockExternalServices = crossDockExternalServices;
        }

        #region create entries cross dock
        [Given(@"a entries cross dock with the following properties")]
        public void GivenAEntriesCrossDockWithTheFollowingProperties(Table table)
        {
            crossDockContext.EntriesCrossDock = crossDockExternalServices.CreateEntriesCrossInstance(table);
        }

        [Given(@"a packages Details")]
        public void GivenAPackagesDetails(Table table)
        {
            crossDockContext.EntriesCrossDock = crossDockExternalServices.AddWarehouseEntryPackages(crossDockContext.EntriesCrossDock, table);
        }

        [When(@"create cross dock")]
        public void WhenCreateCrossDock()
        {
            ApiResponse<CrossDockPM> response = APICaller.CallPost<CrossDockPM>(crossDockContext.EntriesCrossDock, Urls.CrossDockController, UserTenant.Token);
            crossDockContext.EntriesCrossDock = response?.Data;
        }

        [Then(@"the cross dock should create successfully")]
        public void ThenTheCrossDockShouldCreateSuccessfully()
        {
            crossDockContext.EntriesCrossDock.Id.Should().NotBeNull();
        }
        #endregion

    }
}
