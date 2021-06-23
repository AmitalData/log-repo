using FluentAssertions;
using Logitude.CrossDockTests.ExternalServices;
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
        private List<WarehouseEntryPackagePM> warehouseEntryPackages;

        public UpdateCrossDocksSteps(CrossDockContext crossDockContext)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryServices = new CrossDockEntryServices();
        }

        [Given(@"a packages with the following properties")]
        public void GivenAPackageWithTheFollowingProperties(Table table)
        {
            warehouseEntryPackages = crossDockEntryServices.BuildPackages(table);
        }
        
        [Given(@"entry cross dock")]
        public void GivenEntryCrossDock()
        {
            crossDockContext.CrossDockEntry = APICaller.CallGet<CrossDockEntryPM>(Urls.CrossDockGetSingle(CrossDockData.Id), UserTenant.Token).Data;
        }
        
        [When(@"update entry cross dock")]
        public void WhenUpdateEntryCrossDock()
        {
            crossDockContext.CrossDockEntry = new CrossDockBuilder()
                .WithModel(crossDockContext.CrossDockEntry)
                .WarehouseEntryPackages(warehouseEntryPackages).Build();

            crossDockContext.CrossDockEntry.Id = APICaller.CallPut<CrossDockEntryPM>(crossDockContext.CrossDockEntry, Urls.CrossDockController, UserTenant.Token)?.Data?.Id;
        }
        
        [Then(@"the entry cross dock should update successfully")]
        public void ThenTheEntryCrossDockShouldUpdateSuccessfully()
        {
            crossDockContext.CrossDockEntry.Id.Should().NotBeNull();
        }
    }
}
