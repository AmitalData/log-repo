using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class SharedShipmentSteps
    {
        private readonly ShipmentContext ShipmentContext;

        public SharedShipmentSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        #region Step Region
        [Given(@"A master shipment")]
        public void GivenAMasterShipment()
        {
            ShipmentPM MasterShipment = GetValidShipmentPM("C", null); 
            ShipmentContext.MasterShipment = CreateAndGetShipment(MasterShipment);
        }

        [Given(@"A house shipment")]
        public void GivenAHouseShipment()
        {
            ShipmentPM HouseShipment = GetValidShipmentPM("H", ShipmentContext.MasterShipment.Id); 
            ShipmentContext.HouseShipment = CreateAndGetShipment(HouseShipment);
        }
        #endregion

        #region Private Function Region
        private ShipmentPM CreateAndGetShipment(ShipmentPM shipmentPM)
        {
            ApiResponse<ShipmentPM> PostResponse = APICaller.CallPost<ShipmentPM>(shipmentPM, Urls.ShipmentController, UserTenant.Token);
            ShipmentPM shipment = PostResponse.Data;

            string singleShipmentUrl = Urls.ShipmentGetSingle(shipment?.Id);

            ApiResponse<ShipmentPM> GetResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return GetResponse.Data;
        }
        #endregion

        #region Build Models Region
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