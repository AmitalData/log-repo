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
        private WarehouseEntryPackagePM warehouseEntryPackage;

        public UpdateCrossDocksSteps(CrossDockContext crossDockContext)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryServices = new CrossDockEntryServices();
        }

        [Given(@"a package with the following properties")]
        public void GivenAPackageWithTheFollowingProperties(Table table)
        {
            warehouseEntryPackage = crossDockEntryServices.CreatePackage(table);
        }
        
        [Given(@"entry cross dock")]
        public void GivenEntryCrossDock()
        {
            crossDockContext.EntriesCrossDock = APICaller.CallGet<CrossDockPM>(Urls.CrossDockGetSingle(CrossDockData.Id), UserTenant.Token).Data;
        }
        
        [When(@"update entry cross dock")]
        public void WhenUpdateEntryCrossDock()
        {
            crossDockContext.EntriesCrossDock = new CrossDockBuilder()
                .WithModel(crossDockContext.EntriesCrossDock)
                .WarehouseEntryPackages(warehouseEntryPackage).Build();

            crossDockContext.EntriesCrossDock.Id = APICaller.CallPut<CrossDockPM>(crossDockContext.EntriesCrossDock, Urls.CrossDockController, UserTenant.Token)?.Data?.Id;
        }
        
        [Then(@"the entry cross dock should update successfully")]
        public void ThenTheEntryCrossDockShouldUpdateSuccessfully()
        {
            crossDockContext.EntriesCrossDock.Id.Should().NotBeNull();
        }
    }
}
