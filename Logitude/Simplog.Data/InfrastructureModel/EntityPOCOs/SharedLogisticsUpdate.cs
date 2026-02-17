using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class SharedLogisticsUpdate
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

        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }
        [ForeignKey("DocumentId")]
        public virtual Document Document { get; set; }
        [ForeignKey("HandledByUserId")]
        public virtual User HandledByUser { get; set; }

        [ForeignKey("Status")]
        public virtual SharedLogisticsUpdateStatus SharedLogisticsUpdateStatus { get; set; }
        
    }
}