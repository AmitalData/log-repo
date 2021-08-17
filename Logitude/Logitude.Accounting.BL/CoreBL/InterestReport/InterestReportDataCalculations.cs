using Logitude.Accounting.BL.DataContract;
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
        private const string interestTransactionReportEntityType = "4";
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
            interestReportCalculationPreparations = new InterestReportCalculationPreparations();
            interestReportPM = interestReportArgs.InterestReport;
            recalculateData = interestReportArgs.RecalculateData;
        }

        public void StartCalculations()
        {
            try
            {
                interestReportPM = interestReportPM != null ? interestReportPM : interestReportCalculationPreparations.GetInterestReportPM(interestReportId, tenant);
                interestReportPM.GLAccountInterestCreditLimit = interestReportCalculationPreparations.GetCreditLimitFromGLAccount(interestReportPM.GLAccountId, tenant);
                DateTime? interestCalculationStartDate = GetInterestCalculationStartDate();
                List<string> glaccountIds = GetSplittedByCurrencyAcountsIds(interestReportPM.GLAccountId, tenant);
                glaccountIds.Add(interestReportPM.GLAccountId);
                InterestTransactionGetParameters interestTransactionGetParameters = new InterestTransactionGetParameters(interestReportPM.InterestCalculationDate,
                    tenant, glaccountIds, interestCalculationStartDate);
                interestTransactionPMs = interestReportCalculationPreparations.GetInterestTransactionsForGlAccountAndInterestValueDate(interestTransactionGetParameters);
                OpenBalanceTransaction = interestReportCalculationPreparations.GetOpenBalanceTransactionForInterestReport(interestReportId, tenant);

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
            List<InterestTransactionPM> balanceTransactions = GetOpenBalanceTransactionsByReportLines();

            DeleteInterestReportLines();
            DeleteInterestReportLinesByDate();

            DeleteOpenBalanceTransactions(balanceTransactions);
        }

        private void DeleteOpenBalanceTransactions(List<InterestTransactionPM> balanceTransactions)
        {
            if (balanceTransactions.Count() == 0)
                return;

            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);

            balanceTransactions.ForEach(transaction =>
            {
                transaction.ChangeSetOp = ChangeSetOperation.Delete;
                //interestTransactionUpdateService.Update(transaction, true);
                RemoveFromInterestTransactionsPMsList(transaction);
            });
        }

        private void RemoveFromInterestTransactionsPMsList(InterestTransactionPM t)
        {
            InterestTransactionPM transaction = interestTransactionPMs.Find(d => d.Id == t.Id);
            if (transaction != null)
                interestTransactionPMs.Remove(transaction);
        }

        private List<InterestTransactionPM> GetOpenBalanceTransactionsByReportLines()
        {
            InterestReportLineQueryService interestReportLineQueryService = new InterestReportLineQueryService(tenant);
            List<InterestTransactionPM> interestTransactions = interestReportLineQueryService.GetInterestTransactionForReport(interestReportId, tenant);
            var balanceTransactions = interestTransactions.Where(transaction => transaction.InterestEntityTypeCode == interestTransactionReportEntityType).ToList();
            return balanceTransactions;
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
        private void DeleteInterestReportOpenBalanceTransaction()
        {
            InterestReportLineQueryService interestReportLineQueryService = new InterestReportLineQueryService(tenant);
            List<InterestTransactionPM> interestTransactions = interestReportLineQueryService.GetInterestTransactionForReport(interestReportId, tenant);
            InterestTransactionPM balanceTransaction = interestTransactions.Where(transaction => transaction.InterestEntityTypeCode == interestTransactionReportEntityType).FirstOrDefault();

            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(accountingContext, new Dictionary<string, Simplog.Server.Infrastructure.IContext>(), tenant);
            balanceTransaction.ChangeSetOp = ChangeSetOperation.Delete;
            interestTransactionUpdateService.Update(balanceTransaction, true);
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
            interestReportPM.CalCreditAllotmentCommission = (interestReportPM.GLAccountInterestCreditLimit == null ? 0 : interestReportPM.GLAccountInterestCreditLimit) * interestReportPM.CreditAllotmentPercentage * (decimal?)0.01;
            interestReportPM.CalculatedPostponedChequesCommision = GetInterestReportCalculatedPostponedChequesCommision();
            SetInterestReportStatusDraft();
            SubmitInterestReportLinesByDate(interestReportLinesByDatePMs);
            SubmitChangesToInterestReport();
        }

        private void SetGLAccountCreditAllotmentPercentageByGLAccountId(InterestReportPM interestReportPM)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(interestReportPM.Tenant);
            GLAccountPM account = gLAccountQueryService.GetSinglePM(interestReportPM.GLAccountId, interestReportPM.Tenant);
            interestReportPM.CreditAllotmentPercentage = account != null ? account.CreditAllotmentPercentage : null;
        }
        private DateTime GetFirstTransactionDate(InterestTransactionPM firstTransaction, CloseBalanceInterestReportData previousInterestReport)
        {
            if (previousInterestReport != null)
            {
                return firstTransaction.InterestValueDate.Date.AddDays(1);
            }
            else return firstTransaction.InterestValueDate.Date;
        }
        private void CreateOpenBalanceInterestTransaction()
        {
            CloseBalanceInterestReportData previousInterestReport = GetCloseBalanceCalculationDateAndStatusOfTheLastInterestReport();

            InterestTransactionPM firstTransaction = interestTransactionPMs.OrderBy(d => d.InterestValueDate).FirstOrDefault();
            DateTime previousInterestReportCalculationDate = GetOpenBalanceInterestValueDate(previousInterestReport);
           // DateTime date = firstTransaction? firstTransaction.InterestValueDate.Date;
            DateTime firstTransactionDate = firstTransaction!=null? GetFirstTransactionDate(firstTransaction, previousInterestReport):new DateTime();
          
            //if (OpenBalanceTransaction == null && (firstTransaction == null || previousInterestReportCalculationDate != firstTransactionDate))
            //{
                CreateOrUpdateInterestTransaction(previousInterestReportCalculationDate, previousInterestReport?.Id);
                //if (previousInterestReport != null)
                //{
                //    //if(previousInterestReport.InterestReportStatusCode == InterestReportStatusCodes.ClosedWithoutInvoice)
                //    {
                //        CreateNewInterestTransactionPM(previousInterestReportCalculationDate, previousInterestReport.Id);
                //    }
                //}
                //else
                //{
                //    CreateNewInterestTransactionPM(previousInterestReportCalculationDate);
                //}
            //}
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

        private void CreateOrUpdateInterestTransaction(DateTime previousInterestReportCalculationDate, string latestInterestReportId = null)
        {
            InterestTransactionPM openBalanceTransaction = GetOpenBalanceTransactionIfExisit(latestInterestReportId);
            if (openBalanceTransaction == null)
            {
                openBalanceTransaction = CreateNewInterestTransaction(previousInterestReportCalculationDate, latestInterestReportId);
            }
            else
            {
                UpdateCurrentOpenBalanceTransaction(previousInterestReportCalculationDate, openBalanceTransaction);
            }

            var isAdded = interestTransactionPMs.Any(d => d.Id == openBalanceTransaction.Id);
            if(!isAdded)
                interestTransactionPMs.Add(openBalanceTransaction);
        }

        private void UpdateCurrentOpenBalanceTransaction(DateTime previousInterestReportCalculationDate, InterestTransactionPM transaction)
        {
            transaction.InterestValueDate = previousInterestReportCalculationDate;
            transaction.LocalAmount = 0;
            transaction.ForeignAmount = 0;
            transaction.ChangeSetOp = ChangeSetOperation.Update;
            transaction.CurrencyId = GetLocalCurrency();
            transaction.Tenant = tenant;

            SubmitInterestTransaction(transaction);
        }

        private InterestTransactionPM CreateNewInterestTransaction(DateTime previousInterestReportCalculationDate, string latestInterestReportId)
        {
            InterestTransactionPM openBalanceInterestTransaction = new InterestTransactionPM()
            {
                EntityId = latestInterestReportId != null ? latestInterestReportId : interestReportId,
                InterestEntityTypeCode = InterestEntities.OpenBalance,
                InterestValueDate = previousInterestReportCalculationDate,
                GLAccountId = interestReportPM.GLAccountId,
                LocalAmount = 0,
                ForeignAmount = 0,
                OriginalEntityLineNumber = latestInterestReportId != null ? 2 : 1,
                ChangeSetOp = ChangeSetOperation.Insert,
                CurrencyId = GetLocalCurrency(),
                Tenant = tenant,
            };
            SubmitInterestTransaction(openBalanceInterestTransaction);
            return openBalanceInterestTransaction;
        }

        private void SubmitInterestTransaction(InterestTransactionPM openBalanceInterestTransaction)
        {
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            InterestTransactionUpdateService interestTransactionUpdateService = new InterestTransactionUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            interestTransactionUpdateService.Update(openBalanceInterestTransaction, true);
        }

        private string GetLocalCurrency()
        {
            TenantQuery tenantQuery = new TenantQuery(tenant);
            string localCurrencyId = tenantQuery.GetLocalCurrencyFromTenant(tenant);
            return localCurrencyId;
        }

        private InterestTransactionPM GetOpenBalanceTransactionIfExisit(string latestInterestReportId)
        {
            InterestTransactionQueryService interestTransactionQueryService = new InterestTransactionQueryService(tenant);
            var constraintFields = new InterestTransactionUniqueConstraintFields()
            {
                EntityId = latestInterestReportId != null ? latestInterestReportId : interestReportId,
                InterestEntityTypeCode = InterestEntities.OpenBalance,
                GLAccountId = interestReportPM.GLAccountId,
                OriginalEntityLineNumber = latestInterestReportId != null ? 2 : 1,
                Tenant = tenant
            };

            var transaction = interestTransactionQueryService.GetTransactionByUniqueConstraintFields(constraintFields);
            return transaction;
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

        private decimal? GetInterestReportCalculatedPostponedChequesCommision()
        {
            var aRPaymentIds = interestTransactionPMs.Where(x => x.InterestEntityTypeCode == InterestEntities.ARPayment).Select(x => x.EntityId).ToList();
            var gLAccount = GetGLAccount(interestReportPM.GLAccountId, interestReportPM.Tenant);

            int aRPaymentChequesCount = GetARPaymentChequesCountWithValueDateGreaterThanARPaymentRegisterDate(aRPaymentIds, interestReportPM.Tenant);
            decimal? postponedChequesCommision = gLAccount != null ? gLAccount.PostponedChequesCommission * aRPaymentChequesCount : null;

            return postponedChequesCommision;
        }

        private GLAccountPM GetGLAccount(string id, int tenant)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(interestReportPM.Tenant);
            GLAccountPM account = gLAccountQueryService.GetSinglePM(id, tenant);
            return account;
        }

        private int GetARPaymentChequesCountWithValueDateGreaterThanARPaymentRegisterDate(List<string> aRPaymentIds, int tenant)
        {
            ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(interestReportPM.Tenant);
            var aRPaymentChequesCount = aRPaymentChequeQueryService.GetARPaymentChequesCountWithValueDateGreaterThanARPaymentRegisterDate(aRPaymentIds, tenant);
            return aRPaymentChequesCount;
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
            decimal interestReportOpenBalance = interestReportQueryService.GetClosedBalanceOfLastInvoicedOrClosedWithoutInvoiceInterestReport(tenant, interestReportPM.GLAccountId);
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
            for (int i = 0; i < interestReportLinesByDatePMs.Count; i++)
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
            List<GLAccountInterestPeriodPM> gLAccountInterestPeriodPMs = interestReportCalculationPreparations.GetGlaccountInterestPeriods(interestReportPM);
            List<InterestBasesPeriodPM> interestBasesPeriodPMs = interestReportCalculationPreparations.GetAllInterestBasesPeriodPMs(tenant);
            DateTime? recentCalculationDate = GetRecentInterestReportCalculationDate();
            InterestReportLinesByDateCreationParams interestReportLinesByDateCreationParams = new InterestReportLinesByDateCreationParams(
                interestReportPM, interestTransactionPMs, gLAccountInterestPeriodPMs, interestBasesPeriodPMs, recentCalculationDate);
            List<InterestReportLinesByDatePM> interestReportLinesByDatePMs = interestReportLinesByDateCreationService.CreateInterestReportLinesByDate(interestReportLinesByDateCreationParams);
            return interestReportLinesByDatePMs;
        }

        private DateTime? GetRecentInterestReportCalculationDate()
        {
            InterestReportQueryService ReportQueryService = new InterestReportQueryService(tenant);
            DateTime? recentCalculationDate = ReportQueryService.GetRecentCustomerReports(interestReportPM);
            return recentCalculationDate;
        }

    }
}
