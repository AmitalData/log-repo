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
            HandleTransferAccountTransactions(entityPM);

            base.OnCreating(entityPM, entityParentPM);
        }

        private void HandleTransferAccountTransactions(ExternalReconciliationPM externalRecoPM)
        {
            List<ExternalReconciliationLinePM> transferRecoLines = getRecoLinesOfTransferAccount(externalRecoPM, externalRecoPM.Tenant);

            if (transferRecoLines.Count > 1) throw new ApplicationException("for now, you can select only one transaction for transfer account");

            for (int i = 0; i < transferRecoLines.Count; i++)
            {
                MoveTransactionFromTransferGLAccountToBankGLAccount(externalRecoPM, transferRecoLines[i], i);
            }

        }

        private void MoveTransactionFromTransferGLAccountToBankGLAccount(ExternalReconciliationPM externalRecoPM, ExternalReconciliationLinePM recoLine, int iteration)
        {
            LedgerTransactionPM ledgerTransactionForTransferAccount = GetLedgerTransactionById(recoLine.LedgerTransactionId, externalRecoPM.Tenant);

            JournalPM journalPM = CreateJournalForTransferAccountTransaction(externalRecoPM, iteration + 1, ledgerTransactionForTransferAccount);

            BuildInternalReconcile(externalRecoPM, ledgerTransactionForTransferAccount, journalPM);
            BuildExternalReconciliationForTransferTransaction(externalRecoPM, ledgerTransactionForTransferAccount, journalPM);

        }

        private void BuildExternalReconciliationForTransferTransaction(ExternalReconciliationPM externalRecoPM, LedgerTransactionPM ledgerTransactionForTransferAccount, JournalPM journalPM)
        {
            var autoExternalReconcileService = new CreateAutoExternalReconcileWhileStreamingService();
            var accountingContext = AccountingContext.GetContext(externalRecoPM.Tenant);
            var providor = new ExternalReconcileDataProvider(accountingContext);

            ExternalReconciliationLinePM pageLine = externalRecoPM.ExternalReconciliationLines.FirstOrDefault(d => d.ExternalPageLineId != null);

            List<LedgerTransactionPM> transactions = new List<LedgerTransactionPM> { ledgerTransactionForTransferAccount };

            journalPM.JournalExternalReconciles.Add(new JournalExternalReconcilePM()
            {
                Tenant = externalRecoPM.Tenant,
                JournalId = journalPM.Id,
                LedgerTransactionId = ledgerTransactionForTransferAccount.Id,
                ReconcileExternalPageLineId = pageLine.ExternalPageLineId,
            });

            autoExternalReconcileService.MustInit(providor, journalPM, transactions);
            autoExternalReconcileService.CreateAutoExternalReconcileWhileStreaming();
        }

        private List<ExternalReconciliationLinePM> getRecoLinesOfTransferAccount(ExternalReconciliationPM externalRecoPM, int tenant)
        {
            BankAccountPM bankAccountPM = GetBankAccountForExternaReconciliation(tenant, externalRecoPM.BankAccountId);
            List<ExternalReconciliationLinePM> transferAccountTransactions = externalRecoPM.ExternalReconciliationLines.Where(d => d.LedgerGLAccountId == bankAccountPM.TransferGLAcccountId).ToList();
            return transferAccountTransactions;
        }

        private static void BuildInternalReconcile(ExternalReconciliationPM externalRecoPM, LedgerTransactionPM transferTransaction, JournalPM journalPM)
        {
            IAutoReconcileServiceExt autoRecoService = ContainerAccessor.Container.Resolve(typeof(IAutoReconcileServiceExt), "AutoReconcileServiceExt", new ParameterOverride("", 1)) as IAutoReconcileServiceExt;

            List<AutoReconcileRecord> AutoReconcileRecordList = new List<AutoReconcileRecord>
            {
                new AutoReconcileRecord()
                {
                    LedgerTransactionID = transferTransaction.Id,
                    JournalId = journalPM.Id,
                    ForeignAmountToReconcile = transferTransaction.ForeignAmountCredit != 0 ? transferTransaction.ForeignAmountCredit : transferTransaction.ForeignAmountDebit,
                    ForeignCurrencyIdReconcile = transferTransaction.CurrencyId,
                    LocalAmountToReconcile = transferTransaction.LocalAmountCredit != 0 ? transferTransaction.LocalAmountCredit : transferTransaction.LocalAmountDebit,
                }
            };


            GLAccountPM glaccount = GetGLAccount(externalRecoPM.Tenant, externalRecoPM.GLAccountId);
            autoRecoService.InitMust(glaccount, journalPM, AutoReconcileRecordList);
            autoRecoService.InsertJournalReconcile();
        }

        private static GLAccountPM GetGLAccount(int tenant, string glaccountId)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            GLAccountPM glaccount = gLAccountQueryService.GetSingle(glaccountId, false, false);
            return glaccount;
        }

        private JournalPM CreateJournalForTransferAccountTransaction(ExternalReconciliationPM externalRecoPM, int iteration, LedgerTransactionPM ledgerTransactionPM)
        {
            BankAccountPM bankAccountPM = GetBankAccountForExternaReconciliation(externalRecoPM.Tenant, externalRecoPM.BankAccountId);

            int lineNumber = iteration * 2 - 1;

            JournalPM journal = GetNewJournal(externalRecoPM, ledgerTransactionPM);
            ReconcileExternalPageLinePM bankPageLine = GetBankPage(externalRecoPM);

            JournalLinePM creditJournalLinePM = GetNewCreditJournalLineForBanlGLAccount(lineNumber, bankPageLine, bankAccountPM, ledgerTransactionPM, journal);
            journal.JournalLines.Add(creditJournalLinePM);

            JournalLinePM debitJournalLinePM = GetNewDebitJournalLineForTransferAccount(++lineNumber, bankPageLine, bankAccountPM, ledgerTransactionPM, journal);
            journal.JournalLines.Add(debitJournalLinePM);

            SubmitJournal(journal);

            return journal;
        }

        private static ReconcileExternalPageLinePM GetBankPage(ExternalReconciliationPM externalRecoPM)
        {
            ExternalReconciliationLinePM bankPageRecoLine = externalRecoPM.ExternalReconciliationLines.Where(d => d.ExternalPageLineId != null).FirstOrDefault();

            ReconcileExternalPageLineQueryService reconcileExternalPageLineQueryService = new ReconcileExternalPageLineQueryService(externalRecoPM.Tenant);
            ReconcileExternalPageLinePM pageLine = reconcileExternalPageLineQueryService.GetSingle(bankPageRecoLine.ExternalPageLineId,false,false);
            return pageLine;
        }

        private JournalLinePM GetNewDebitJournalLineForTransferAccount(int lineNumber, ReconcileExternalPageLinePM bankPageLine, BankAccountPM bankAccountPM, LedgerTransactionPM ledgerTransactionPM, JournalPM journal)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(bankAccountPM.Tenant);
            JournalLinePM debitJournalLinePM = new JournalLinePM
            {
                Tenant = bankAccountPM.Tenant,
                Line = lineNumber,
                JournalId = journal.Id,
                ActionCode = "2",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,

                DebitAccountId = bankAccountPM.TransferGLAcccountId,
                //DebitControlAccountId = null,
                CreditAccountId = bankAccountPM.GLAccountId,
                //CreditControlAccountId = null; 

                DocumentDate = bankPageLine.ReferenceDate,
                AccountingDate = ledgerTransactionPM.AccountingDate,
                DueDate = ledgerTransactionPM.AccountingDate,
                LocalAmount = ledgerTransactionPM.LocalAmountCredit,
                CurrencyId = ledgerTransactionPM.CurrencyId,
                ForeignAmount = ledgerTransactionPM.ForeignAmountCredit,
                ExchangeRate = ledgerTransactionPM.ExchangeRate,

                Reference1 = ledgerTransactionPM.Reference1,
                Reference2 = ledgerTransactionPM.Reference2,
                Reference3 = ledgerTransactionPM.Reference3,
                Notes = TextCodesTranslator.TranslateText("ExternalReconciliation.O.RepaymentDeferredCheck", bankAccountPM.Tenant, showLocal),
                ChangeSetOp = ChangeSetOperation.Insert
            };
            return debitJournalLinePM;
        }

        private JournalLinePM GetNewCreditJournalLineForBanlGLAccount(int lineNumber, ReconcileExternalPageLinePM bankPageLine, BankAccountPM bankAccountPM, LedgerTransactionPM ledgerTransactionPM, JournalPM journal)
        {
            bool showLocal = LoggedContactResolver.GetLoggedContactShowLocal(bankAccountPM.Tenant);

            JournalLinePM creditJournalLinePM = new JournalLinePM
            {
                Tenant = bankAccountPM.Tenant,
                Line = lineNumber,
                JournalId = journal.Id,
                ActionCode = "1",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,

                DebitAccountId = bankAccountPM.GLAccountId,
                //DebitControlAccountId = null,
                CreditAccountId = bankAccountPM.TransferGLAcccountId,
                //CreditControlAccountId = null,

                DocumentDate = bankPageLine.ReferenceDate,
                AccountingDate = ledgerTransactionPM.AccountingDate,
                DueDate = ledgerTransactionPM.AccountingDate,
                LocalAmount = ledgerTransactionPM.LocalAmountCredit,
                CurrencyId = ledgerTransactionPM.CurrencyId,
                ForeignAmount = ledgerTransactionPM.ForeignAmountCredit,
                ExchangeRate = ledgerTransactionPM.ExchangeRate,

                Reference1 = ledgerTransactionPM.Reference1,
                Reference2 = ledgerTransactionPM.Reference2,
                Reference3 = ledgerTransactionPM.Reference3,
                Notes = TextCodesTranslator.TranslateText("ExternalReconciliation.O.RepaymentDeferredCheck", bankAccountPM.Tenant, showLocal),
                ChangeSetOp = ChangeSetOperation.Insert
            };
            return creditJournalLinePM;
        }

        private void SubmitJournal(JournalPM journal)
        {
            var accountingContext = AccountingContext.GetContext(journal.Tenant);
            JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), journal.Tenant);
            journalUpdateService.Update(journal, true);
        }

        private JournalPM GetNewJournal(ExternalReconciliationPM externalRecoPM, LedgerTransactionPM ledgerTransactionPM)
        {
            JournalPM journal = new JournalPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                //JournalNumber = "1",
                Tenant = externalRecoPM.Tenant,
                AccountingDate = ledgerTransactionPM.AccountingDate,
                TypeCode = JournalTypeValues.Regular,
                StatusCode = JournalStatusTypeValues.Approved,
                AccountingEntityCode = ledgerTransactionPM.SourceTypeCode,
                AccountingEntityId = ledgerTransactionPM.SourceId,
                AccountingEntityReference = ledgerTransactionPM.SourceNumber,

                CreateDate = TenantServerConfigration.GetCurrentDateTime(externalRecoPM.Tenant),
                CreatedByUserId = externalRecoPM.CreatedByUserId,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(externalRecoPM.Tenant),
                UpdatedByUserId = externalRecoPM.CreatedByUserId,
                ApproveDate = TenantServerConfigration.GetCurrentDateTime(externalRecoPM.Tenant),
                ApprovedByUserId = externalRecoPM.CreatedByUserId,
            };
            return journal;
        }

        private LedgerTransactionPM GetLedgerTransactionById(string ledgerTransactionId, int tenant)
        {
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(tenant);
            LedgerTransactionPM ledgerTransactionPM = ledgerTransactionQueryService.GetSingle(ledgerTransactionId, false, false);
            return ledgerTransactionPM;
        }

        private BankAccountPM GetBankAccountForExternaReconciliation(int tenant, string bankAccountId)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(tenant);
            BankAccountPM bankAccountPM = bankAccountQueryService.GetSingle(bankAccountId, false, false);
            return bankAccountPM;
        }

        protected override void OnUpdating(ExternalReconciliationPM entityPM)
        {

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert) 
            {
                LedgerTransactionQueryService transQuery = new LedgerTransactionQueryService(entityPM.Tenant);
                ReconcileExternalPageLineQueryService pageLineQuery = new ReconcileExternalPageLineQueryService(entityPM.Tenant);
                LedgerTransactionUpdateService transactionService = new LedgerTransactionUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                ReconcileExternalPageLineUpdateService pageLineService = new ReconcileExternalPageLineUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                ARPaymentChequeQueryService aRPaymentChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);
                BankDepositQueryService bankDepositQueryService = new BankDepositQueryService(entityPM.Tenant);
                PaymentChequeQueryService paymentChequeQuery  = new PaymentChequeQueryService(entityPM.Tenant);

                List<string> LedgerTransactionIds = EntityPM.ExternalReconciliationLines.Where(d => d.LedgerTransactionId != null).Select(d=>d.LedgerTransactionId).ToList();
                List<string> PageLineIds = EntityPM.ExternalReconciliationLines.Where(d => d.ExternalPageLineId != null).Select(d => d.ExternalPageLineId).ToList();

                List<LedgerTransactionPM> LedgerTransactions = transQuery.GetLedgerTransactionPMsByIdList(LedgerTransactionIds, entityPM.Tenant);
                List<ReconcileExternalPageLinePM> PageLines = pageLineQuery.GetPageLinesPMsByIdList(PageLineIds, entityPM.Tenant);


                foreach (var transactionPM in LedgerTransactions)
                {
                    transactionPM.ChangeSetOp = ChangeSetOperation.Update;
                    transactionPM.IsExternalReconcile = true;
                    if (transactionPM.SourceTypeCode == "6") // 6- Cheque Deposit
                    {
                        List<ARPaymentChequePM> aRPaymentChequePMs = bankDepositQueryService.GetListByPaymentId(transactionPM.SourceId, entityPM.Tenant);
                        ARPaymentChequePM aRPaymentCheque = aRPaymentChequePMs.Where(a => a.ChequeNumber == transactionPM.Reference1).FirstOrDefault();

                        aRPaymentCheque.StatusCode = "6";
                        aRPaymentCheque.ChangeSetOp = ChangeSetOperation.Update;
                        ARPaymentChequeUpdateService aRPaymentChequeUpdateService = new ARPaymentChequeUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                         aRPaymentChequeUpdateService.Update(aRPaymentCheque, true);
                        
                    }
                    else if (transactionPM.SourceTypeCode == "9") // 9- Payment Cheque
                    {
                        PaymentChequePM chequePM = paymentChequeQuery.GetSingle(transactionPM.SourceId, false, false);

                        chequePM.PaymentChequeStatusCode = "3"; // 3- Redeemed
                        chequePM.ChangeSetOp = ChangeSetOperation.Update;
                        PaymentChequeUpdateService paymentChequeUpdateService = new PaymentChequeUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
                        paymentChequeUpdateService.Update(chequePM, true);

                    }
                    transactionService.Update(transactionPM, false);
                }

                foreach (var pageLinePM in PageLines)
                {
                    pageLinePM.ChangeSetOp = ChangeSetOperation.Update;
                    pageLinePM.IsReconciled = true;
                    pageLineService.Update(pageLinePM, false);
                }


            }

            base.OnUpdating(entityPM);
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
                        PaymentChequePM chequePM = paymentChequeQuery.GetSingle(transactionPM.SourceId, false, false);
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
        protected override void Validate(ExternalReconciliationPM entityPM)
        {

            CheckDifferenc(entityPM);

            base.Validate(entityPM);
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
	 