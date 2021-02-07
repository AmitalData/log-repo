using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Resolvers;
using Simplog.Data.Helpers;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.Def.BLExt;
using Microsoft.Practices.Unity;
using Logitude.Accounting.BL.CoreBL.ExternalReconcile;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public partial class ExternalReconciliationUpdateService
   {
        protected override void OnCreating(ExternalReconciliationPM entityPM, EntityPM entityParentPM)
        {
            //Create ID
            if (entityPM.Id == null || entityPM.Id == "" || entityPM.Id == "new")
                entityPM.Id = IdCounter.GetNumber("ExternalReconciliation", entityPM.Tenant);

            //Create Code Number
            entityPM.ReconciliationNumber = CodeCounter.GetNumber("ExternalReconciliation", entityPM.Tenant);

            //Fill created by fields
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
            entityPM.CreateDate = DateTime.Now;
            entityPM.CreatedByUserId = loggedContact.Id;

            //Update line parent id
            List<string> ledgerIds = new List<string>();
            foreach (var line in entityPM.ExternalReconciliationLines)
            {
                line.ReconciliationId = entityPM.Id;
                ledgerIds.Add(line.LedgerTransactionId);
            }

            base.OnCreating(entityPM, entityParentPM);
        }
        

        protected override void OnUpdating(ExternalReconciliationPM externalRecoPM)
        {

            if (externalRecoPM.ChangeSetOp == ChangeSetOperation.Insert)
            {

                List<LedgerTransactionPM> reconciliationLedgerTransactions = GetLedgerTransactionsOfExternalReconcile(externalRecoPM);

                SetTransactionsAsExternallyReconciled(externalRecoPM.Tenant, reconciliationLedgerTransactions);
                SetExtenalPageAsReconciled(externalRecoPM);

                RedeemARPaymentCheques(reconciliationLedgerTransactions);
                RedeemPaymentCheques(reconciliationLedgerTransactions);                

            }
        }

        private void SetExtenalPageAsReconciled(ExternalReconciliationPM externalRecoPM)
        {
            List<ReconcileExternalPageLinePM> PageLines = GetExternalPageLinesOfExternalReconcile(externalRecoPM);

            ReconcileExternalPageLineUpdateService pageLineService = new ReconcileExternalPageLineUpdateService(MainContext, AdditionalContexts, externalRecoPM.Tenant);
            foreach (var pageLinePM in PageLines)
            {
                pageLinePM.ChangeSetOp = ChangeSetOperation.Update;
                pageLinePM.IsReconciled = true;
                pageLineService.Update(pageLinePM, false);
            }
        }

        private void RedeemPaymentCheques(List<LedgerTransactionPM> LedgerTransactions)
        {
            foreach (var transactionPM in LedgerTransactions)
            {
                if (transactionPM.SourceTypeCode == AccountingEntityValues.APPayment)
                {
                    List<PaymentChequePM> paymentCheques = GetTransactionPaymentCheques(transactionPM);
                    if (paymentCheques.Count > 0)
                        UpdatePaymentChequeStatus(paymentCheques.FirstOrDefault());
                }
            }
        }

        private static List<PaymentChequePM> GetTransactionPaymentCheques(LedgerTransactionPM transactionPM)
        {
            PaymentChequeQueryService paymentChequeQuery = new PaymentChequeQueryService(transactionPM.Tenant);
            List<PaymentChequePM> paymentCheques = paymentChequeQuery.GetPaymentChequesByPaymentId(transactionPM.SourceId, transactionPM.Tenant);
            return paymentCheques;
        }

        private void RedeemARPaymentCheques(List<LedgerTransactionPM> ledgerTransactions)
        {
            foreach (var transaction in ledgerTransactions)
            {
                List<ARPaymentChequePM> arpaymentCheques = GetARPChequesFromTransaction(transaction);
                SetARPaymentChequesAsReedemed(arpaymentCheques);
            }
        }

        private void SetARPaymentChequesAsReedemed(List<ARPaymentChequePM> arpaymentCheques)
        {
            foreach (var arpaymentCheque in arpaymentCheques)
            {
                if (arpaymentCheque != null && (arpaymentCheque.StatusCode == ARPaymentChequeStatusValues.InBank || arpaymentCheque.StatusCode == ARPaymentChequeStatusValues.InBankAccount))
                    SetARPaymentChequeAsRedeemed(arpaymentCheque);
            }
        }

        private void SetTransactionsAsExternallyReconciled(int tenant, List<LedgerTransactionPM> LedgerTransactions)
        {
            foreach (var transactionPM in LedgerTransactions)
            {
                transactionPM.ChangeSetOp = ChangeSetOperation.Update;
                transactionPM.IsExternalReconcile = true;

                LedgerTransactionUpdateService transactionService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, tenant);
                transactionService.Update(transactionPM, false);
            }
        }

        private List<ARPaymentChequePM> GetARPChequesFromTransaction(LedgerTransactionPM transactionPM)
        {
            List<ARPaymentChequePM> arpaymentCheques = null;

            if (transactionPM.SourceTypeCode == AccountingEntityValues.ChequeDeposit)
                arpaymentCheques = GetChequeOfDepositTransaction(transactionPM.Reference2, transactionPM.SourceId, transactionPM.Tenant);

            if (transactionPM.SourceTypeCode == AccountingEntityValues.ARPayment)
                arpaymentCheques = GetChequeOfARPaymentTransaction(transactionPM.Reference2, transactionPM.SourceId, transactionPM.Tenant);

            return arpaymentCheques;
        }

        private List<ARPaymentChequePM> GetChequeOfDepositTransaction(string transactionReference, string depositId, int tenant)
        {
            List<ARPaymentChequePM> arpaymentCheques = GetChequesOfDeposit(tenant, depositId);

            arpaymentCheques = arpaymentCheques.Where(a => a.ChequeNumber == transactionReference).ToList();
            return arpaymentCheques;
        }

        private List<ARPaymentChequePM> GetChequeOfARPaymentTransaction(string transactionReference, string paymentId, int tenant)
        {
            ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(tenant);
            List<ARPaymentChequePM> aRPaymentChequePMs = aRPaymentChequeQueryService.GetInBankAccountChequesByPaymentId(paymentId, tenant);

            aRPaymentChequePMs = aRPaymentChequePMs.Where(a => a.ChequeNumber == transactionReference).ToList();
            return aRPaymentChequePMs;
        }


        private static List<ARPaymentChequePM> GetChequesOfDeposit(int tenant, string depositId)
        {
            BankDepositQueryService bankDepositQueryService = new BankDepositQueryService(tenant);
            List<ARPaymentChequePM> aRPaymentChequePMs = bankDepositQueryService.GetChequesOfDeposit(depositId, tenant);
            return aRPaymentChequePMs;
        }

        private List<ReconcileExternalPageLinePM> GetExternalPageLinesOfExternalReconcile(ExternalReconciliationPM externalRecoPM)
        {
            ReconcileExternalPageLineQueryService pageLineQuery = new ReconcileExternalPageLineQueryService(externalRecoPM.Tenant);
            List<string> PageLineIds = EntityPM.ExternalReconciliationLines.Where(d => d.ExternalPageLineId != null).Select(d => d.ExternalPageLineId).ToList();
            List<ReconcileExternalPageLinePM> PageLines = pageLineQuery.GetPageLinesPMsByIdList(PageLineIds, externalRecoPM.Tenant);
            return PageLines;
        }

        private static List<LedgerTransactionPM> GetLedgerTransactionsOfExternalReconcile(ExternalReconciliationPM externalRecoPM)
        {
            LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(externalRecoPM.Tenant);
            List<string> LedgerTransactionIds = externalRecoPM.ExternalReconciliationLines.Where(d => d.LedgerTransactionId != null).Select(d => d.LedgerTransactionId).ToList();
            List<LedgerTransactionPM> LedgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(LedgerTransactionIds, externalRecoPM.Tenant);
            return LedgerTransactions;
        }

        private void SetARPaymentChequeAsRedeemed(ARPaymentChequePM aRPaymentCheque)
        {
            aRPaymentCheque.StatusCode = ARPaymentChequeStatusValues.Redeemed;
            aRPaymentCheque.ChangeSetOp = ChangeSetOperation.Update;

            SubmitCheque(aRPaymentCheque);
        }

        private void SubmitCheque(ARPaymentChequePM aRPaymentCheque)
        {
            ARPaymentChequeUpdateService aRPaymentChequeUpdateService = new ARPaymentChequeUpdateService(MainContext, AdditionalContexts, aRPaymentCheque.Tenant);
            aRPaymentChequeUpdateService.Update(aRPaymentCheque, true);
        }

        private void UpdatePaymentChequeStatus(PaymentChequePM chequePM)
        {
            if (chequePM != null)
            {
                chequePM.PaymentChequeStatusCode = "3"; // 3- Redeemed
                chequePM.ChangeSetOp = ChangeSetOperation.Update;
                PaymentChequeUpdateService paymentChequeUpdateService = new PaymentChequeUpdateService(MainContext, AdditionalContexts, chequePM.Tenant);
                paymentChequeUpdateService.Update(chequePM, true);
            }

        }
        protected override void OnUpdating(ExternalReconciliationPM entityPM, ExternalReconciliation entityPOCO)
        {

            IAccountingContext accountingContext = AccountingContext.GetContext(entityPM.Tenant);
            if (entityPOCO.IsCancelled == false && entityPM.IsCancelled == true)
            {
                // canceled!!
                // Update for each transaction(ReconciliationLines.TransactionId): IsExternalReconcile.LegderTransactions = False
                UpdateLedgerTransactions(entityPM);

                // Update for each bank transaction(In table ReconcileExternalPageLines): IsReconciled=False
                UpdateBankPages(entityPM);

                // change payment cheque status
                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
                PaymentChequeQueryService paymentChequeQuery = new PaymentChequeQueryService(entityPM.Tenant);
                BankDepositQueryService  depositQuery = new BankDepositQueryService(entityPM.Tenant);
                ARPaymentChequeQueryService  arpChequeQuery = new ARPaymentChequeQueryService(entityPM.Tenant);
                ARPaymentChequeUpdateService arpChequeUpdateService = new ARPaymentChequeUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);

                List<string> LedgerTransactionIds = entityPM.ExternalReconciliationLines.Where(d => d.LedgerTransactionId != null).Select(d => d.LedgerTransactionId).ToList();
                List<LedgerTransactionPM> LedgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(LedgerTransactionIds, entityPM.Tenant);
                foreach (var transactionPM in LedgerTransactions)
                {
                    if (transactionPM.SourceTypeCode == "9") // 9- Payment Cheque
                    {
                        PaymentChequePM chequePM = paymentChequeQuery.GetSingle(transactionPM.SourceId, true, false);
                        chequePM.PaymentChequeStatusCode = "2"; // 2- Approved
                        chequePM.ChangeSetOp = ChangeSetOperation.Update;
                        PaymentChequeUpdateService paymentChequeUpdateService = new PaymentChequeUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                        paymentChequeUpdateService.Update(chequePM, true);
                    }
                    else if(transactionPM.SourceTypeCode == "6") // 6- Cheque Deposit
                    {
                        BankDepositPM depositPM = depositQuery.GetSingle(transactionPM.SourceId, true, false);

                        foreach (BankDepositLinePM depLine in depositPM.BankDepositLines)
                        {

                            if (transactionPM.Reference1 == depLine.ChequeNumber)
                            {
                                ARPaymentChequePM chequePM = arpChequeQuery.GetSingle(depLine.ARPaymentChequeId, false, false);
                                chequePM.ChangeSetOp = ChangeSetOperation.Update;
                                chequePM.StatusCode = "3"; // 3- in bank account
                                arpChequeUpdateService.Update(chequePM, true);
                            }

                        }
                    }
                }

            }


            base.OnUpdating(entityPM, entityPOCO);
        }
        protected override void UpdateComposition(ExternalReconciliationPM entityPM)
        {
            ExternalReconciliationLineUpdateService lineUpdateService = new ExternalReconciliationLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            lineUpdateService.UpdateMulti(entityPM.ExternalReconciliationLines, entityPM.DeletedExternalReconciliationLines, entityPM, false);
            base.UpdateComposition(entityPM);
        }
        protected override void Trace(ExternalReconciliationPM entityPM, ExternalReconciliation entityPOCO, string changesXml)
        {
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                //create trace event with created type.
                EventTracerArgs eventTracerArgs = new EventTracerArgs()
                {
                    EntityId = entityPM.Id,
                    Tenant = entityPM.Tenant,
                    UserId = loggedContact.Id,
                    ObjectTableName = "ExternalReconciliation",
                    IsAddedManually = false,
                    EventTypeCode = "CREV",
                    Notes = "",
                };
                EventTracer.CreateTraceEvent(eventTracerArgs);
            }
            else
            {
                if(entityPM.IsCancelled != entityPOCO.IsCancelled)
                {
                    //create trace event with created type.
                    EventTracerArgs eventTracerArgs = new EventTracerArgs()
                    {
                        EntityId = entityPM.Id,
                        Tenant = entityPM.Tenant,
                        UserId = loggedContact.Id,
                        ObjectTableName = "ExternalReconciliation",
                        IsAddedManually = false,
                        EventTypeCode = "ERCN",
                        Notes = "",
                    };
                    EventTracer.CreateTraceEvent(eventTracerArgs);
                }
            }

            base.Trace(entityPM, entityPOCO, changesXml);
        }
        protected override void Validate(ExternalReconciliationPM externalRecoPM)
        {

            CheckDifferenc(externalRecoPM);
            CheckTransferTransactions(externalRecoPM);

            base.Validate(externalRecoPM);
        }

        private static void CheckTransferTransactions(ExternalReconciliationPM externalRecoPM)
        {
            BankAccountPM bankAccount = GetBankAccountForExternalReconciliation(externalRecoPM);

            if (bankAccount != null && bankAccount.GLAccountId != bankAccount.TransferGLAcccountId)
            {
                bool haveExternalPageLines = externalRecoPM.ExternalReconciliationLines.Count(d => d.LedgerTransactionId == null && d.ExternalPageLineId != null) > 0;
                int transferTransactionsCount = externalRecoPM.ExternalReconciliationLines.Count(d => d.LedgerGLAccountId == bankAccount.TransferGLAcccountId);

                if (haveExternalPageLines && transferTransactionsCount > 1)
                {
                    var msg = TextCodesTranslator.TranslateText("ExternalReconciliation.O.CantReconcileTwoTransfer", 0,
                        LoggedContactResolver.GetLoggedContactShowLocal(externalRecoPM.Tenant));
                    throw new ApplicationException(msg);
                }
            }

        }

        private static BankAccountPM GetBankAccountForExternalReconciliation(ExternalReconciliationPM externalRecoPM)
        {
            BankAccountPM bankAccount;
            if (externalRecoPM.BankAccountId != null)
                bankAccount = GetBankAccount(externalRecoPM);
            else
            {
                bankAccount = GetBankAccountByTransferGLaccount(externalRecoPM.GLAccountId, externalRecoPM.Tenant);
            }

            return bankAccount;
        }

        private static BankAccountPM GetBankAccount(ExternalReconciliationPM externalRecoPM)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(externalRecoPM.Tenant);
            BankAccountPM bankAccount = bankAccountQueryService.GetSingle(externalRecoPM.BankAccountId, false, false);
            return bankAccount;
        }
        private static BankAccountPM GetBankAccountByTransferGLaccount(string transferGLAccountId, int tenant)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(tenant);
            BankAccountPM bankAccount = bankAccountQueryService.GetBankAccountByTransferGLAcccountId(transferGLAccountId, tenant);
            return bankAccount;
        }

        void CheckDifferenc(ExternalReconciliationPM entityPM)
        {
            decimal ledgerLinesTotal = GetLedgerTransactionsTotal(entityPM);
            decimal bankLinesTotal = GetBankPageLinesTotal(entityPM);

            //decimal totalDifference = Math.Abs(bankLinesTotal - ledgerLinesTotal);
            decimal totalDifference = Math.Abs(bankLinesTotal + ledgerLinesTotal);

            if (totalDifference != 0)
            {
                ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);
                bool showLocal = !loggedContact.DontShowLocal;

                var msg = TranslateTextsClass.Translate("Accounting.General.O.DifferenceMustEqual0", 0, showLocal);
                throw new ApplicationException(msg);
            }

        }

        private decimal GetBankPageLinesTotal(ExternalReconciliationPM entityPM)
        {
            List<ReconcileExternalPageLine> pageLines = GetBankPagesLinesForReconciliations(entityPM);

            decimal BankLinesSum = 0;
            BankLinesSum = pageLines.Sum(a => a.CreditAmount * -1);
            BankLinesSum = BankLinesSum + pageLines.Sum(a => a.DebitAmount);
            return BankLinesSum;
        }

        private List<ReconcileExternalPageLine> GetBankPagesLinesForReconciliations(ExternalReconciliationPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            ExternalReconciliationQueryService query = new ExternalReconciliationQueryService(context);
            List<string> listOfBankLinesIds = entityPM.ExternalReconciliationLines.FindAll(d => d.ExternalPageLineId != null).Select(d => d.ExternalPageLineId).ToList();
            List<ReconcileExternalPageLine> pageLines = query.GetBankPagesByIds(listOfBankLinesIds, entityPM.Tenant);
            return pageLines;
        }

        private decimal GetLedgerTransactionsTotal(ExternalReconciliationPM entityPM)
        {
            decimal LedgerLinesSum = 0;
            List<LedgerTransaction> ledgerLines = GetLedgerTransactionsForReconciliationLines(entityPM);
            LedgerLinesSum = ledgerLines.Sum(a => a.ForeignAmountDebit);
            LedgerLinesSum -= ledgerLines.Sum(a => a.ForeignAmountCredit);
            return LedgerLinesSum;
        }

        private List<LedgerTransaction> GetLedgerTransactionsForReconciliationLines(ExternalReconciliationPM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            ExternalReconciliationQueryService query = new ExternalReconciliationQueryService(context);
            List<string> listOfLedgerLinesIds = entityPM.ExternalReconciliationLines.FindAll(d => d.LedgerTransactionId != null).Select(d => d.LedgerTransactionId).ToList();
            List<LedgerTransaction> ledgerLines = query.GetLedgerTransactionByIds(listOfLedgerLinesIds, entityPM.Tenant);
            return ledgerLines;
        }

        private void UpdateLedgerTransactions(ExternalReconciliationPM entityPM)
        {
            var transactionIdList = entityPM.ExternalReconciliationLines.Select(rec => rec.LedgerTransactionId).ToList();
            var qs = new LedgerTransactionQueryService(entityPM.Tenant);
            var LedgerTransactionPMsUpdated = qs.GetLedgerTransactionPMsByIdList(transactionIdList, entityPM.Tenant);

            foreach (var line in LedgerTransactionPMsUpdated)
            {
                line.ChangeSetOp = ChangeSetOperation.Update;
                line.IsExternalReconcile = false;
            }
            var ledgerTransactionUpdateService = new LedgerTransactionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            ledgerTransactionUpdateService.UpdateMulti(LedgerTransactionPMsUpdated, new List<LedgerTransactionPM>(), entityPM, false);
        }
        private void UpdateBankPages(ExternalReconciliationPM entityPM)
        {
            var pageLinesIdList = entityPM.ExternalReconciliationLines.Select(rec => rec.ExternalPageLineId).ToList();
            var qs = new ReconcileExternalPageLineQueryService(entityPM.Tenant);
            var pageLinesPMsUpdated = qs.GetPageLinesPMsByIdList(pageLinesIdList, entityPM.Tenant);

            foreach (var line in pageLinesPMsUpdated)
            {
                line.ChangeSetOp = ChangeSetOperation.Update;
                line.IsReconciled = false;
            }
            var reconcileExternalPageLineUpdateService = new ReconcileExternalPageLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            reconcileExternalPageLineUpdateService.UpdateMulti(pageLinesPMsUpdated, new List<ReconcileExternalPageLinePM>(), new ReconcileExternalPagePM(), false);
        }


        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }




        public static ContactPM GetLoggedContact(int tenant)
        {
            if (OverrideGetLoggedContactFunc != null)
            {
                return OverrideGetLoggedContactFunc(tenant);
            }

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }

}
	 