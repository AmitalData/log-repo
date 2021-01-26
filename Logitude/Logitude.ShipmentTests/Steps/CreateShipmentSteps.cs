using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class CreateShipmentSteps
    {
        protected readonly ShipmentContext ShipmentContext;
        protected ApiResponse<ShipmentPM> Response;

        public CreateShipmentSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        [Given(@"A master shipment fields")]
        public void GivenAMasterShipmentFields(Table table)
        {
            ShipmentContext.MasterShipment = CreateShipmentInstance(table);
        }

        [When(@"Create master shipment API request sent")]
        public void WhenCreateMasterShipmentAPIRequestSent()
        {
            var Response = APICaller.CallPost<ShipmentPM>(ShipmentContext.MasterShipment, Urls.ShipmentController, UserTenant.Token);
            ShipmentContext.MasterShipment = Response?.Data;
        }

        [Then(@"A new master created successfully")]
        public void ThenANewMasterCreatedSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }

        [Given(@"A house shipment fields")]
        public void GivenAHouseShipmentFields(Table table)
        {
            ShipmentContext.HouseShipment = CreateShipmentInstance(table);
        }

        [When(@"Create house shipment API request sent")]
        public void WhenCreateHouseShipmentAPIRequestSent()
        {
            ShipmentContext.HouseShipment = new ShipmentBuilder().WithModel(ShipmentContext.HouseShipment)
                                                     .MasterShipmentDataId(ShipmentContext.MasterShipment.Id)
                                                     .Build();
            var response = APICaller.CallPost<ShipmentPM>(ShipmentContext.HouseShipment, Urls.ShipmentController, UserTenant.Token);
            ShipmentContext.HouseShipment = response?.Data;
        }

        [Then(@"A new house created successfully")]
        public void ThenANewHouseCreatedSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
        }

        private ShipmentPM CreateShipmentInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .ShipmentLevelCode((string)dataTable.ShipmentLevel)
                .OtherPrepaidCollectId((string)dataTable.OtherPrepaidCollect)
                .FreightPrepaidCollectId((string)dataTable.FreightPrepaidCollect)
                .MainCarriageToPortIdByCode((string)dataTable.MainCarriageToPort)
                .MainCarriageFromPortIdByCode((string)dataTable.MainCarriageFromPort)
                .Build();
        }
    }
}