using System;
using System.ComponentModel.DataAnnotations;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class SharedLogisticsUpdateList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public DateTime ReceivedDate { get; set; }
        public string ReceivedFrom { get; set; }
        public string DocumentId { get; set; }
        public string Subject { get; set; }
        public bool Read { get; set; }
        public string Status { get; set; }
        public string HandledByUserId { get; set; }
        public DateTime? HandledDate { get; set; }

     
    }
}