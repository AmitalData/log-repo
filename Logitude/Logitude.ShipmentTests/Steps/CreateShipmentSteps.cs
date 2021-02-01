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

        #region Step Region
        [Given(@"A master shipment fields")]
        public void GivenAMasterShipmentFields(Table table)
        {
            ShipmentContext.MasterShipment = CreateShipmentInstance(table);
        }

        [When(@"Create master shipment API request sent")]
        public void WhenCreateMasterShipmentAPIRequestSent()
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(ShipmentContext.MasterShipment, Urls.ShipmentController, UserTenant.Token);
            ShipmentContext.MasterShipment = response?.Data;
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
            ApiResponse<ShipmentPM> response = CreateAndConnectHouseToMaster(ShipmentContext.HouseShipment, ShipmentContext.MasterShipment.Id);
            ShipmentContext.HouseShipment = response?.Data;
        }

        [Then(@"A new house created successfully")]
        public void ThenANewHouseCreatedSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
        }
        #endregion

        #region Private Function Region
        private ApiResponse<ShipmentPM> CreateAndConnectHouseToMaster(ShipmentPM houseShipment, string masterShipmentDataId)
        {
            ShipmentContext.HouseShipment = new ShipmentBuilder().WithModel(houseShipment)
                                                     .MasterShipmentDataId(masterShipmentDataId)
                                                     .Build();
            return APICaller.CallPost<ShipmentPM>(houseShipment, Urls.ShipmentController, UserTenant.Token);
        }
        #endregion

        #region Build Models Region
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
        #endregion
    }
}