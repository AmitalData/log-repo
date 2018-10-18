using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
   public class ReportsTemplatesVersion
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string CreatedByUserId { get; set; }
        public int Version { get; set; }
        public string ReportId { get; set; }
        public string TemplateId { get; set; }
        public string ReportDocumentId { get; set; }

        public bool  IsRestored { get; set; }

        [ForeignKey("ReportDocumentId")]
        public virtual Document Document { get; set; }

        [ForeignKey("TemplateId")]
        public virtual ReportsTemplate ReportsTemplate { get; set; }

        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [ForeignKey("UpdatedByUserId")]
        public virtual User UpdatedByUser { get; set; }

        [ForeignKey("CreatedByUserId")]
        public virtual User CreatedByUser { get; set; }
    }
}
