using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.BLExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.CoreBL.ExternalReconcile
{
    public class TransferTransactionsExternalReconciliationService
    {
        int tenant;
        ExternalReconciliationPM externalRecoPM;
        BankAccountPM bankAccountPM;

        public TransferTransactionsExternalReconciliationService(ExternalReconciliationPM _externalRecoPM)
        {
            ValidateExternalReconciliation(_externalRecoPM);

            tenant = _externalRecoPM.Tenant;
            externalRecoPM = _externalRecoPM;

            bankAccountPM = GetBankAccountById(externalRecoPM.BankAccountId);

        }

        public void HandleTransferAccountTransactions()
        {
            if (bankAccountPM == null)
                UpdateExternalReconciliation();  // came from glaccount external transaction
            else if (bankAccountPM.GLAccountId == bankAccountPM.TransferGLAcccountId)
                UpdateExternalReconciliation();
            else
            {
                
                List<ExternalReconciliationLinePM> transferRecoLines = GetRecoLinesOfTransferAccount();
                //externalRecoPM.ExternalReconciliationLines


                //if (transferRecoLines.Count == 1)
                if (transferRecoLines.Count >= 1)
                {
                    MoveTransactionFromTransferGLAccountToBankGLAccount(transferRecoLines.Select(r=>r.LedgerTransactionId).ToList());
                }
                else if (transferRecoLines.Count == 0)
                {
                    UpdateExternalReconciliation();
                }
                //else if (transferRecoLines.Count > 1)
                //{
                    //throw new ApplicationException("for now, you can select only one transaction for transfer account");
                //}

            }


        }

        private void UpdateExternalReconciliation()
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            ExternalReconciliationUpdateService service = new ExternalReconciliationUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
            externalRecoPM.ChangeSetOp = ChangeSetOperation.Insert;
            service.Update(externalRecoPM, true);
        }

        private void ValidateExternalReconciliation(ExternalReconciliationPM _externalRecoPM)
        {
            if (_externalRecoPM == null)
                throw new ApplicationException("[TransferTransactionsMovingService] external reco cannot be null!");
            if (_externalRecoPM.BankAccountId == null)
                throw new ApplicationException("[TransferTransactionsMovingService] external reco does not have bank account id!");
        }


        private void MoveTransactionFromTransferGLAccountToBankGLAccount(List<string> transactionIds)
        {
            //LedgerTransactionPM transferAccountTransaction = GetLedgerTransactionById(transactionIds);
            ExternalReconciliationLinePM pageLine = externalRecoPM.ExternalReconciliationLines.FirstOrDefault(d => d.ExternalPageLineId != null);

            ExternalReconcileMoveBankCheckFromTransfer2GLAccountService extRecoJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            extRecoJournalService.MustInit(new ExternalReconcileDataProvider(AccountingContext.GetContext(tenant)));
            extRecoJournalService.CreateJournalWithExtReconcile(tenant, transactionIds /*transferAccountTransaction.Id*/, pageLine.ExternalPageLineId);
            JournalPM journalPM = extRecoJournalService.TheJournalPM;

            //JournalPM journalPM = CreateMovingJournal(transferAccountTransaction);

            //BuildInternalReconcile(transferAccountTransaction, journalPM);
            //BuildExternalReconciliationForTransferTransaction(transferAccountTransaction, journalPM);

            SubmitJournal(journalPM);

        }
#if noReference
        private void BuildExternalReconciliationForTransferTransaction(LedgerTransactionPM ledgerTransactionForTransferAccount, JournalPM journalPM)
        {
            var autoExternalReconcileService = new CreateAutoExternalReconcileWhileStreamingService();
            var accountingContext = AccountingContext.GetContext(tenant);
            var providor = new ExternalReconcileDataProvider(accountingContext);

            ExternalReconciliationLinePM pageLine = externalRecoPM.ExternalReconciliationLines.FirstOrDefault(d => d.ExternalPageLineId != null);

            List<LedgerTransactionPM> transactions = new List<LedgerTransactionPM> { ledgerTransactionForTransferAccount };

            //journalPM.JournalExternalReconciles.Add(new JournalExternalReconcilePM()
            //{
            //    Tenant = tenant,
            //    JournalId = journalPM.Id,
            //    LedgerTransactionId = ledgerTransactionForTransferAccount.Id,
            //    ReconcileExternalPageLineId = pageLine.ExternalPageLineId,
            //});

            var myExternalReconcileJournalService = new ExternalReconcileMoveBankCheckFromTransfer2GLAccountService();
            myExternalReconcileJournalService.MustInit(new ExternalReconcileDataProvider(AccountingContext.GetContext(tenant)));
            myExternalReconcileJournalService.CreateJournalWithExtReconcile(tenant, ledgerTransactionForTransferAccount.Id, pageLine.ExternalPageLineId);

            //autoExternalReconcileService.MustInit(providor, journalPM, transactions);
            //autoExternalReconcileService.MoveBankCheckFromTransfer2GLAccount();
        }


#endif
        private List<ExternalReconciliationLinePM> GetRecoLinesOfTransferAccount()
        {

            List<ExternalReconciliationLinePM> transferAccountTransactions = new List<ExternalReconciliationLinePM>();
            if (bankAccountPM.TransferGLAcccountId != null)
                transferAccountTransactions = externalRecoPM.ExternalReconciliationLines.Where(d => d.LedgerGLAccountId == bankAccountPM.TransferGLAcccountId).ToList();

            return transferAccountTransactions;
        }

        private void BuildInternalReconcile(LedgerTransactionPM transferTransaction, JournalPM journalPM)
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


            GLAccountPM glaccount = GetGLAccount(tenant, externalRecoPM.GLAccountId);
            autoRecoService.InitMust(glaccount, journalPM, AutoReconcileRecordList);
            autoRecoService.InsertJournalReconcile();
        }

        private GLAccountPM GetGLAccount(int tenant, string glaccountId)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(tenant);
            GLAccountPM glaccount = gLAccountQueryService.GetSingle(glaccountId, false, false);
            return glaccount;
        }

        private JournalPM CreateMovingJournal(LedgerTransactionPM ledgerTransactionPM)
        {
            int lineNumber = 1;

            JournalPM journal = GetNewJournal(ledgerTransactionPM);

            JournalLinePM creditJournalLinePM = GetNewCreditJournalLineForBankGLAccount(lineNumber, ledgerTransactionPM, journal);
            journal.JournalLines.Add(creditJournalLinePM);

            JournalLinePM debitJournalLinePM = GetNewDebitJournalLineForTransferAccount(++lineNumber, ledgerTransactionPM, journal);
            journal.JournalLines.Add(debitJournalLinePM);


            return journal;
        }

        private ReconcileExternalPageLinePM GetFirstBankPageLineForExternalReco()
        {
            ExternalReconciliationLinePM bankPageRecoLine = externalRecoPM.ExternalReconciliationLines.Where(d => d.ExternalPageLineId != null).FirstOrDefault();

            ReconcileExternalPageLineQueryService reconcileExternalPageLineQueryService = new ReconcileExternalPageLineQueryService(tenant);
            ReconcileExternalPageLinePM pageLine = reconcileExternalPageLineQueryService.GetSingle(bankPageRecoLine.ExternalPageLineId, false, false);
            return pageLine;
        }

        private JournalLinePM GetNewDebitJournalLineForTransferAccount(int lineNumber, LedgerTransactionPM ledgerTransactionPM, JournalPM journal)
        {
            ReconcileExternalPageLinePM bankPageLine = GetFirstBankPageLineForExternalReco();

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

        private JournalLinePM GetNewCreditJournalLineForBankGLAccount(int lineNumber, LedgerTransactionPM ledgerTransactionPM, JournalPM journal)
        {
            ReconcileExternalPageLinePM bankPageLine = GetFirstBankPageLineForExternalReco();

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

        private JournalPM GetNewJournal(LedgerTransactionPM ledgerTransactionPM)
        {
            JournalPM journal = new JournalPM()
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                //JournalNumber = "1",
                Tenant = tenant,
                AccountingDate = ledgerTransactionPM.AccountingDate,
                TypeCode = JournalTypeValues.Regular,
                StatusCode = JournalStatusTypeValues.Approved,
                AccountingEntityCode = ledgerTransactionPM.SourceTypeCode,
                AccountingEntityId = ledgerTransactionPM.SourceId,
                AccountingEntityReference = ledgerTransactionPM.SourceNumber,

                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                CreatedByUserId = externalRecoPM.CreatedByUserId,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = externalRecoPM.CreatedByUserId,
                ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                ApprovedByUserId = externalRecoPM.CreatedByUserId,
            };
            return journal;
        }

        private LedgerTransactionPM GetLedgerTransactionById(string ledgerTransactionId)
        {
            LedgerTransactionQueryService ledgerTransactionQueryService = new LedgerTransactionQueryService(tenant);
            LedgerTransactionPM ledgerTransactionPM = ledgerTransactionQueryService.GetSingle(ledgerTransactionId, false, false);
            return ledgerTransactionPM;
        }

        private BankAccountPM GetBankAccountById(string bankAccountId)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(tenant);
            BankAccountPM bankAccountPM = bankAccountQueryService.GetSingle(bankAccountId, false, false);
            return bankAccountPM;
        }


    }
}
