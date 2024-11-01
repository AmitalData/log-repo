using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmitalCloud.Infrastructure.Domain.EntityPOCOs
{
    public class AgentSharedLogisticsKey
    {
        [Key]
        public string SharedKey { get; set; }
        public int Agent1Tenant { get; set; }
        public int Agent2Tenant { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedByUserEmail { get; set; }
        public string ApprovedByUserEmail { get; set; }
        public DateTime? ApproveDate { get; set; }

        public DateTime? InactiveDate { get; set; }
        public string InactiveByUserEmail { get; set; }

        public string StatusCode { get; set; }
 
    }
}
