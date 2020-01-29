using Logitude.BL.ShipmentsModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.IntegrationTest.Shipment.Tests
{
    class IntegrationShipmentPickUpDeliveriesPackages
    {

        public static List<ShipmentPickUpDeliveryPackagePM> ShipmentPickUpDeliveryPackage()
        {
            List<ShipmentPickUpDeliveryPackagePM> ShipmentPickUpDeliveryPackage = new List<ShipmentPickUpDeliveryPackagePM>();
            ShipmentPickUpDeliveryPackage.Add(PickUpDeliveryPackageItem(1, 10, 10, 10, 10));
            ShipmentPickUpDeliveryPackage.Add(PickUpDeliveryPackageItem(3, 10, 10, 10, 10));

            return ShipmentPickUpDeliveryPackage;
        }

        public static ShipmentPickUpDeliveryPackagePM PickUpDeliveryPackageItem(int quantity, double? length, double? width, double? height, double? weight)
        {
            ShipmentPickUpDeliveryPackagePM PickUpDeliveryPackageItem = new ShipmentPickUpDeliveryPackagePM();

            PickUpDeliveryPackageItem.Quantity = quantity;
            PickUpDeliveryPackageItem.Length = length;
            PickUpDeliveryPackageItem.Width = width;
            PickUpDeliveryPackageItem.Height = height;
            PickUpDeliveryPackageItem.Weight = weight;

            return PickUpDeliveryPackageItem;
        }
    }
}
