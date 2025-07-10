using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class CustomerCompetitorProduct
    {
        [Key]
        [Column("CustomerId", Order = 1)]
        public string CustomerId { get; set; }

        [Key]
        [Column("CompetitorId", Order = 2)]
        public string CompetitorId { get; set; }

        [Key]
        [Column("ProductTypeCode", Order = 3)]
        public string ProductTypeCode { get; set; }

        public int Tenant { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [ForeignKey("CompetitorId")]
        public virtual Competitor Competitor { get; set; }

        [ForeignKey("ProductTypeCode")]
        public virtual ProductType ProductType { get; set; }
    }
}
