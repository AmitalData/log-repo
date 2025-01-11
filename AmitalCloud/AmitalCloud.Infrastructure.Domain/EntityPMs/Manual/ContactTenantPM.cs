using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ContactTenantPM
    {
        [Key]
        public string Id { get; set; }
        public int TenantId { get; set; }
        public string ContactId { get; set; }
    }
}
