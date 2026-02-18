using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ObjectTableLastUpdate
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ObjectTableId { get; set; }

        public virtual User UpdatedByUser { get; set; }
        public virtual ObjectTable ObjectTable { get; set; }
    }
}
