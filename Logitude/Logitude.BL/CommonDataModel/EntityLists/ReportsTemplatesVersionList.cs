using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.EntityLists
{
   public class ReportsTemplatesVersionList
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
        public string UpdateByUserName { get; set; }
        public bool IsRestored { get; set; }
        
    }
}
