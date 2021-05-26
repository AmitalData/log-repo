using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Logitude.BL.CommonDataModel.EntityPMs
{
    
    public class ReportExecutionLogPM
    {
        [Key]
        public string Id { get; set; }
        public int Tenant { get; set; }
        public string CreatedByUserId { get; set; }
        public string StatusCode { get; set; }
        public string ExceptionMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? DoneDate { get; set; }
        public string ReportFilterXML { get; set; }
        public string ReportId { get; set; }
        public string ReportTemplateId { get; set; }
        public int RetryNumber { get; set; }
        public DateTime? StartDate { get; set; }
        public string ExecutedByServerName { get; set; }

        public bool DisablePreview { get; set; }

        public string StatusName { get; set; }
        public string CreatedByUserName { get; set; }
        public string SearchFields { get; set; }
        public string ReportName { get; set; }

    }
}