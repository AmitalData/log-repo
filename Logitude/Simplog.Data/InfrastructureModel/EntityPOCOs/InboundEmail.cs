using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.EntityPOCOs
{
    public class InboundEmail
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        
        public string EntityId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Uniquekey { get; set; }

        public string ObjectTableId { get; set; }
        [ForeignKey("ObjectTableId")]
        public virtual ObjectTable ObjectTable { get; set; }

        public string CreatedByContactId { get; set; }
        [ForeignKey("CreatedByContactId")]
        public virtual Contact CreatedByContact { get; set; }

        public bool IsRejected { get; set; }

        public string AnalyzeQueueId { get; set; }
    }
}
