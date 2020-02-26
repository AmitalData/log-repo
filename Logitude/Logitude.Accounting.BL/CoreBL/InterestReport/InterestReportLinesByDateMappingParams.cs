using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportLinesByDateMappingParams
    {
        public InterestReportLinesByDateMappingParams(InterestTransactionsGroupedByDate currentInterestTransactionGroupedByDate,
            InterestTransactionsGroupedByDate nextInterestTransactionGroupedByDate,
            string interestReportId,
            int tenant,
            InterestReportLinesByDateCreationParams InterestReportLinesByDateCreationParams,
            decimal accumulatedAmount)
        {
            CurrentInterestTransactionGroupedByDate = currentInterestTransactionGroupedByDate;
            NextInterestTransactionGroupedByDate = nextInterestTransactionGroupedByDate;
            Tenant = tenant;
            AccumulatedAmount = accumulatedAmount;
            InterestReportId = interestReportId;

        }
        public InterestTransactionsGroupedByDate CurrentInterestTransactionGroupedByDate { get; private set; }
        public InterestTransactionsGroupedByDate NextInterestTransactionGroupedByDate { get; private set; }
        public string InterestReportId { get; private set; }
        public int Tenant { get; private set; }
        public InterestReportLinesByDateCreationParams InterestReportLinesByDateCreationParams { get; private set; }
        public decimal AccumulatedAmount { get; private set; }
    }
}
