using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.EntityPOCOs
{
    public class Report
    {
        [Key]
        public string Id { get; set; }
        public string Code { get; set; }
        public int Tenant { get; set; }
        public string Name { get; set; }
        public string LocalName { get; set; }
        public string Description { get; set; }
        public string SearchFields { get; set; }
        public string FilterControlName { get; set; }
        public string ReportGroupId { get; set; }
        public string FeatureId { get; set; }
        public string ReportDocumentId { get; set; }
        public bool InActive { get; set; }
        public string FilterHtmlComponentUrl { get; set; }
        public string DefaultTemplateId { get; set; }
        public string DefaultMessageTemplateId { get; set; }
        public string FeatureUniqeCode { get; set; }
        public bool AvailableForScheduling { get; set; }



        [ForeignKey("DefaultMessageTemplateId")]
        public ReportsTemplate ReportsTemplateDefaultMessage { get; set; }


        [ForeignKey("DefaultTemplateId")]
        public ReportsTemplate ReportsTemplate { get; set; }


        [ForeignKey("ReportGroupId")]
        public virtual ReportGroup ReportGroup { get; set; }

        //[ForeignKey("FeatureId")]
        public Feature Feature { get; set; }

        [ForeignKey("ReportDocumentId")]
        public Document ReportDocument { get; set; }
    }
}
