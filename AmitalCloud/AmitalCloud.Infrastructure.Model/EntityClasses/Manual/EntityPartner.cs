using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityPartner
    {
        [Key]
        public int Id { get; set; }
        public string PartnerId { get; set; }
        public string PartnerType { get; set; }
        public string PartnerContactId { get; set; }

        public bool IsUser { get; set; }
        public string PartnerContactName { get; set; }
        public string PartnerContactMail { get; set; }
    }
}