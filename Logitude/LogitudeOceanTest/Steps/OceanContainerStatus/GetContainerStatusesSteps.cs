using Logitude.OceanTest.Models;
using Logitude.OceanTest.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.OceanTest.Steps.OceanContainerStatus
{
    [Binding]
    public class GetContainerStatusesSteps
    {
        public OceanContext oceanContext;
        public CheckerService checkerService;
        public OceanInsightsStatusesServices containerStatusesServices;
        public GetContainerStatusesSteps(OceanContext oceanContext, OceanInsightsStatusesServices containerStatusesServices, CheckerService checkerService)
        {
            this.oceanContext = oceanContext;
            this.checkerService = checkerService;
            this.containerStatusesServices = containerStatusesServices;
        }
        [Given(@"Read the file ""(.*)"" Data Response")]
        public void GivenReadTheFileDataResponse(string XMLFileName)
        {
            oceanContext.ShipmentContainerSimulator = containerStatusesServices.CreateShipmentContainerSimulatorForContener(XMLFileName);
        }
        [When(@"get container status request")]
        public void WhenGetContainerStatusRequest()
        {
            oceanContext.ShipmentContainerSimulator = APICaller.CallPost<ShipmentContainerSimulator>(oceanContext.ShipmentContainerSimulator, Urls.ShipmentContainersWebServiceController, UserTenant.Token)?.Data;

        }

        [Then(@"container Statuses should be change successfully")]
        public void ThenContainerStatusesShouldBeChangeSuccessfully()
        {
            checkerService.CheckContainerStatuses(oceanContext.ShipmentContainerSimulator);
        }

    }
}
