using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityLastActivity
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string UserId { get; set; }
        public DateTime ActivityDate { get; set; }
        public string EntityId { get; set; }
        public string ActivityTypeCode { get; set; }
        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        [ForeignKey("ActivityTypeCode")]
        public virtual EntityLastActivityType ActivityType { get; set; }

    }
}