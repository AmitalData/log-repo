using FluentAssertions;
using Logitude.CrossDockTests.Services;
using Logitude.CrossDockTests.Models;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.CrossDockTests.Steps
{
    [Binding]
    public class CreateCrossDocksReleaseSteps
    {
        private readonly CrossDockContext crossDockContext;
        private readonly CrossDockReleaseServices crossDockReleaseServices;

        public CreateCrossDocksReleaseSteps(CrossDockContext crossDockContext,CrossDockReleaseServices crossDockReleaseServices)
        {
            this.crossDockContext = crossDockContext;
            this.crossDockReleaseServices = crossDockReleaseServices;
        }

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
            crossDockContext.CrossDockRelease.Id.Should().NotBeNull();
        }
    }
}
