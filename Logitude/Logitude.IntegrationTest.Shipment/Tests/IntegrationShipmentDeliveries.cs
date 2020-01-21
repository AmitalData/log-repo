using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    class IntegrationShipmentDeliveries
    {

        public static List<ShipmentDeliveryPM> shipmentDelivey()
        {
            List<ShipmentDeliveryPM> shipmentDeliveyPM = new List<ShipmentDeliveryPM>();
            shipmentDeliveyPM.Add(shipmentDeliveryItem("DELV", "PORT", "PORT", ShipmentVariables.PortLHRId, ShipmentVariables.PortLONId, ShipmentVariables.ShipperExport1, ShipmentVariables.TruckerId));
            return shipmentDeliveyPM;
        }

        public static ShipmentDeliveryPM shipmentDeliveryItem(string PickUpDeliveryTypeCode, string PickUpDeliveryFromTypeCode, string PickUpDeliveryToTypeCode, string FromPortId, string ToPortId, string ToPartnerCardId, string CarrierId)
        {
            ShipmentDeliveryPM shipmentDeliveryItem = new ShipmentDeliveryPM();
            shipmentDeliveryItem.PickUpDeliveryTypeCode = PickUpDeliveryTypeCode;
            shipmentDeliveryItem.PickUpDeliveryFromTypeCode = PickUpDeliveryFromTypeCode;
            shipmentDeliveryItem.PickUpDeliveryToTypeCode = PickUpDeliveryToTypeCode;
            shipmentDeliveryItem.FromPortId = FromPortId;
            shipmentDeliveryItem.ToPortId = ToPortId;
            shipmentDeliveryItem.ToPartnerCardId = ToPartnerCardId;
            shipmentDeliveryItem.CarrierId = CarrierId;
            shipmentDeliveryItem.ShipmentPickUpDeliveryPackages = IntegrationShipmentPickUpDeliveriesPackages.ShipmentPickUpDeliveryPackage();


            return shipmentDeliveryItem;
        }

        



    }
}
