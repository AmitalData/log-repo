using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using Logitude.ShipmentOrderTests.Models;
using Logitude.ShipmentOrderTests.Services;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentOrderTests.Steps
{
    [Binding]
    public class UpdateShipmentOrderSteps
    {

        private readonly ShipmentOrderContext shipmentOrderContext;
        private readonly ShipmentOrderServices shipmentOrderServices;
        private ShipmentOrder updatedShipmentOrder;

        public UpdateShipmentOrderSteps(ShipmentOrderContext shipmentOrderContext, ShipmentOrderServices shipmentOrderServices)
        {
            this.shipmentOrderContext = shipmentOrderContext;
            this.shipmentOrderServices = shipmentOrderServices;
        }

        [Given(@"a shipment order")]
        public void GivenAShipmentOrder()
        {
            shipmentOrderContext.ShipmentOrder = APICaller.CallGet<ShipmentOrder>(Urls.ShipmentOrderSingle(ShipmentOrderData.OrderNumber), UserTenant.Token).Data;
        }

        [Given(@"following shipment order properties")]
        public void GivenFollowingShipmentOrderProperties(Table table)
        {
            shipmentOrderServices.UpdateInstance(table, shipmentOrderContext.ShipmentOrder);
        }

        [When(@"update shipment order")]
        public void WhenUpdateShipmentOrder()
        {
            updatedShipmentOrder = APICaller.CallPut<ShipmentOrder>(shipmentOrderContext.ShipmentOrder, Urls.ShipmentOrderController, UserTenant.Token)?.Data;
        }

        [Then(@"the shipment order should update successfully")]
        public void ThenTheShipmentOrderShouldUpdateSuccessfully()
        {
            shipmentOrderServices.AssertUpdate(shipmentOrderContext.ShipmentOrder, updatedShipmentOrder);
        }
    }
}
