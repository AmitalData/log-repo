using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class InterestTransactionQueryService
    {
        public List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(string glAccountId, DateTime InterestReportCalculationDate, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);
            IQueryable<InterestTransaction> interestTransactions = interestTransactionRepository.GetInterestTransactionsForGlAccountAndInterestValueDate(glAccountId, InterestReportCalculationDate, tenant);

            List<InterestTransactionPM> interestTransactionPMs = MapInterestTransactionsPocosToPMs(interestTransactions);
            return interestTransactionPMs;

        }

        private List<InterestTransactionPM> MapInterestTransactionsPocosToPMs(IQueryable<InterestTransaction> interestTransactions)
        {
            List<InterestTransactionPM> interestTransactionPMs = new List<InterestTransactionPM>();
            foreach(InterestTransaction interestTransaction in interestTransactions)
            {
                InterestTransactionPM interestTransactionPM = this.GetEntityPM(interestTransaction, false);
                interestTransactionPMs.Add(interestTransactionPM);
            }
            return interestTransactionPMs;
        }
    }
}
