using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
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

        #region Create master shipment steps
        [Given(@"a master shipment with the following properties")]
        public void GivenAMasterShipmentWithTheFollowingProperties(Table table)
        {
            ShipmentContext.MasterShipment = CreateShipmentInstance(table);
        }

        [When(@"create master shipment")]
        public void WhenCreateMasterShipment()
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(ShipmentContext.MasterShipment, Urls.ShipmentController, UserTenant.Token);
            ShipmentContext.MasterShipment = response?.Data;
        }

        [Then(@"the master should create successfully")]
        public void ThenTheMasterShouldCreateSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }
        #endregion

        #region Create house shipment steps
        [Given(@"a house shipment with the following properties")]
        public void GivenAHouseShipmentWithTheFollowingProperties(Table table)
        {
            ShipmentContext.HouseShipment = CreateShipmentInstance(table);
        }

        [When(@"create house shipment")]
        public void WhenCreateHouseShipment()
        {
            ApiResponse<ShipmentPM> response = CreateAndConnectHouseToMaster(ShipmentContext.HouseShipment, ShipmentContext.MasterShipment.Id);
            ShipmentContext.HouseShipment = response?.Data;
        }

        [Then(@"the house should create successfully")]
        public void ThenTheHouseShouldCreateSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
        }

        #endregion

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