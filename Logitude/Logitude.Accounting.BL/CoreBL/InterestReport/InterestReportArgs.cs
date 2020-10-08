using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportArgs
    {
        public string InterestReportId { get; set; }
        public InterestReportPM InterestReport { get; set; }
        public int Tenant { get; set; }
        public string ReportNumber { get; set; }
        public string Email { get; set; }
        public bool RecalculateData { get; set; }
        public DateTime InvoiceDate { get; set; }
        public bool CloseWithoutInvoice{get; set;}
    }
}
