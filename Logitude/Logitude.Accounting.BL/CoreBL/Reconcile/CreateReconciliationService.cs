using Logitude.Accounting.BL.CoreBL.Reconcile;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Logitude.Accounting.BL.CoreBL
{
    public class CreateReconciliationService
    {

        public ReconciliationPM GetReconciliation(List<LedgerTransactionPM> myMatchLedgerTransactionList)
        {
            LedgerTransactionPM firstMatch = myMatchLedgerTransactionList[0];
            var myReconciliationPM = new ReconciliationPM();
            myReconciliationPM.Id = "new";
            myReconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
            myReconciliationPM.Tenant = firstMatch.Tenant;
            myReconciliationPM.AccountId = firstMatch.AccountId;
            myReconciliationPM.Number = "get";
            myReconciliationPM.CreateDate = DateTime.UtcNow;
            myReconciliationPM.CreatedByUserId = null;
            myReconciliationPM.CreatedByUserId = null;

            myReconciliationPM.ReconciliationLines = new List<ReconciliationLinePM>();

            for (var i = 0; i < myMatchLedgerTransactionList.Count; i++)
            {
                var currLedgerTrans = myMatchLedgerTransactionList[i];
                var myReconciliationLinePM = new ReconciliationLinePM();
                myReconciliationLinePM.ChangeSetOp = ChangeSetOperation.Insert; ;
                myReconciliationLinePM.ReconciliationId = myReconciliationPM.Id;
                myReconciliationLinePM.Tenant = myReconciliationPM.Tenant;
                myReconciliationLinePM.Line = i;
                myReconciliationLinePM.CurrencyId = currLedgerTrans.OpenAmountCurrencyId;
                myReconciliationLinePM.TransactionId = currLedgerTrans.Id;
                myReconciliationLinePM.ReconciliationAmount = //currLedgerTrans.OpenAmount;
                            currLedgerTrans.AmountToReconcile;

                //ReconciliationLinePM.IsPartial = currLedgerTrans.OpenAmount;
                myReconciliationLinePM.GroupNumber = currLedgerTrans.GroupMatch;

                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);
            }

            return myReconciliationPM;

        }

        public RecoCallback CreateReconciliation(ReconciliationPM reconciliationPM)
        {

            // changeset
            if (reconciliationPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                throw new Exception("Meanwhile Only Insert Enable ");
            }
            reconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
            foreach (var item in reconciliationPM.ReconciliationLines)
            {
                item.ChangeSetOp = ChangeSetOperation.Insert;
            }

            RecoCallback recoCallBack = new RecoCallback();


            List<LedgerTransactionPM> recoTransactions = GetReconcileTransactions(reconciliationPM);

            bool hasTwoPaymentsOnly = (recoTransactions.Count(d => d.SourceTypeCode == AccountingEntities.ARPayment) == 2) && recoTransactions.TrueForAll(d => d.SourceTypeCode == AccountingEntities.ARPayment);
            bool hasMultipleARPayments = CheckIfHasMultiplePayment(reconciliationPM, recoTransactions);
            if (hasMultipleARPayments == true && !hasTwoPaymentsOnly)
            {
                CheckIfReconcilePaymentOnly(reconciliationPM, recoTransactions);

                CheckIfTotalNotEqualsZero(reconciliationPM);

                MultipleARPaymentReconciliationSplitter splitter = new MultipleARPaymentReconciliationSplitter(reconciliationPM);
                List<ReconciliationPM> paymentReconciliations = splitter.SplitReconciliationByPayments();

                SubmitReconciliations(reconciliationPM.Tenant, paymentReconciliations);
                recoCallBack = new RecoCallback() { isSplitted = true, splittedRecoCount = paymentReconciliations.Count };

            }
            else
            {
                recoCallBack = SplitAndSubmitReconciliationByGroupNumber(reconciliationPM);
            }





            return recoCallBack;
        }

        private static void CheckIfReconcilePaymentOnly(ReconciliationPM reconciliationPM, List<LedgerTransactionPM> recoTransactions)
        {
            bool allTransactionsIsPayments = recoTransactions.TrueForAll(d => d.SourceTypeCode == AccountingEntities.ARPayment);
            if (allTransactionsIsPayments == true)
            {
                var msg = TextCodesTranslator.TranslateText("ARPayment.O.AllPaymentsReconciliation", reconciliationPM.Tenant, LoggedContactResolver.GetLoggedContactShowLocal(reconciliationPM.Tenant));
                throw new ApplicationException(msg);
            }
        }

        private void SubmitReconciliations(int tenant, List<ReconciliationPM> paymentReconciliations)
        {
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            foreach (ReconciliationPM recoPM in paymentReconciliations)
            {
                service.Update(recoPM, true);
            }
        }

        private RecoCallback SplitAndSubmitReconciliationByGroupNumber(ReconciliationPM reconciliationPM)
        {
            var accountingContext = AccountingContext.GetContext(reconciliationPM.Tenant);
            RecoCallback recoCallBack;
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), reconciliationPM.Tenant);
            int groupsCount = reconciliationPM.ReconciliationLines.GroupBy(d => d.GroupNumber).Count();
            if (groupsCount > 1)
            {
                List<ReconciliationPM> recoPMs = SplitReconciliationByGroup(reconciliationPM);
                foreach (ReconciliationPM recoPM in recoPMs)
                {
                    service.Update(recoPM, true);
                }

                recoCallBack = new RecoCallback() { isSplitted = true, splittedRecoCount = recoPMs.Count };
            }
            else
            {
                service.Update(reconciliationPM, true);
                recoCallBack = new RecoCallback(reconciliationPM);

            }

            return recoCallBack;
        }

        private void CheckIfTotalNotEqualsZero(ReconciliationPM reconciliationPM)
        {
            decimal reconciliaionTotal = reconciliationPM.ReconciliationLines.Sum(d => d.ReconciliationAmount);
            if (reconciliaionTotal != 0)
            {
                var msg = TextCodesTranslator.TranslateText("ARPayment.O.MultiPaymentZeroDifference", reconciliationPM.Tenant, LoggedContactResolver.GetLoggedContactShowLocal(reconciliationPM.Tenant));
                throw new ApplicationException(msg);
            }
        }

        private static bool CheckIfHasMultiplePayment(ReconciliationPM reconciliationPM, List<LedgerTransactionPM> recoTransactions)
        {
            bool hasMultipleARPayments = recoTransactions.Count(d => d.SourceTypeCode == AccountingEntities.ARPayment) > 1;
            return hasMultipleARPayments;
        }

        private static List<LedgerTransactionPM> GetReconcileTransactions(ReconciliationPM reconciliationPM)
        {
            List<string> transactionsIds = reconciliationPM.ReconciliationLines.Select(d => d.TransactionId).ToList();
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(reconciliationPM.Tenant);
            List<LedgerTransactionPM> recoTransactions = transactionQueryService.GetLedgerTransactionPMsByIdList(transactionsIds, reconciliationPM.Tenant);
            return recoTransactions;
        }

        private List<ReconciliationPM> SplitReconciliationByGroup(ReconciliationPM originalRecoPM)
        {
            List<ReconciliationPM> recoPMs = new List<ReconciliationPM>();

            List<IGrouping<int, ReconciliationLinePM>> groups = originalRecoPM.ReconciliationLines.GroupBy(d => d.GroupNumber).ToList();

            foreach (IGrouping<int, ReconciliationLinePM> group in groups)
            {
                //header
                ReconciliationPM recoPM = new ReconciliationPM()
                {
                    Tenant = originalRecoPM.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,

                    AccountId = originalRecoPM.AccountId,
                    CreatedByUserId = originalRecoPM.CreatedByUserId,
                    Number = originalRecoPM.Number,
                    CreateDate = originalRecoPM.CreateDate,
                    SearchFields = originalRecoPM.SearchFields,
                    IsCancelled = originalRecoPM.IsCancelled,
                    CurrencyCode = originalRecoPM.CurrencyCode,
                    AccountName = originalRecoPM.AccountName,
                    AccountNumber = originalRecoPM.AccountNumber,
                    CreatedByUserName = originalRecoPM.CreatedByUserName,
                };

                //lines
                List<ReconciliationLinePM> groupLines = group.ToList();
                foreach (ReconciliationLinePM line in groupLines)
                {
                    line.ChangeSetOp = ChangeSetOperation.Insert;
                    recoPM.ReconciliationLines.Add(line);
                }

                //add to list
                recoPMs.Add(recoPM);
            }

            return recoPMs;
        }



    }

    public class RecoCallback
    {
        public RecoCallback(ReconciliationPM reco = null)
        {
            if (reco != null)
                reconciliationPM = reco;
        }


        public ReconciliationPM reconciliationPM;
        //public List<ReconciliationPM> splittedRecoPMs;

        public bool isSplitted = false;
        public int splittedRecoCount = 0;
    }
}
