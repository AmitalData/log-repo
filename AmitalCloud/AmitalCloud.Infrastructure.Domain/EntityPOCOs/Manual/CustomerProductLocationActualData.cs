using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CustomerProductLocationActualData
    {
        [Key]
        public string CustomerId { get; set; }
        [Key]
        public string ProductTypeCode { get; set; }
        [Key]
        public int Month { get; set; }
        [Key]
        public int Year { get; set; }
        [Key]
        public string CountryId { get; set; }

        public int Tenant { get; set; }
        public decimal? TEU { get; set; }
        public int? NumberOfShipments { get; set; }
        public decimal? ChargeableWeight { get; set; }

        public decimal? Revenue { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country Country { get; set; }
    }
}
