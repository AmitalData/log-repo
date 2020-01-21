using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityLists
{
    public class RuleUpdateHistoryList
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string RuleCode { get; set; }
        public string EventName { get; set; }

        public string CreatedBy { get; set; }
        
        public string UpdatedBy { get; set; }
        
    }
}
