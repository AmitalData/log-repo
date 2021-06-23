using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public static class CrossDockReleaseData
    {
        public static int Tenant { get; set; }
        public static string CreatedByUserId { get; set; }
        public static string UpdatedByUserId { get; set; }
        public static DateTime CreateDate { get; set; }
        public static DateTime UpdateDate { get; set; }
        public static string StatusCode { get; set; }
        public static string Id { get; set; }
        public static string GrossWeightUnitCode { get; set; }
        public static string VolumeUnitCode { get; set; }
        public static string DimensionsUnitCode { get; set; }
        public static string ChargeableWeightUnitCode { get; set; }
        public static int TotalVolume { get; set; }
        public static int TotalGrossWeight { get; set; }
        public static int TotalPieces { get; set; }
        public static string ReleaseNumber { get; set; }
        public static List<WarehouseReleasePackagePM> WarehouseReleasePackages { get; set; }
        public static string WarehouseId { get; set; }
        public static string CustomerId { get; set; }
        public static int TotalVolumetricWeight { get; set; }
        public static string FromPortId { get; set; }
        public static string ToTypeCode { get; set; }
    }
}
