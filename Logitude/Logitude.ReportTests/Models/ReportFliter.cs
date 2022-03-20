using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.ReportTests.Models
{
    public class ReportFliter
    {
        public string DefaultTemplateId { get; set; }
        public int DefaultTemplateVsersion { get; set; }
        public int NumberOfPage { get; set; }
        public string ReportCode { get; set; }
        public string ReportId { get; set; }
        public string ReportName { get; set; }
        public bool ReportsRunUsingWR { get; set; }
        public int Tenant { get; set; }
        public string UserId { get; set; }
        public string ProcessType { get; set; }
        public string ReportKey { get; set; }
        public List<ReportFliterItem> QueryFilterItemLists { get; set; }

    }
}
