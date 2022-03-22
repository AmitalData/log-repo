using FluentAssertions;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using Logitude.ShipmentOrderTests.Models;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentOrderTests.Steps
{
    [Binding]
    public class CancelShipmentOrderSteps
    {
        private readonly ShipmentOrderContext shipmentOrderContext;

        public CancelShipmentOrderSteps(ShipmentOrderContext shipmentOrderContext)
        {
            this.shipmentOrderContext = shipmentOrderContext;
        }

        [Given(@"the shipment order number")]
        public void GivenTheShipmentOrder()
        {
            shipmentOrderContext.ShipmentOrder = new ShipmentOrder { OrderNumber = ShipmentOrderData.OrderNumber };
        }

        [When(@"cancel shipment order")]
        public void WhenCancelShipmentOrder()
        {
            shipmentOrderContext.ShipmentOrder = APICaller.CallDelete<ShipmentOrder>(Urls.ShipmentOrderSingle(shipmentOrderContext.ShipmentOrder.OrderNumber), UserTenant.Token).Data;
        }

        [Then(@"the shipment order should cancel successfully")]
        public void ThenTheShipmentOrderShouldCancelSuccessfully()
        {
            shipmentOrderContext.ShipmentOrder.Id.Should().NotBeNull();
            shipmentOrderContext.ShipmentOrder.IsCancelled.Should().BeTrue();
        }
    }
}
