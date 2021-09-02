using Logitude.OceanTest.Models;
using Logitude.OceanTest.Services;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.OceanTest.Steps.OceanShipmentStatus
{
    [Binding]
    public class GetShipmentContainersStatusesSteps
    {
        public OceanContext oceanContext { get; set; }
        public OceanInsightsStatusesServices oceanInsightsStatusesServices { get; set; }
        public CheckerService checkerService { get; set; }
        public GetShipmentContainersStatusesSteps(OceanContext oceanContext, OceanInsightsStatusesServices oceanInsightsStatusesServices, CheckerService checkerService)
        {
            this.oceanContext = oceanContext;
            this.oceanInsightsStatusesServices = oceanInsightsStatusesServices;
            this.checkerService = checkerService;
        }
        [Given(@"Read the file ""(.*)"" shipment data response")]
        public void GivenReadTheFileShipmentDataResponse(string XMLFileName)
        {
            oceanContext.ShipmentContainerSimulator = oceanInsightsStatusesServices.CreateShipmentContainerSimulatorForShipment(XMLFileName);
        }

        [When(@"get Shipment status request")]
        public void WhenGetShipmentStatusRequest()
        {
            oceanContext.ShipmentContainerSimulator = APICaller.CallPost<ShipmentContainerSimulator>(oceanContext.ShipmentContainerSimulator, Urls.ShipmentContainersWebServiceController, UserTenant.Token)?.Data;
        }

        [Then(@"The status of the containers in the shipment should be changed successfully")]
        public void ThenTheStatusOfTheContainersInTheShipmentShouldBeChangedSuccessfully()
        {
            checkerService.CheckContainerStatuses(oceanContext.ShipmentContainerSimulator);
        }
    }
}
