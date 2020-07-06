using Logitude.Accounting.BL.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportsCreationForCustomersArgs
    {
        public DateTime InterestCalculationDate { get; set; }
        public int Tenant { get; set; }

        public List<InterestReportCustomerPM> EligibleCustomers { get; set; }

    }
}
