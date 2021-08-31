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
        public OceanContext _oceanContext { get; set; }
        public OceanInsightsStatusesServices _oceanInsightsStatusesServices { get; set; }
        public GetShipmentContainersStatusesSteps(OceanContext oceanContext, OceanInsightsStatusesServices oceanInsightsStatusesServices)
        {
            _oceanContext = oceanContext;
            _oceanInsightsStatusesServices = oceanInsightsStatusesServices;
        }
        [Given(@"Read the file ""(.*)"" shipment data response")]
        public void GivenReadTheFileShipmentDataResponse(string XMLFileName)
        {
            _oceanContext.ShipmentContainerSimulator = _oceanInsightsStatusesServices.CreateShipmentContainerSimulatorForShipment(XMLFileName);
        }

        [When(@"get Shipment status request")]
        public void WhenGetShipmentStatusRequest()
        {
            _oceanContext.ShipmentContainerSimulator = APICaller.CallPost<ShipmentContainerSimulator>(_oceanContext.ShipmentContainerSimulator, Urls.ShipmentContainersWebServiceController, UserTenant.Token)?.Data;
        }

        [Then(@"The status of the containers in the shipment should be changed successfully")]
        public void ThenTheStatusOfTheContainersInTheShipmentShouldBeChangedSuccessfully()
        {
            _oceanInsightsStatusesServices.CheckContainerStatuses(_oceanContext.ShipmentContainerSimulator);
        }
    }
}
