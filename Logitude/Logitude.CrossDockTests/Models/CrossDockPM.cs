using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public class CrossDockPM
    {
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string StatusCode { get; set; }
        public string Id { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public string VolumeUnitCode { get; set; }
        public string DimensionsUnitCode { get; set; }
        public string EntryNumber { get; set; }
        public int TotalVolume { get; set; }
        public int TotalGrossWeight { get; set; }
        public int TotalPieces { get; set; }
        public string FromTypeCode { get; set; }
        public string ToTypeCode { get; set; }
        public string DirectionId { get; set; }
        public string TransportModeId { get; set; }
        public int Ratio { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public string ShipperId { get; set; }
        public object FromPartnerId { get; set; }
        public object FromAddressId { get; set; }
        public string ShipperName { get; set; }
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string WarehouseId { get; set; }
        public List<WarehouseEntryPackagePM> WarehouseEntryPackages { get; set; }
        public int TotalVolumetricWeight { get; set; }
    }
}
