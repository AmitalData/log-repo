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
        public OceanContext _oceanContext { get; set; }
        public ContainerStatusesServices _getContainerStatusesServices { get; set; }
        public GetContainerStatusesSteps(OceanContext oceanContext, ContainerStatusesServices getContainerStatusesServices)
        {
            _oceanContext = oceanContext;
            _getContainerStatusesServices = getContainerStatusesServices;
        }
        [Given(@"Response ""(.*)"" Data")]
        public void GivenResponseData(string XMLFile)
        {
            _oceanContext.ShipmentContainerSimulator = _getContainerStatusesServices.CreateShipmentContainerSimulator(XMLFile);
        }

        [When(@"get container status request")]
        public void WhenGetContainerStatusRequest()
        {
            _oceanContext.ShipmentContainerSimulator = APICaller.CallPost<ShipmentContainerSimulator>(_oceanContext.ShipmentContainerSimulator, Urls.ShipmentContainersWebServiceController, UserTenant.Token)?.Data;

        }

        [Then(@"container Statuses should be change successfully")]
        public void ThenContainerStatusesShouldBeChangeSuccessfully()
        {
            _getContainerStatusesServices.ValidateShipmentContainerSimulator(_oceanContext.ShipmentContainerSimulator);
            var TryEvreySecound = 4;
            var TineLifeInSecound = 60;
            Waiter.RunAndWait(TryEvreySecound, TineLifeInSecound,()=>_getContainerStatusesServices.CheckWorkerQuewe(_oceanContext.ShipmentContainerSimulator));

        }

    }
}
