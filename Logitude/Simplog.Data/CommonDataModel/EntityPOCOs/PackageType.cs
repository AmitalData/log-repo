using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class PackageType
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Code { get; set; }
        public string EnglishName { get; set; }
        public string LocalName { get; set; }
        public bool IsOcean { get; set; }
        public bool IsAir { get; set; }
        public bool IsInland { get; set; }
        public bool AddedManually { get; set; }
        public string Notes { get; set; }
        public bool IsContainer { get; set; }
        public double TEU { get; set; }
        public int ContainerSize { get; set; }
        public decimal Volume { get; set; }
        public bool InActive { get; set; }
        public string MeasurementId { get; set; }
        public string SearchFields { get; set; }
        public string PrintAs { get; set; }
        public bool IsRefrigerated { get; set; }
        public bool IsVehicle { get; set; }

        [ForeignKey("MeasurementId")]
        public virtual Measurement Measurement { get; set; }
    }
}