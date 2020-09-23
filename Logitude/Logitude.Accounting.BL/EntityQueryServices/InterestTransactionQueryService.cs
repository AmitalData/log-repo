using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.Utilities;
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
        public List<InterestTransactionPM> GetInterestTransactionsForGlAccountAndInterestValueDate(InterestTransactionGetParameters interestTransactionGetParameters)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(interestTransactionGetParameters.Tenant);
            IQueryable<InterestTransaction> interestTransactions = interestTransactionRepository.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);

            List <InterestTransactionPM> interestTransactionPMs = MapInterestTransactionsPocosToPMs(interestTransactions);
            return interestTransactionPMs;

        }
        public List<InterestTransactionPM> GetInterestTransactionsByInterestReportId(string interestReportId, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);
            List<InterestTransaction> interestTransactions = (from a in context.InterestTransactions
                                                                    where a.InterestReportId == interestReportId && a.Tenant== tenant select a).ToList();
            List<InterestTransactionPM> interestTransactionPMs = interestTransactions.Select(r => this.GetEntityPM(r)).ToList();
            return interestTransactionPMs;

        }
        public InterestTransactionPM GetOpenBalanceInterestTransactionsByInterestReportId(string interestReportId, int tenant)
        {
            InterestTransactionRepository interestTransactionRepository = new InterestTransactionRepository(tenant);
            InterestTransaction interestTransaction = (from a in context.InterestReportLines
                                                       where a.Tenant == tenant && a.InterestReportId == interestReportId && a.InterestTransaction.InterestEntityTypeCode == "4"
                                                       select a.InterestTransaction).FirstOrDefault();

            InterestTransactionPM interestTransactionPM = null;
            if (interestTransaction != null)
            {
              interestTransactionPM = this.GetEntityPM(interestTransaction);
            }
            
            return interestTransactionPM;

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
