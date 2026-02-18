using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class HybridPartnersPermission
    {
        [Key]
        [Column("HybridPartnerId", Order = 1)]
        public string HybridPartnerId { get; set; }
        [ForeignKey("HybridPartnerId")]
        public virtual HybridPartner HybridPartner { get; set; }

        [Key]
        [ForeignKey("AllowedByHybridPartner")]
        [Column("AllowedByHybridPartnerId", Order = 2)]
        public string AllowedByHybridPartnerId { get; set; }
        public virtual HybridPartner AllowedByHybridPartner { get; set; }

        public bool InActive { get; set; }


    }
}
