using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class HybridPartnersPermission
    {
        [Key]
        [ForeignKey("HybridPartner")]
        [Column("HybridPartnerId", Order = 1)]
        public string HybridPartnerId { get; set; }
        public virtual HybridPartner HybridPartner { get; set; }

        [Key]
        [ForeignKey("AllowedByHybridPartner")]
        [Column("AllowedByHybridPartnerId", Order = 2)]
        public string AllowedByHybridPartnerId { get; set; }
        public virtual HybridPartner AllowedByHybridPartner { get; set; }

        public bool InActive { get; set; }


    }
}
