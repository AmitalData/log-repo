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
    public class RuleUpdateHistory
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public DateTime CreateDate { get; set; }
       
        public DateTime UpdateDate { get; set; }
      
        public string RuleCode { get; set; }
        public string EventName { get; set; }

        public string CreatedByUserId { get; set; }
        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        public string UpdatedByUserId { get; set; }
        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }
    }
}
