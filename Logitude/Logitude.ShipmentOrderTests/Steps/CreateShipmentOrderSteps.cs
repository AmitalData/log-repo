using Logitude.ShipmentOrderTests.Models;
using Logitude.ShipmentOrderTests.Services;
using System;
using TechTalk.SpecFlow;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using FluentAssertions;

namespace Logitude.ShipmentOrderTests.Steps
{
    [Binding]
    public class CreateShipmentOrderSteps
    {
        private readonly ShipmentOrderContext shipmentOrderContext;
        private readonly ShipmentOrderServices shipmentOrderServices;

        public CreateShipmentOrderSteps(ShipmentOrderContext shipmentOrderContext, ShipmentOrderServices shipmentOrderServices)
        {
            this.shipmentOrderContext = shipmentOrderContext;
            this.shipmentOrderServices = shipmentOrderServices;
        }

        [Given(@"a shipment order with the following properties")]
        public void GivenAShipmentOrderWithTheFollowingProperties(Table table)
        {
            shipmentOrderContext.ShipmentOrder = shipmentOrderServices.CreateInstance(table);
        }

        [When(@"create shipment order")]
        public void WhenCreateShipmentOrder()
        {
            shipmentOrderContext.ShipmentOrder = shipmentOrderServices.Create(shipmentOrderContext.ShipmentOrder);
        }

        [Then(@"the shipment order should create successfully")]
        public void ThenTheShipmentOrderShouldCreateSuccessfully()
        {
            shipmentOrderContext.ShipmentOrder.Should().NotBeNull();
            shipmentOrderContext.ShipmentOrder.Id.Should().NotBeNull();
        }
    }
}
