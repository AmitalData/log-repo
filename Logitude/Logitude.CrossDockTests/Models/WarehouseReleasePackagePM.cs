using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models
{
    public class WarehouseReleasePackagePM
    {
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string WarehouseReleaseId { get; set; }
        public int Quantity { get; set; }
        public double? Weight { get; set; }
        public object Volume { get; set; }
        public object Description { get; set; }
        public object PackageTypeId { get; set; }
        public object Seal { get; set; }
        public object Harmonize { get; set; }
        public double? Width { get; set; }
        public double? Length { get; set; }
        public double? Height { get; set; }
        public string Dimensions { get; set; }
        public bool IsContainer { get; set; }
        public string EntryPackageId { get; set; }
        public object ContainerNumberWarning { get; set; }
        public object ActualReleaseDate { get; set; }
        public object DimensionUnitCode { get; set; }
        public object VolumeUnitCode { get; set; }
        public object GrossWeightUnitCode { get; set; }
        public double? VolumetricWeight { get; set; }
        public object ChargeableWeightUnitCode { get; set; }
        public string ChangeSetOp { get; set; }
    }
}
