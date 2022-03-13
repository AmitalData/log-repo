using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.Warmup.Steps
{
    [Binding]
    public class ShipmentSteps
    {
        protected readonly ShipmentContext ShipmentContext;
        protected ApiResponse<ShipmentPM> Response;

        public ShipmentSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        #region Step Region

        #region Get master shipment
        [When(@"get master shipment")]
        public void WhenGetMasterShipment()
        {
            ShipmentPM shipment = GetASingleShipment(ShipmentContext.MasterShipment.Id);
            ShipmentContext.MasterShipment.Id = shipment?.Id;
        }

        [Then(@"the master should exist")]
        public void ThenTheMasterShouldExist()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }

        #endregion

        #region Create master shipment steps
        [Given(@"a master shipment with the following properties")]
        public void GivenAMasterShipmentWithTheFollowingProperties(Table table)
        {
            ShipmentContext.MasterShipment = CreateShipmentInstance(table);
        }

        [When(@"create master shipment")]
        public void WhenCreateMasterShipment()
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(ShipmentContext.MasterShipment, Urls.ShipmentController, UserTenant.Token, 5);
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

        #region Shared Region
        [Given(@"a master shipment")]
        public void GivenAMasterShipment()
        {
            ShipmentPM MasterShipment = GetValidShipmentPM("C", null);
            ShipmentContext.MasterShipment = CreateAndGetShipment(MasterShipment);
        }

        [Given(@"a house shipment")]
        public void GivenAHouseShipment()
        {
            ShipmentPM HouseShipment = GetValidShipmentPM("H", ShipmentContext.MasterShipment.Id);
            ShipmentContext.HouseShipment = CreateAndGetShipment(HouseShipment);
        }
        #endregion

        #endregion

        #region Private Function Region
        private ApiResponse<ShipmentPM> CreateAndConnectHouseToMaster(ShipmentPM houseShipment, string masterShipmentDataId)
        {
            ShipmentContext.HouseShipment = new ShipmentBuilder().WithModel(houseShipment)
                                                     .MasterShipmentDataId(masterShipmentDataId)
                                                     .Build();
            return APICaller.CallPost<ShipmentPM>(houseShipment, Urls.ShipmentController, UserTenant.Token, 5);
        }

        private ShipmentPM CreateAndGetShipment(ShipmentPM shipmentPM)
        {
            ApiResponse<ShipmentPM> PostResponse = APICaller.CallPost<ShipmentPM>(shipmentPM, Urls.ShipmentController, UserTenant.Token, 5);
            ShipmentPM shipment = PostResponse.Data;

            string singleShipmentUrl = Urls.ShipmentGetSingle(shipment?.Id);

            ApiResponse<ShipmentPM> GetResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token, 5);
            return GetResponse.Data;
        }

        private ShipmentPM GetASingleShipment(string Id)
        {
            string shipmentGetSingleUrl = Urls.ShipmentGetSingle(Id);
            ApiResponse<ShipmentPM> GetResponse = APICaller.CallGet<ShipmentPM>(shipmentGetSingleUrl, UserTenant.Token, 5);
            return GetResponse.Data;
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

        private ShipmentPM GetValidShipmentPM(string shipmentLevel, string masterShipmentDataId)
        {
            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .ShipmentLevelCode(shipmentLevel)
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("C")
                .MainCarriageToPortIdByCode("LHR")
                .MainCarriageFromPortIdByCode("MIA")
                .MasterShipmentDataId(masterShipmentDataId)
                .Build();
        }
        #endregion
    }
}