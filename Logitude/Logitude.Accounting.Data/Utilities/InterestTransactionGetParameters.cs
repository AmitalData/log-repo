using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.Utilities
{
    public class InterestTransactionGetParameters
    {
        public DateTime InterestCalculationDate { get; set; }
        public int Tenant { get; set; }
        public string GLAccountId { get; set; }
        public DateTime? InterestCalculationStartDate { get; set; }
        public InterestTransactionGetParameters(DateTime interestCalculationDate, int tenant,string glaccountId,DateTime? interestCalculationStartDate)
        {
            InterestCalculationDate = interestCalculationDate;
            Tenant = tenant;
            GLAccountId = glaccountId;
            InterestCalculationStartDate = interestCalculationStartDate;
        }
    }
}
