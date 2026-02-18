using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ContactTenantPM : BaseEntityPM
    {
        private ContactTenant contactTenant;

        public ContactTenantPM(ContactTenant contactTenant)
        {
            this.contactTenant = contactTenant;
            this.Id = contactTenant.Id;
            this.TenantId = contactTenant.TenantId;
            this.ContactId = contactTenant.ContactId;
        }

        [Key]
        public string Id { get; set; }
        public int TenantId { get; set; }
        public string ContactId { get; set; }
    }
}
