using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CRM.Data.EntityLists
{
    public class OpportunityJoinOpportunityStageList
    {
        [Key]
        public string Id { get; set; }
        public string OpportunityId { get; set; }
        public int? Probability { get; set; }
        public DateTime? CreateDate { get; set; }
        public string StageName { get; set; }

    }
}
