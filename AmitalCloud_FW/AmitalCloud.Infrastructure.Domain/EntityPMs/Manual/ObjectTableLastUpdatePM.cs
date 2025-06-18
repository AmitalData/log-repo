using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class ObjectTableLastUpdatePM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string ObjectTableId { get; set; }
        public string ObjectTableName { get; set; }


    }
}
