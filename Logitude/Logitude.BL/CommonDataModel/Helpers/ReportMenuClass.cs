using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.Helpers
{
    public class ReportMenuClass
    {
        public ReportMenuClass()
        {
        }
        public string StatusCode { get; set; }
        public string ReportLocalName { get; set; }
        public string ReportName { get; set; }
        public DateTime CreateDate { get; set; }
        public string ExceptionMessage { get; set; }
        public string ReportId { get; set; }
        public string ReportFilterXML { get; set; }
        public string ReportTemplateId { get; set; }
        public bool NotDisplayInMenu { get; set; }
        public int ItemType { get; set; }
        public string Id { get; set; }
    }

     enum MenuTypes
    {
       
        ReportExecutionLog = 0,
        BatchTaskExecution = 1,
    }
    
}
