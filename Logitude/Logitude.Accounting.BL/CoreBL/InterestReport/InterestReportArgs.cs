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
        public int Tenant { get; set; }
        public string ReportNumber { get; set; }
        public string Email { get; set; }

    }
}
