using FluentAssertions;
using Logitude.CrossDockTests.Services;
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
        private readonly CrossDockEntryServices crossDockEntryService;
        private readonly CrossDockReleaseServices crossDockReleaseServices;

        public CreateCrossDocksSteps(CrossDockContext crossDockContext)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryService = new CrossDockEntryServices();
            this.crossDockReleaseServices = new CrossDockReleaseServices();
        }



        #region create entries cross dock
        [Given(@"an entry cross dock with the following properties")]
        public void GivenAEntriesCrossDockWithTheFollowingProperties(Table table)
        {
            crossDockContext.CrossDockEntry = crossDockEntryService.CreateInstance(table);
        }

        [Given(@"packages details")]
        public void GivenAPackagesDetails(Table table)
        {
            crossDockContext.CrossDockEntry.WarehouseEntryPackages = crossDockEntryService.BuildPackages(table);
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

        #region create release cross dock
        [Given(@"a release cross dock with the following properties")]
        public void GivenAnReleaseCrossDockWithTheFollowingProperties(Table table)
        {
            crossDockContext.CrossDockRelease = crossDockReleaseServices.CreateInstance(table);
        }

        [Given(@"an entry cross dock")]
        public void GivenAnEntryCrossDock()
        {
            crossDockContext.CrossDockRelease.WarehouseReleasePackages = crossDockReleaseServices.BuildPackages();
        }

        [When(@"create release cross dock")]
        public void WhenCreateReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease = APICaller.CallPost<CrossDockReleasePM>(crossDockContext.CrossDockRelease, Urls.CrossReleaseController, UserTenant.Token)?.Data;
        }

        [Then(@"the release cross dock should create successfully")]
        public void ThenTheReleaseCrossDockShouldCreateSuccessfully()
        {
            crossDockContext.CrossDockRelease?.Id.Should().NotBeNull();
        }
        #endregion

    }
}
