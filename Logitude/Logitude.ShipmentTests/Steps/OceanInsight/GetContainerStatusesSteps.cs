using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Services.OceanInsight;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps.OceanInsight
{
    [Binding]
    public class GetContainerStatusesSteps
    {
        public ShipmentContext shipmentContext;
        public ContainerDetailsService containerDetailsService;
        public GetContainerStatusesSteps(ShipmentContext shipmentContext, ContainerDetailsService containerDetailsService)
        {
            this.shipmentContext = shipmentContext;
            this.containerDetailsService = containerDetailsService;
            
        }
        [Given(@"Read the file ""(.*)"" Data Response")]
        public void GivenReadTheFileDataResponse(string xMLFileName)
        {
            shipmentContext.ShipmentContainerSimulator = containerDetailsService.CreateShipmentContainerSimulator(xMLFileName);
        }
        [When(@"get container details request")]
        public void WhenGetContainerDetailsRequest()
        {
            shipmentContext.ShipmentContainerSimulator = APICaller.CallPost<ShipmentContainerSimulator>(shipmentContext.ShipmentContainerSimulator, Urls.ShipmentContainersWebServiceController, UserTenant.Token)?.Data;
        }

        [Then(@"container details should be change successfully")]
        public void ThenContainerDetailsShouldBeChangeSuccessfully()
        {
            containerDetailsService.AssertChanging(shipmentContext.ShipmentContainerSimulator);
        }

    }
}
