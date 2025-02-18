using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ContactTenantRolePM
    {
        [Key]
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string ContactTenantId { get; set; }
        public int Tenant { get; set; }
    }
}
