using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    class IntegrationShipmentPackages
    {
        public static List<ShipmentPackagePM> ShipmentPackages()
        {
            List<ShipmentPackagePM> shipmentPackagePM = new List<ShipmentPackagePM>();

            shipmentPackagePM.Add(ShipmentPackageItem(5, 5, 5, 5, 100));
            shipmentPackagePM.Add(ShipmentPackageItem(7, 10, 10, 10, 120));
            return shipmentPackagePM;
        }
        public static ShipmentPackagePM ShipmentPackageItem(int quantity, double? length, double? width, double? height, double? weight)
        {
            ShipmentPackagePM shipmentPackageItem = new ShipmentPackagePM();

            shipmentPackageItem.Quantity = quantity;
            shipmentPackageItem.Length = length;
            shipmentPackageItem.Width = width;
            shipmentPackageItem.Height = height;
            shipmentPackageItem.Weight = weight;
            //shipmentPackageItem.VolumetricWeight = 0.104;
            return shipmentPackageItem;
        }
    }
}
