using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class Automation
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string ObjectTableId { get; set; }
        public string TemplateId { get; set; }
        public string DocumentTypeId { get; set; }
        public int Version { get; set; }
        public string  AutomationXML { get; set; }

        public string Type { get; set; }
        public string ResultCode { get; set; }
        public string Description { get; set; }
        public bool Inactive { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string CreatedByUserId { get; set; }
        public string UpdatedByUserId { get; set; }
        public string From { get; set; }
        public string FromEmail { get; set; }
        public int Order { get; set; }
        public bool IsAutomationDone { get; set; }
        public string Code { get; set; }


        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

    }
}
