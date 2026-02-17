using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class GeneralLock
    {
        [Key]
        [Column("GeneralKey", Order = 1)]
        public string GeneralKey { get; set; }
        [Key]
        [Column("Tenant", Order = 2)]
        public int Tenant { get; set; }

        public DateTime  CreatedAt { get; set; }
    }
}
