using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public class CrossDockReleasePM
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
        public string ChargeableWeightUnitCode { get; set; }
        public int TotalVolume { get; set; }
        public int TotalGrossWeight { get; set; }
        public int TotalPieces { get; set; }
        public string ReleaseNumber { get; set; }
        public List<WarehouseReleasePackagePM> WarehouseReleasePackages { get; set; }
        public string WarehouseId { get; set; }
        public string CustomerId { get; set; }
        public int TotalVolumetricWeight { get; set; }
        public string FromPortId { get; set; }
        public string ToTypeCode { get; set; }
        public string CustomerRef1 { get; set; }
        public string CustomerRef2 { get; set; }
        public string HouseNumber { get; set; }
        public string MasterNumber { get; set; }
    }
}
