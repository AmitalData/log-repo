using System;
using System.ComponentModel.DataAnnotations;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class ContactActivityLog
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Module { get; set; }
        public string Activity { get; set; }
        public string ContactId { get; set; }
        public DateTime LogDateTime { get; set; }
        public DateTime GMTLogDateTime { get; set; }
        public bool IsSharedLogisticsContact { get; set; }
        public string CardId { get; set; }
        public string PartnerTypeId { get; set; }
        public string Via { get; set; }
    }
}
