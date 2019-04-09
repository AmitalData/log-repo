using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.WarehouseLib.BL.DataContracts
{

    public class WarehouseEntryPackageItem
    {
        public string WarehouseEntryPackagId { get; set; }
        public string WarehouseId { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerReference { get; set; }
        public string PackageType { get; set; }
        public string Dimensions { get; set; }
        public string Description { get; set; }
        public string EntryNumber { get; set; }
        public int QuantityNotRelease { get; set; }
        public int DaysInWarehouse { get; set; }
        public DateTime? ActualEntryDate { get; set; }
        public string WarehouseName { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
        public string ContainerNumber { get; set; }
        public string ShipperConsignee { get; set; }
        public string ShipmentId { get; set; }
        public string Location { get; set; }
        public string ShipperId { get; set; }
        public string ConsigneeId { get; set; }
        public string DirectionId { get; set; }
        public decimal? GrossWeight { get; set; }
        public decimal? Volume { get; set; }
        public int Quantity { get; set; }
        public string ShipmentNumber { get; set; }
        public string InternalNotes { get; set; }
        public string SpecialInstructions { get; set; }
        public double? VolumetricWeight { get; set; }
        public string EntryReference { get; set; }
    }

}
