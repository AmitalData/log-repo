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

        [Given(@"A master shipment")]
        public void GivenAMasterShipment()
        {
            ShipmentPM MasterShipment = new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .ShipmentLevelCode("C")
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("C")
                .MainCarriageToPortIdByCode("LHR")
                .MainCarriageFromPortIdByCode("MIA")
                .Build();

            ShipmentContext.MasterShipment = CreateAndGetShipment(MasterShipment);
        }

        [Given(@"A house shipment")]
        public void GivenAHouseShipment()
        {
            ShipmentPM HouseShipment = new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .ShipmentLevelCode("H")
                .OtherPrepaidCollectId("C")
                .FreightPrepaidCollectId("P")
                .MainCarriageToPortIdByCode("LHR")
                .MainCarriageFromPortIdByCode("MIA")
                .MasterShipmentDataId(ShipmentContext.MasterShipment.Id)
                .Build();

            ShipmentContext.HouseShipment = CreateAndGetShipment(HouseShipment);
        }

        private ShipmentPM CreateAndGetShipment(ShipmentPM shipmentPM)
        {
            ApiResponse<ShipmentPM> PostResponse = APICaller.CallPost<ShipmentPM>(shipmentPM, Urls.Shipment(), UserTenant.Token);
            ShipmentPM shipment = PostResponse.Data;

            string singleShipmentUrl = Urls.ShipmentGetSingle(shipment?.Id);

            ApiResponse<ShipmentPM> GetResponse = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token);
            return GetResponse.Data;
        }
    }
}