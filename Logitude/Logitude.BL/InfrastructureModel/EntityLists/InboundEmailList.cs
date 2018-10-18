using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class InboundEmailList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public string EntityId { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public string Uniquekey { get; set; }

        public string ObjectTableId { get; set; }

        public string CreatedByContactId { get; set; }

        public string ObjectTableName { get; set; }

        public bool IsRejected { get; set; }

        public string AnalyzeQueueId { get; set; }

    }
}
