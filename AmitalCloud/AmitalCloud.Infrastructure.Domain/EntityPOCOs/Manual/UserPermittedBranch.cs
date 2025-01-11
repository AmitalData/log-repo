using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class UserPermittedBranch
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string UserId { get; set; }

        public string BranchId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }

    }
}
