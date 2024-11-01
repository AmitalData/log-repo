using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactTenant
    {
        [Key]
        public string Id { get; set; }

        public int TenantId { get; set; }

        public string ContactId { get; set; }

        //[Include]
        //[Association("ContactContactTenant", "ContactId", "Id", IsForeignKey = true)]
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }

        //[Include]
        //[Association("TenantContactTenan", "TenantId", "Id", IsForeignKey = true)]
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }


        //public virtual List<ContactTenantRole> ContactTenantRoles { get; set; }
        //public List<Restriction> Restrictions { get; set; }

    }

}
