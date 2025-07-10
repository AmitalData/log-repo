using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactTenantRole
    {
        [Key]
        public string Id { get; set; }
        public string RoleId { get; set; }

        public string ContactTenantId { get; set; }
        public int Tenant { get; set; }


        //[Include]
        //[Association("ContactTenanContactRole", "ContactTenantId", "Id", IsForeignKey = true)]
        [ForeignKey("ContactTenantId")]
        public virtual ContactTenant ContactTenant { get; set; }

        //[Include]
        //[Association("RoleContactTenantRole", "RoleId", "Id", IsForeignKey = true)]
        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
    }
}
