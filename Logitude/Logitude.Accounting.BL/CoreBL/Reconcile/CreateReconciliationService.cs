using Logitude.Accounting.BL.CoreBL.Reconcile;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Transactions;
using Newtonsoft.Json;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.Accounting.Data.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

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
                myReconciliationLinePM.ReconciliationAmount = 
                            currLedgerTrans.AmountToReconcile;
                myReconciliationLinePM.CurrencyRate = currLedgerTrans.ExchangeRate;

                myReconciliationLinePM.GroupNumber = currLedgerTrans.GroupMatch;

                myReconciliationPM.ReconciliationLines.Add(myReconciliationLinePM);
            }

            return myReconciliationPM;

        }
        private static Object thisLock = new Object();
        public RecoCallback CreateReconciliation(ReconciliationPM reconciliationPM, string exceptCommLogId = "")
        {
            // changeset
            if (reconciliationPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                throw new ApplicationException("Meanwhile Only Insert Enable ");
            }
            reconciliationPM.ChangeSetOp = ChangeSetOperation.Insert;
            foreach (var item in reconciliationPM.ReconciliationLines)
            {
                item.ChangeSetOp = ChangeSetOperation.Insert;
            }

            RecoCallback recoCallBack = new RecoCallback();
            if (!reconciliationPM.CreatedByReconciliationStageB)
            {
                using (TransactionScope scope = TransactionFactory.GetNewReadUncommittedTransaction())
                {
                    var ledgerTransactionInProgress = CheckIfAnotherReconciliationInProgress(reconciliationPM, exceptCommLogId);
                    if (ledgerTransactionInProgress)
                    {
                        throw new ApplicationException(TextCodesTranslator.TranslateText("GLAccounts.O.LedgerTransactionInProgress", 0, LoggedContactResolver.GetLoggedContactShowLocal(reconciliationPM.Tenant)));

                    }
                     var ledgerTransactionReconciled = CheckAnyLedgerTransactionReconciledByIdList(reconciliationPM);
                    if (ledgerTransactionReconciled)
                    {
                    throw new ApplicationException("GLAccounts.O.MarkedByAnother");

                    }
                   
                }
            }


            bool hasMultipleARPayments = false; 
            if (hasMultipleARPayments && reconciliationPM.ReconciliationLines.Any(d => d.GroupNumber == 0) && reconciliationPM.ReconciliationLines.Any(d => d.GroupNumber != 0))
            {
                    recoCallBack = SplitAndSubmitReconciliationByGroupNumberNonZero(reconciliationPM, hasMultipleARPayments);
            }
            else if (hasMultipleARPayments != true)
            {
                recoCallBack = SplitAndSubmitReconciliationByGroupNumber(reconciliationPM);
            }
            else
            {
                MultipleARPaymentReconciliationSplitter splitter = new MultipleARPaymentReconciliationSplitter(reconciliationPM);


                List<ReconciliationPM> paymentReconciliations = splitter.SplitReconciliationByPayment();



                SubmitReconciliations(reconciliationPM.Tenant, paymentReconciliations);
                recoCallBack = new RecoCallback() { isSplitted = true, splittedRecoCount = paymentReconciliations.Count };

            }
            if (!reconciliationPM.CreatedByReconciliationStageB)
            {
                var accountingContext = AccountingContext.GetContext(reconciliationPM.Tenant);
                var repoLedger = new LedgerTransactionRepository(accountingContext as IAccountingContext);
                repoLedger.ResetDraftOpenReconciliation(reconciliationPM.AccountId, reconciliationPM.Tenant);
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
            bool updateGLAccountAgingDataUsingWR = FeatureToggleHelper.HasFeatureToggle("UAD", tenant);
            var accountingContext = AccountingContext.GetContext(tenant);
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            service.updateGLAccountAgingDataUsingWR = updateGLAccountAgingDataUsingWR;
            foreach (ReconciliationPM recoPM in paymentReconciliations)
            {
                service.Update(recoPM, true);
                
            }
            if (updateGLAccountAgingDataUsingWR) {
                WriteEntityPMOnCommunicationLog(paymentReconciliations, tenant);
            }
        }

        private RecoCallback SplitAndSubmitReconciliationByGroupNumber(ReconciliationPM reconciliationPM)
        {
            bool updateGLAccountAgingDataUsingWR = FeatureToggleHelper.HasFeatureToggle("UAD", reconciliationPM.Tenant);
            if (reconciliationPM.CreatedByReconciliationStageB) updateGLAccountAgingDataUsingWR = false;
            var accountingContext = AccountingContext.GetContext(reconciliationPM.Tenant);
            RecoCallback recoCallBack;
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), reconciliationPM.Tenant);
            service.updateGLAccountAgingDataUsingWR = updateGLAccountAgingDataUsingWR;
            int groupsCount = reconciliationPM.ReconciliationLines.GroupBy(d => d.GroupNumber).Count();
            if (groupsCount > 1)
            {
                List<ReconciliationPM> recoPMs = SplitReconciliationByGroup(reconciliationPM);
                foreach (ReconciliationPM recoPM in recoPMs)
                {
                    service.Update(recoPM, true);
                }
                if (updateGLAccountAgingDataUsingWR)
                {
                    WriteEntityPMOnCommunicationLog(recoPMs, reconciliationPM.Tenant);
                }
                recoCallBack = new RecoCallback() { isSplitted = true, splittedRecoCount = recoPMs.Count };
            }
            else
            {
                service.Update(reconciliationPM, true);
                if (updateGLAccountAgingDataUsingWR)
                {
                    WriteEntityPMOnCommunicationLog(new List<ReconciliationPM>
                    {
                       reconciliationPM
                    }, reconciliationPM.Tenant);
                }
                recoCallBack = new RecoCallback(reconciliationPM);

            }

            return recoCallBack;
        }



        private RecoCallback SplitAndSubmitReconciliationByGroupNumberNonZero(ReconciliationPM reconciliationPM, bool hasMultipleARPayments)
        {
            bool updateGLAccountAgingDataUsingWR = FeatureToggleHelper.HasFeatureToggle("UAD", reconciliationPM.Tenant);
            var accountingContext = AccountingContext.GetContext(reconciliationPM.Tenant);
            RecoCallback recoCallBack;
            ReconciliationUpdateService service = new ReconciliationUpdateService(accountingContext, new Dictionary<string, IContext>(), reconciliationPM.Tenant);
            service.updateGLAccountAgingDataUsingWR = updateGLAccountAgingDataUsingWR;
            int groupsCount = reconciliationPM.ReconciliationLines.Where(d => d.GroupNumber != 0).GroupBy(d => d.GroupNumber).Count();
            if (groupsCount > 0)
            {
                List<ReconciliationPM> recoPMs = SplitReconciliationByGroupNonZero(reconciliationPM);
                foreach (ReconciliationPM recoPM in recoPMs)
                {
                    service.Update(recoPM, true);
                }

                recoCallBack = new RecoCallback() { isSplitted = true, splittedRecoCount = recoPMs.Count };


                if (hasMultipleARPayments)
                {
                    MultipleARPaymentReconciliationSplitter splitter = new MultipleARPaymentReconciliationSplitter(reconciliationPM);


                    List<ReconciliationPM> paymentReconciliations = splitter.SplitReconciliationByPaymentZeroGroup();



                    SubmitReconciliations(reconciliationPM.Tenant, paymentReconciliations);
                    recoCallBack.splittedRecoCount += paymentReconciliations.Count;
                }
                if (updateGLAccountAgingDataUsingWR)
                {
                    WriteEntityPMOnCommunicationLog(recoPMs, reconciliationPM.Tenant);
                }

            }
            else
            {
                // should not get there
                service.Update(reconciliationPM, true);
                if (updateGLAccountAgingDataUsingWR)
                {
                    WriteEntityPMOnCommunicationLog(new List<ReconciliationPM>
                    {
                       reconciliationPM
                    }, reconciliationPM.Tenant);
                }
                recoCallBack = new RecoCallback(reconciliationPM);

            }

            return recoCallBack;
        }


        private void WriteEntityPMOnCommunicationLog(List<ReconciliationPM> reconciliations, int tenant)
        {
            string jsonString = JsonConvert.SerializeObject(reconciliations);
            byte[] xmlFile = Encoding.UTF8.GetBytes(jsonString);
            string communicationLogId = Communications.AddCommunicationLog(new CommunicationsParams()
            {
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                Priority = 1,
                InOut = "O",
                Status = "W",
                Subject = "Update GLAccount Aging Data",
                FolderName = "Other",
                ByteData = xmlFile
            });
            var queueService = new DbQueueService();
            if (FeatureToggleHelper.HasFeatureToggle("JAM", tenant))
            {
                queueService.InitializeQueue("AccountingJournalApproveMutliThreadingWR", tenant);
            }
            else {
                queueService.InitializeQueue("AccountingJournalApproveWR", tenant);
            }
            queueService.Send(new Dictionary<string, string>() { { "tenant", tenant.ToString() }, { "communicationLogId", communicationLogId } }, tenant, null, null);
        }

        private static bool CheckIfHasMultiplePayment(ReconciliationPM reconciliationPM, List<LedgerTransactionPM> recoTransactions)
        {
            bool hasMultipleARPayments = recoTransactions.Count(d => d.SourceTypeCode == AccountingEntities.ARPayment) > 1;
            return hasMultipleARPayments;
        }


        private static bool CheckIfHasMultiplePaymentsLT(List<LedgerTransactionJournalLineLT> recoTransactions)
        {
            bool hasMultipleARPayments = recoTransactions.Count(d => d.SourceTypeCode == AccountingEntities.ARPayment) > 1;
            return hasMultipleARPayments;
        }


        public List<LedgerTransactionJournalLineLT> GetLedgerTransactionJournalLineLTsByIdList(List<string> transactionIdList, int tenant)
        {
            var a = new LedgerTransactionQueryService(tenant);
            return a.GetLedgerTransactionJournalLineLTsByIdList(transactionIdList, tenant);
        }

        private static bool CheckAnyLedgerTransactionReconciledByIdList(ReconciliationPM reconciliationPM)
        {
            List<string> transactionsIds = reconciliationPM.ReconciliationLines.Select(d => d.TransactionId).ToList();
            LedgerTransactionQueryService transactionQueryService = new LedgerTransactionQueryService(reconciliationPM.Tenant);
            return transactionQueryService.CheckAnyLedgerTransactionReconciledByIdList(transactionsIds, reconciliationPM.Tenant);
        }
        private bool CheckIfAnotherReconciliationInProgress(ReconciliationPM entityPm, string exceptCommLogId = "")
        {
            ICommonDataContext context = CommonDataContext.GetContext(entityPm.Tenant);
            CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
            CommunicationLog commLog = communicationLogRep.GetCommunicationLogByEntityIdAndSubject(entityPm.AccountId, "Create internal Reconciliation", entityPm.Tenant, exceptCommLogId, "W");
            if (commLog != null)
            {
                return true;
            }
            return false;
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


        private List<ReconciliationPM> SplitReconciliationByGroupNonZero(ReconciliationPM originalRecoPM)
        {
            List<ReconciliationPM> recoPMs = new List<ReconciliationPM>();

            List<IGrouping<int, ReconciliationLinePM>> groups = originalRecoPM.ReconciliationLines.Where(d => d.GroupNumber != 0).GroupBy(d => d.GroupNumber).ToList();

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
        

        public bool isSplitted = false;
        public int splittedRecoCount = 0;
        public string communicationLogId;
    }
}
