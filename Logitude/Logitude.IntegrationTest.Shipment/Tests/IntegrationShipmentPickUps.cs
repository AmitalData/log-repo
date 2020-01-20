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
        public List<ShipmentPickUpPM> shipmentPickUpPM;
        public IntegrationShipmentPickUps() {
            this.shipmentPickUpPM  = new List<ShipmentPickUpPM>();
        }

        public List<ShipmentPickUpPM> ShipmentPickUps()
        {
            shipmentPickUpPM.Add(ShipmentPickUpItem("PICK", "PORT", "PORT",ShipmentVariables.PortJFKId,ShipmentVariables.PortLHRId,ShipmentVariables.ShipperExport1, ShipmentVariables.TruckerId));
            return shipmentPickUpPM;
        }

        public  ShipmentPickUpPM ShipmentPickUpItem(string PickUpDeliveryTypeCode,string PickUpDeliveryFromTypeCode, string PickUpDeliveryToTypeCode, string FromPortId, string ToPortId, string FromPartnerCardId, string CarrierId)
        {
            ShipmentPickUpPM ShipmentPickUpItem = new ShipmentPickUpPM();
            ShipmentPickUpItem.PickUpDeliveryTypeCode = PickUpDeliveryTypeCode;
            ShipmentPickUpItem.PickUpDeliveryFromTypeCode = PickUpDeliveryFromTypeCode;
            ShipmentPickUpItem.PickUpDeliveryToTypeCode = PickUpDeliveryToTypeCode;
            ShipmentPickUpItem.FromPortId = FromPortId;
            ShipmentPickUpItem.ToPortId = ToPortId;
            ShipmentPickUpItem.FromPartnerCardId = FromPartnerCardId;
            ShipmentPickUpItem.CarrierId = CarrierId;
            ShipmentPickUpItem.ShipmentPickUpDeliveryPackages = this.ShipmentPickUpDeliveryPackage();
            return ShipmentPickUpItem;

        }

        public List<ShipmentPickUpDeliveryPackagePM> ShipmentPickUpDeliveryPackage()
        {
            List<ShipmentPickUpDeliveryPackagePM> ShipmentPickUpDeliveryPackage = new List<ShipmentPickUpDeliveryPackagePM>();
             ShipmentPickUpDeliveryPackage.Add(PickUpPackageItem(1,10,10,10,10));
            ShipmentPickUpDeliveryPackage.Add(PickUpPackageItem(3, 10, 10, 10, 10));

            return ShipmentPickUpDeliveryPackage;
        }

        public  ShipmentPickUpDeliveryPackagePM PickUpPackageItem(int quantity, double? length, double? width, double? height, double? weight)
        {
            ShipmentPickUpDeliveryPackagePM PickUpPackageItem = new ShipmentPickUpDeliveryPackagePM();

            PickUpPackageItem.Quantity = quantity;
            PickUpPackageItem.Length = length;
            PickUpPackageItem.Width = width;
            PickUpPackageItem.Height = height;
            PickUpPackageItem.Weight = weight;
           
            return PickUpPackageItem;
        }

    }
}
