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
    public class UpdateCrossDocksEntrySteps
    {
        private readonly CrossDockContext crossDockContext;
        private readonly CrossDockEntryServices crossDockEntryServices;

        public UpdateCrossDocksEntrySteps(CrossDockContext crossDockContext, CrossDockEntryServices crossDockEntryServices)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryServices = crossDockEntryServices;
        }

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
    }
}
