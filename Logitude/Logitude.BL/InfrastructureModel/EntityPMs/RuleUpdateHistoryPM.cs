using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityPMs
{
    public class RuleUpdateHistoryPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }

        public string RuleCode { get; set; }
        public string EventName { get; set; }

        public string CreatedByUserId { get; set; }
         
        public string UpdatedByUserId { get; set; }
       
    }
}
