using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactLoginLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string IP { get; set; }
        public string Browser { get; set; }
        public string ContactId { get; set; }

        public DateTime? GMTDateTime { get; set; }
        public DateTime? LocalDateTime { get; set; }

        public string ComputerId { get; set; }

        public string ContactAgent { get; set; }

        public string Via { get; set; }

        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
