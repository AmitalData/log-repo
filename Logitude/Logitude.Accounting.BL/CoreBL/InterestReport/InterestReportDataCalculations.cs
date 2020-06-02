using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Utilities;
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
    public class InterestReportDataCalculations
    {
        private InterestReportPM interestReportPM;
        private List<InterestTransactionPM> interestTransactionPMs;
        private string interestReportId;
        private int tenant;
        IInterestReportCalculationPreparations interestReportCalculationPreparations;
        public InterestReportDataCalculations(InterestReportArgs interestReportArgs)
        {
            interestReportId = interestReportArgs.InterestReportId;
            tenant = interestReportArgs.Tenant;
            interestReportCalculationPreparations =  new InterestReportCalculationPreparations();
        }

        public void StartCalculations()
        {
            try
            {
                interestReportPM = interestReportCalculationPreparations.GetInterestReportPM(interestReportId, tenant);
                DateTime? interestCalculationStartDate = GetInterestCalculationStartDate();
                InterestTransactionGetParameters interestTransactionGetParameters = new InterestTransactionGetParameters(interestReportPM.InterestCalculationDate, tenant, interestReportPM.GLAccountId, interestCalculationStartDate);
                interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    CreateInterestReportLines();
                    interestReportPM.OpenBalance = GetInterestReportOpenBalance();
                    List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = CreateInterestReportLinesByDate();
                    interestReportPM.CloseBalance = GetInterestReportCloseBalance(interestReportLinesByDatePMs);
                    interestReportPM.TotalAmount = GetInterestReportTotalAmount(interestReportLinesByDatePMs);
                    SetInterestReportStatusDraft();
                    SubmitInterestReportLinesByDate(interestReportLinesByDatePMs);
                    SubmitChangesToInterestReport();
                    scope.Complete();
                }
            }
            catch
            {
                SetInterestReportStatusFailed();
                throw;
            }
        }

        private DateTime? GetInterestCalculationStartDate()
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            DateTime? interestCalculationStartDate = gLAccountQueryService.GetInterestCalculationStartDate(interestReportPM.GLAccountId, tenant);
            return interestCalculationStartDate;
        }

        private decimal? GetInterestReportTotalAmount(List<InterestReportLinesByDatePM> interestReportLinesByDatePMs)
        {
            decimal? totalAmount = (from a in interestReportLinesByDatePMs
                                    select a).Sum(d => (d.CalculatedStandInterestAmount 
                                    + d.CalculatedExcepInterestAmount 
                                    + d.CalculatedCreditInterestAmount));
            return totalAmount;
        }

        private void SetInterestReportStatusDraft()
        {
            interestReportPM.InterestReportStatusCode = "1";
        }
        private void SetInterestReportStatusFailed()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                interestReportPM.InterestReportStatusCode = "6";
                SubmitChangesToInterestReport();
                scope.Complete();
            }
        }

        private void SubmitChangesToInterestReport()
        {
            interestReportPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestReportUpdateService interestReportUpdateService = new InterestReportUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            interestReportUpdateService.Update(interestReportPM, true);
        }

        private decimal? GetInterestReportCloseBalance(List<InterestReportLinesByDatePM> interestReportLinesByDatePMs)
        {
            decimal closeBalance = (from a in interestReportLinesByDatePMs
                                    orderby a.FromDate descending
                                    select a.AccumulatedAmount).FirstOrDefault();
            return closeBalance;
        }

        private decimal? GetInterestReportOpenBalance()
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            decimal interestReportOpenBalance = interestReportQueryService.GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(tenant,interestReportPM.GLAccountId);
            return interestReportOpenBalance;
        }

        private void SubmitInterestReportLinesByDate(List<InterestReportLinesByDatePM> interestReportLinesByDatePMs)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            for(int i = 0; i < interestReportLinesByDatePMs.Count; i++)
            {
                InterestReportLinesByDateUpdateService interestReportLinesByDateUpdateService = new InterestReportLinesByDateUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
                interestReportLinesByDateUpdateService.Update(interestReportLinesByDatePMs[i], true);
            }
        }

        private void CreateInterestReportLines()
        {
            InterestReportLinesCreationService interestReportLinesCreationService = new InterestReportLinesCreationService();
            interestReportLinesCreationService.CreateInterestReportLines(interestTransactionPMs, interestReportId, tenant);
        }

        private List<InterestReportLinesByDatePM> CreateInterestReportLinesByDate()
        {
            InterestReportLinesByDateCreationService interestReportLinesByDateCreationService = new InterestReportLinesByDateCreationService();
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs =interestReportCalculationPreparations.GetGlaccountInterestPeriods(interestReportPM);
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestReportCalculationPreparations.GetAllInterestBasesPeriodPMs(tenant);
            InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams = new InterestReportLinesByDateCreationParams(
                interestReportPM,interestTransactionPMs,gLAccountInterestPeriodPMs,interestBasesPeriodPMs);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateCreationService.CreateInterestReportLinesByDate(interestReportLinesByDateCreationParams);
            return interestReportLinesByDatePMs;
        }

    }
}
