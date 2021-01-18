using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class CreateShipmentSteps
    {
        protected User User;
        protected readonly ShipmentContext ShipmentContext;

        public CreateShipmentSteps(MultiUsers multiUsers, ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
           // User = multiUsers.Users[0];
        }

        [Given(@"A master shipment fields")]
        public void GivenAMasterShipmentFields(Table table)
        {
            dynamic dataTable = table.CreateDynamicInstance();

            ShipmentBuilder shipmentBuilder = new ShipmentBuilder();
            ShipmentContext.MasterShipment = shipmentBuilder.WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .ShipmentLevelCode((string)dataTable.ShipmentLevel)
                .OtherPrepaidCollectId((string)dataTable.OtherPrepaidCollectId)
                .FreightPrepaidCollectId((string)dataTable.FreightPrepaidCollectId)
                .MainCarriageToPortIdByCode((string)dataTable.MainCarriageToPort)
                .MainCarriageFromPortIdByCode((string)dataTable.MainCarriageFromPort)
                .Build();
        }

        [When(@"Create master shipment API request sent")]
        public void WhenCreateMasterShipmentAPIRequestSent()
        {
            ShipmentContext.MasterShipment = APICaller.CallPost<ShipmentPM>(ShipmentContext.MasterShipment, "Shipment", UserTenant.Token);
        }

        [Then(@"A new master created successfully")]
        public void ThenANewMasterCreatedSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }

        [Given(@"A house shipment fields")]
        public void GivenAHouseShipmentFields(Table table)
        {
            ShipmentContext.HouseShipment = table.CreateInstance<ShipmentPM>();
            ShipmentContext.HouseShipment.Tenant = User.Tenant;
            ShipmentContext.HouseShipment.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        [When(@"Create house shipment API request sent")]
        public void WhenCreateHouseShipmentAPIRequestSent()
        {
            ShipmentContext.HouseShipment.MasterShipmentDataId = ShipmentContext.MasterShipment.Id;
            ShipmentContext.HouseShipment = APICaller.CallPost<ShipmentPM>(ShipmentContext.HouseShipment, "Shipment", User.Token);
        }

        [Then(@"A new house created successfully")]
        public void ThenANewHouseCreatedSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
            ShipmentContext.HouseShipment.MasterShipmentDataId.Should().NotBeNull();
            ShipmentContext.HouseShipment.MasterShipmentNumber.Should().NotBeNull();
        }
    }
}
