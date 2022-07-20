using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.Accounting.BL.CoreBL.InterestReport
{
    public class InterestReportLinesCreationService
    {
        InterestReportLineUpdateService interestReportLineUpdateService;
        const int maxAllowedLinesCount = 3000;
        private string interest_Report_Id;
        public void CreateInterestReportLines(List<InterestTransactionPM> interestTransactionPMs, string interestReportId, int tenant)
        {
            interest_Report_Id = interestReportId;
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            interestReportLineUpdateService = new InterestReportLineUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            HandleInterestReportLines(interestTransactionPMs);
            //for (int i = 0; i < interestTransactionPMs.Count; i++)
            //{
            //    InterestReportLinePM interestReportLinePM = new InterestReportLinePM()
            //    {
            //        InterestReportId = interestReportId,
            //        InterestTransactionId = interestTransactionPMs[i].Id,
            //        Tenant = tenant,
            //        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
            //    };
            //    interestReportLineUpdateService.Update(interestReportLinePM, true);
            //}
        }

        private void HandleInterestReportLines(List<InterestTransactionPM> interestTransactionPMs)
        {
            if (interestTransactionPMs.Count > maxAllowedLinesCount)
            {
                InsertMoreThanMaxAllowedLinesCount(interestTransactionPMs);
            }
            else
            {
                InsertInterestTransactions(interestTransactionPMs, 0);
            }
        }

        private void InsertMoreThanMaxAllowedLinesCount(List<InterestTransactionPM> interestTransactions)
        {
            List<InterestTransactionPM> createdLines = new List<InterestTransactionPM>();

            for (int i = 0; i < interestTransactions.Count; i += maxAllowedLinesCount)
            {
                createdLines = createdLines.Concat(InsertInterestTransactionsInMaxAllowedListRange(interestTransactions, i)).ToList();
            }
           
        }

        private List<InterestTransactionPM> InsertInterestTransactionsInMaxAllowedListRange(List<InterestTransactionPM> interestTransactions, int startIndex)
        {
            return InsertInterestTransactions(interestTransactions.GetRange(startIndex, Math.Min(maxAllowedLinesCount, interestTransactions.Count - startIndex)), startIndex);
        }

        private List<InterestTransactionPM> InsertInterestTransactions(List<InterestTransactionPM> interestTransactions, int startIndex)
        {
            using (TransactionScope scope = TransactionFactory.GetTransaction(TimeSpan.FromMinutes(60)))
            {
                int count = startIndex;
                for (int i = 0; i < interestTransactions.Count; i++)
                {
                    InterestReportLinePM interestReportLinePM = new InterestReportLinePM()
                    {
                        InterestReportId = interest_Report_Id,
                        InterestTransactionId = interestTransactions[i].Id,
                        Tenant = interestTransactions[i].Tenant,
                        ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert,
                    };
                    interestReportLineUpdateService.Update(interestReportLinePM, true);
                }
                scope.Complete();
                return interestTransactions;

            }
        }
    }
}
