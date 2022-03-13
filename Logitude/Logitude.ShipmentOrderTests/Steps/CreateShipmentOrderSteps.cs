using Logitude.ShipmentOrderTests.Models;
using Logitude.ShipmentOrderTests.Services;
using System;
using TechTalk.SpecFlow;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using FluentAssertions;

namespace Logitude.ShipmentOrderTests.Steps
{
    [Binding]
    public class CreateShipmentOrderSteps
    {
        private readonly ShipmentOrderContext shipmentOrderContext;
        private readonly ShipmentOrderServices shipmentOrderServices;
        private string insertException;

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
            try
            {
                shipmentOrderContext.ShipmentOrder = APICaller.CallPost<ShipmentOrder>(shipmentOrderContext.ShipmentOrder, Urls.ShipmentOrderController, UserTenant.Token)?.Data;
            }
            catch (Exception e)
            {
                insertException = e.InnerException.Message;
            }
        }

        [Then(@"the shipment order should create successfully")]
        public void ThenTheShipmentOrderShouldCreateSuccessfully()
        {
            if (!string.IsNullOrEmpty(insertException))
            {
                insertException.Should().Contain("Violation of UNIQUE KEY constraint 'UQ_ShipmentOrders_Tenant_OrderNumber'");
                return;
            }
            shipmentOrderContext.ShipmentOrder.Should().NotBeNull();
            shipmentOrderContext.ShipmentOrder?.Id.Should().NotBeNull();
        }
    }
}
