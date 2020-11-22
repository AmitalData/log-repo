using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.DataContract;
using Logitude.Accounting.Data.Enums;
using Logitude.Accounting.Data.Utilities;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Server.Infrastructure;
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
        private InterestTransactionPM OpenBalanceTransaction;
        private List<InterestTransactionPM> interestTransactionPMs;
        private string interestReportId;
        private int tenant;
        IInterestReportCalculationPreparations interestReportCalculationPreparations;
        private bool recalculateData;
        public InterestReportDataCalculations(InterestReportArgs interestReportArgs)
        {
            interestReportId = interestReportArgs.InterestReportId;
            tenant = interestReportArgs.Tenant;
            interestReportCalculationPreparations =  new InterestReportCalculationPreparations();
            interestReportPM = interestReportArgs.InterestReport;
            recalculateData = interestReportArgs.RecalculateData;
        }

        public void StartCalculations()
        {
            try
            {
                interestReportPM = interestReportPM !=null?interestReportPM: interestReportCalculationPreparations.GetInterestReportPM(interestReportId, tenant);
                interestReportPM.GLAccountInterestCreditLimit = interestReportCalculationPreparations.GetCreditLimitFromGLAccount(interestReportPM.GLAccountId, tenant);
                DateTime? interestCalculationStartDate = GetInterestCalculationStartDate();
                List<string> glaccountIds = GetSplittedByCurrencyAcountsIds(interestReportPM.GLAccountId, tenant);
                glaccountIds.Add(interestReportPM.GLAccountId);
                InterestTransactionGetParameters interestTransactionGetParameters = new InterestTransactionGetParameters(interestReportPM.InterestCalculationDate,
                    tenant, glaccountIds, interestCalculationStartDate);
                interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);
                OpenBalanceTransaction = interestReportCalculationPreparations.GetOpenBalanceTransactionForInterestReport(interestReportId,tenant);

                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (recalculateData)
                    {
                        ClearOldDataForInterestReport();
                    }
                    CalculateDataForInterestReport();
                    scope.Complete();
                }
            }
            catch (Exception e)
            {
                SetInterestReportStatusFailed();
                throw new Exception(e.Message);
            }
        }

        private void ClearOldDataForInterestReport()
        {
            DeleteInterestReportLines();
            DeleteInterestReportLinesByDate();
        }

        private void DeleteInterestReportLinesByDate()
        {
            InterestReportLinesByDateQueryService interestReportLinesByDateQueryService = new InterestReportLinesByDateQueryService(tenant);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateQueryService.GetInterestReportLinesByDatePMsForInterestReport(interestReportId, tenant);

            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestReportLinesByDateUpdateService interestReportLinesByDateUpdateService = new InterestReportLinesByDateUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

            for (int i = 0; i < interestReportLinesByDatePMs.Count; i++)
            {
                interestReportLinesByDatePMs[i].ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                interestReportLinesByDateUpdateService.Update(interestReportLinesByDatePMs[i], true);
            }

        }

        private void DeleteInterestReportLines()
        {
            InterestReportLineQueryService interestReportLineQueryService = new InterestReportLineQueryService(tenant);
            List<InterestReportLinePM> interestReportLinePMs = interestReportLineQueryService.GetInterestReportLinesForInterestReport(interestReportId, tenant);

            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestReportLineUpdateService interestReportLineUpdateService = new InterestReportLineUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            for (int i = 0; i < interestReportLinePMs.Count; i++)
            {
                interestReportLinePMs[i].ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                interestReportLineUpdateService.Update(interestReportLinePMs[i], true);
            }

        }

        private void CalculateDataForInterestReport()
        {
            interestReportPM.OpenBalance = recalculateData ? interestReportPM.OpenBalance : GetInterestReportOpenBalance();
            CreateOpenBalanceInterestTransaction();
            CreateInterestReportLines();
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = CreateInterestReportLinesByDate();
            interestReportPM.CloseBalance = GetInterestReportCloseBalance(interestReportLinesByDatePMs);
            interestReportPM.TotalAmount = GetInterestReportTotalAmount(interestReportLinesByDatePMs);
            SetGLAccountCreditAllotmentPercentageByGLAccountId(interestReportPM);
            SetInterestReportStatusDraft();
            SubmitInterestReportLinesByDate(interestReportLinesByDatePMs);
            SubmitChangesToInterestReport();
        }

        private void SetGLAccountCreditAllotmentPercentageByGLAccountId(InterestReportPM interestReportPM)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(interestReportPM.Tenant);
            GLAccountPM account=    gLAccountQueryService.GetSinglePM(interestReportPM.GLAccountId, interestReportPM.Tenant);
            interestReportPM.CreditAllotmentPercentage = account != null ? account.CreditAllotmentPercentage : null;
        }

        private void CreateOpenBalanceInterestTransaction()
        {
            CloseBalanceInterestReportData latestInterestReport = GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport();
          
            InterestTransactionPM firstTransaction = interestTransactionPMs.OrderBy(d => d.InterestValueDate).FirstOrDefault();
            DateTime openBalanceInterestValueDate = GetOpenBalanceInterestValueDate(latestInterestReport);

            if (OpenBalanceTransaction == null && (firstTransaction == null || openBalanceInterestValueDate != firstTransaction.InterestValueDate.Date))
            {
                if (latestInterestReport != null)
                {
                    if(latestInterestReport.InterestReportStatusCode == InterestReportStatusCodes.ClosedWithoutInvoice)
                    {
                        CreateNewInterestTransactionPM(openBalanceInterestValueDate);
                    }
                }
                else
                {
                    CreateNewInterestTransactionPM(openBalanceInterestValueDate);
                }
            }
        }

        private DateTime GetOpenBalanceInterestValueDate(CloseBalanceInterestReportData latestInterestReport)
        {
            DateTime openBalanceInterestValueDate;
            if (latestInterestReport != null)
            {
                openBalanceInterestValueDate = latestInterestReport.InterestCalculationDate.Date.AddDays(1);
            }
            else
            {
                openBalanceInterestValueDate = GetGlaccountInterestCalculationStartDate();

            }
            return openBalanceInterestValueDate;
        }

        private void CreateNewInterestTransactionPM(DateTime openBalanceInterestValueDate)
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            string localCurrencyId = tenantQuery.GetLocalCurrencyFromTenant(tenant);

            InterestTransactionPM openBalanceInterestTransaction = new InterestTransactionPM()
            {
                EntityId = interestReportId,
                InterestEntityTypeCode = InterestEntities.OpenBalance,
                InterestValueDate = openBalanceInterestValueDate,
                GLAccountId = interestReportPM.GLAccountId,
                LocalAmount = 0,
                ForeignAmount = 0,
                OriginalEntityLineNumber = 1,
                ChangeSetOp = ChangeSetOperation.Insert,
                CurrencyId = localCurrencyId,
                Tenant = tenant,
                
            };
            
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(accountingContext,new Dictionary<string, IContext>(),tenant);
            interestTransactionUpdateService.Update(openBalanceInterestTransaction, true);
            interestTransactionPMs.Add(openBalanceInterestTransaction);
        }
        private DateTime GetGlaccountInterestCalculationStartDate()
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            DateTime? interestCalculationStartDate = gLAccountQueryService.GetInterestCalculationStartDate(interestReportPM.GLAccountId, tenant);
            return interestCalculationStartDate.Value;
        }

        private List<string> GetSplittedByCurrencyAcountsIds(string accountId, int tenant)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            return gLAccountQueryService.GetSplittedByCurrencyGLAccountIds(accountId, tenant);

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
            interestReportPM.IsUpdatedFromBatch = true;
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

        private CloseBalanceInterestReportData GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport()
        {
            InterestReportQueryService interestReportQueryService = new InterestReportQueryService(tenant);
            return interestReportQueryService.GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport(tenant, interestReportPM.GLAccountId);
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
