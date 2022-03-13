using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CrossDockTests.Steps
{
    [Binding]
    public class CreateCrossDocksEntrySteps
    {
        private readonly CrossDockContext crossDockContext;
        private readonly CrossDockEntryServices crossDockEntryService;

        public CreateCrossDocksEntrySteps(CrossDockContext crossDockContext, CrossDockEntryServices crossDockEntryService)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockEntryService = crossDockEntryService;
        }

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


    }
}
