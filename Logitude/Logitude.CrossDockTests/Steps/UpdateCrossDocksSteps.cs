using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Logitude.CrossDockTests.Steps
{
    [Binding]
    public class UpdateCrossDocksSteps
    {

        private readonly CrossDockContext crossDockContext;
        private readonly CrossDockEntryServices crossDockEntryServices;
        private readonly CrossDockReleaseServices crossDockReleaseServices;

        public UpdateCrossDocksSteps(CrossDockContext crossDockContext, CrossDockEntryServices crossDockEntryServices, CrossDockReleaseServices crossDockReleaseServices)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryServices = crossDockEntryServices;
            this.crossDockReleaseServices = crossDockReleaseServices;
        }

        #region update entries cross dock
        [Given(@"a packages with the following properties")]
        public void GivenAPackageWithTheFollowingProperties(Table table)
        {
            crossDockContext.WarehouseEntryPackages = crossDockEntryServices.BuildPackages(table);
        }

        [Given(@"entry cross dock")]
        public void GivenEntryCrossDock()
        {
            crossDockContext.CrossDockEntry = APICaller.CallGet<CrossDockEntryPM>(Urls.CrossDockGetSingle(CrossDockData.Id), UserTenant.Token).Data;
        }

        [When(@"update entry cross dock")]
        public void WhenUpdateEntryCrossDock()
        {
            crossDockContext.CrossDockEntry = new CrossDockEntryBuilder()
                .WithModel(crossDockContext.CrossDockEntry)
                .WarehouseEntryPackages(crossDockContext.WarehouseEntryPackages).Build();

            crossDockContext.CrossDockEntry.Id = APICaller.CallPut<CrossDockEntryPM>(crossDockContext.CrossDockEntry, Urls.CrossDockController, UserTenant.Token)?.Data?.Id;
        }

        [Then(@"the entry cross dock should update successfully")]
        public void ThenTheEntryCrossDockShouldUpdateSuccessfully()
        {
            crossDockContext.CrossDockEntry.Id.Should().NotBeNull();
        }
        #endregion





        #region update release cross dock
        [Given(@"CustomerRef1 '(.*)' and House '(.*)'")]
        public void GivenFollowingProperties(string customerRef1, string house)
        {
            crossDockContext.House = house;
            crossDockContext.CustomerRef1 = customerRef1;
        }

        [Given(@"release cross dock")]
        public void GivenReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease = APICaller.CallGet<CrossDockReleasePM>(Urls.CrossDockReleaseGetSingle(CrossDockReleaseData.Id), UserTenant.Token).Data;
        }

        [When(@"update release cross dock")]
        public void WhenUpdateReleaseCrossDock()
        {
            crossDockContext.CrossDockRelease = new CrossDockReleaseBuilder()
            .WithModel(crossDockContext.CrossDockRelease)
            .HouseNumber(crossDockContext.House)
            .CustomerRef1(crossDockContext.CustomerRef1).Build();

            crossDockContext.CrossDockRelease.Id = APICaller.CallPut<CrossDockReleasePM>(crossDockContext.CrossDockRelease, Urls.CrossReleaseGetController, UserTenant.Token)?.Data?.Id;
        }

        [Then(@"the release cross dock should update successfully")]
        public void ThenTheReleaseCrossDockShouldUpdateSuccessfully()
        {
            crossDockContext.CrossDockRelease.Id.Should().NotBeNull();
        }
        #endregion



    }
}
