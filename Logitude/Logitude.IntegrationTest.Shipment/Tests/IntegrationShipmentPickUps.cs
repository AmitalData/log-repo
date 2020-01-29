using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
   public class IntegrationShipmentPickUps
    {
       
        public static List<ShipmentPickUpPM> ShipmentPickUps()
        {
            List<ShipmentPickUpPM> shipmentPickUpPM = new List<ShipmentPickUpPM>();
            shipmentPickUpPM.Add(ShipmentPickUpItem("PICK", "PORT", "PORT",ShipmentVariables.PortJFKId,ShipmentVariables.PortLHRId,ShipmentVariables.ShipperExport1, ShipmentVariables.TruckerId));
            return shipmentPickUpPM;
        }

        public static ShipmentPickUpPM ShipmentPickUpItem(string PickUpDeliveryTypeCode,string PickUpDeliveryFromTypeCode, string PickUpDeliveryToTypeCode, string FromPortId, string ToPortId, string FromPartnerCardId, string CarrierId)
        {
            ShipmentPickUpPM ShipmentPickUpItem = new ShipmentPickUpPM();
            ShipmentPickUpItem.PickUpDeliveryTypeCode = PickUpDeliveryTypeCode;
            ShipmentPickUpItem.PickUpDeliveryFromTypeCode = PickUpDeliveryFromTypeCode;
            ShipmentPickUpItem.PickUpDeliveryToTypeCode = PickUpDeliveryToTypeCode;
            ShipmentPickUpItem.FromPortId = FromPortId;
            ShipmentPickUpItem.ToPortId = ToPortId;
            ShipmentPickUpItem.FromPartnerCardId = FromPartnerCardId;
            ShipmentPickUpItem.CarrierId = CarrierId;
            ShipmentPickUpItem.ShipmentPickUpDeliveryPackages = IntegrationShipmentPickUpDeliveriesPackages.ShipmentPickUpDeliveryPackage();
            return ShipmentPickUpItem;

        }

        

    }
}
