using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class ReportsTemplate
    {

        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string ReportId { get; set; }
        public string Description { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public int CurrentVersion { get; set; }
        public bool IsSystem { get; set; }
        public bool InActive { get; set; }
        public string TemplateType { get; set; }

        public string From { get; set; }
        public string ReplyTo { get; set; }
        public string CC { get; set; }
        public string Subject { get; set; }
        public bool IsSystemReportFixed { get; set; }


        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }

        public string ObjectTableId { get; set; }
        public string EntityId { get; set; }
        public bool IsCopiedAtSignup { get; set; }
        public string OriginalTemplateId { get; set; }
    }
}
