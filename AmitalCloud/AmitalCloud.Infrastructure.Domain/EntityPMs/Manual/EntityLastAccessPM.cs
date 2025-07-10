using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPMs
{
    public class EntityLastAccessPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string UserId { get; set; }
        public DateTime AccessDate { get; set; }
        public string EntityId { get; set; }
        public string UserName { get; set; }
    }
}