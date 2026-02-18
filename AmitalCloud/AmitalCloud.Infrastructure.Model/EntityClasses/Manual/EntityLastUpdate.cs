using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class EntityLastUpdate
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string EntityId { get; set; }
        public string ObjectTableId { get; set; }
        public string UpdatedByUserId { get; set; }
        public DateTime UpdateDate { get; set; }
        public string EntityGUID { get; set; }

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
    }
}
