using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class ReportModification
    {
        [Key]
        public string ReportId { get; set; }
        [Key]
        public int Tenant { get; set; }

        public string ReportDocumentId { get; set; }

        [ForeignKey("ReportId")]
        public virtual Report Report { get; set; }

        [ForeignKey("ReportDocumentId")]
        public Document ReportDocument { get; set; }
    }
}
