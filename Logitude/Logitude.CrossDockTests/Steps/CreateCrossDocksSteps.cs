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
        private readonly CrossDockEntryServices crossDockEntryServices;

        public CreateCrossDocksSteps(CrossDockContext crossDockContext, CrossDockEntryServices crossDockEntryServices)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryServices = crossDockEntryServices;
        }

        #region create entries cross dock
        [Given(@"an entry cross dock with the following properties")]
        public void GivenAEntriesCrossDockWithTheFollowingProperties(Table table)
        {
            crossDockContext.CrossDockEntry = crossDockEntryServices.CreateInstance(table);
        }

        [Given(@"packages details")]
        public void GivenAPackagesDetails(Table table)
        {
            crossDockContext.CrossDockEntry.WarehouseEntryPackages = crossDockEntryServices.BuildPackages(table);
        }

        [When(@"create entry cross dock")]
        public void WhenCreateCrossDock()
        {
            crossDockContext.CrossDockEntry = APICaller.CallPost<CrossDockEntryPM>(crossDockContext.CrossDockEntry, Urls.CrossDockController, UserTenant.Token)?.Data;
        }

        [Then(@"the entry cross dock should create successfully")]
        public void ThenTheCrossDockShouldCreateSuccessfully()
        {
            crossDockContext.CrossDockEntry.Id.Should().NotBeNull();
        }
        #endregion

    }
}
