using Logitude.ShipmentTests.Constants;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
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
            ShipmentPM MasterShipment = new ShipmentBuilder().MasterShipment().Build();
            ShipmentContext.MasterShipment = CreateAndGetShipment(MasterShipment);
        }

        [Given(@"A house shipment")]
        public void GivenAHouseShipment()
        {
            ShipmentPM HouseShipment = new ShipmentBuilder().HouseShipment()
                                                            .MasterShipmentDataId(ShipmentContext.MasterShipment.Id)
                                                            .Build();

            ShipmentContext.HouseShipment = CreateAndGetShipment(HouseShipment);
        }

        private ShipmentPM CreateAndGetShipment(ShipmentPM shipmentPM)
        {
            ShipmentPM shipment = APICaller.CallPost<ShipmentPM>(shipmentPM, URLs.Shipment, UserTenant.Token);

            string singleShipmentUrl = URLs.ShipmentGetSingle + shipment?.Id;
            return APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserTenant.Token, null);
        }
    }
}
