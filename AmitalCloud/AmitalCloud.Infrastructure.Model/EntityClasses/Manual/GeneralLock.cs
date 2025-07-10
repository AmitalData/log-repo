using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class GeneralLock
    {
        [Key]
        [Column("GeneralKey", Order = 1)]
        public string GeneralKey { get; set; }
        [Key]
        [Column("Tenant", Order = 2)]
        public int Tenant { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
