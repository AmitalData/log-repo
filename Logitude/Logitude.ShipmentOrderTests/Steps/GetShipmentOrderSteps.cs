using FluentAssertions;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using Logitude.ShipmentOrderTests.Models;
using System;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentOrderTests.Steps
{
    [Binding]
    public class GetShipmentOrderSteps
    {
        private readonly ShipmentOrderContext shipmentOrderContext;

        public GetShipmentOrderSteps(ShipmentOrderContext shipmentOrderContext)
        {
            this.shipmentOrderContext = shipmentOrderContext;
        }

        [When(@"get shipment order with OrderNumber")]
        public void WhenGetShipmentOrderWithOrderNumber()
        {
            shipmentOrderContext.ShipmentOrder = APICaller.CallGet<ShipmentOrder>(Urls.ShipmentOrderSingle(ShipmentOrderData.OrderNumber), UserTenant.Token).Data;
        }

        [Then(@"shipment order should be avaliable")]
        public void ThenShipmentOrderShouldBeAvaliable()
        {
            shipmentOrderContext.ShipmentOrder.Should().NotBeNull();
            shipmentOrderContext.ShipmentOrder.Id.Should().NotBeNull();
        }
    }
}
