using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class VatTypePercentage
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string VatTypeId { get; set; }
        public DateTime? FromDate { get; set; }
        public double? Percentage { get; set; }

        [ForeignKey("VatTypeId")]
        public virtual VatType VatType { get; set; }
    }
}