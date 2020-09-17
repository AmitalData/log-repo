using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Logitude.Accounting.BL.CoreBL.BankDeposit
{
    class BankDepositJournalCreator
    {
        bool ShowLocals = false;
        int Tenant;

        private BankDepositPM DepositPM;
        CashBookPM CashbookPM;
        JournalPM journal;
        BankAccountPM BankAccount;
        GLAccountPM BankGLAccount;
        GLAccountPM BankDeferedGLAccount;
        GLAccountPM CashbookGLAccount;

        public BankDepositJournalCreator(BankDepositPM _depositPM)
        {
            DepositPM = _depositPM;
            Tenant = _depositPM.Tenant;

            ShowLocals = LoggedContactResolver.GetLoggedContactShowLocal(Tenant);

            GetRelatedEntities();

        }

        public void CreateJounal()
        {
            journal = InitJournal();
            CreateJournalLines();
            SubmitJournal();
        }
        private void CreateJournalLines()
        {
            List<ARPaymentChequePM> cheques = GetDepositCheques();
            CreateCreditLinesForCashbook(cheques);
            CreateDebitLinesForBank(cheques);
        }
        private List<ARPaymentChequePM> GetDepositCheques()
        {
            List<string> chequeIds = DepositPM.BankDepositLines.Select(d => d.ARPaymentChequeId).ToList();
            List<ARPaymentChequePM> cheques = GetARPaymentChequesById(chequeIds);
            return cheques;
        }

        private List<ARPaymentChequePM> GetARPaymentChequesById(List<string> ids)
        {
            ARPaymentChequeQueryService arpChequeQueryService = new ARPaymentChequeQueryService(Tenant);
            List<ARPaymentChequePM> cheque = arpChequeQueryService.GetChequesByIds(ids, Tenant);
            return cheque;
        }
        private void CreateDebitLinesForBank(List<ARPaymentChequePM> cheques)
        {
            int lineNumber = journal.JournalLines.Max(d => d.Line);

            if (DepositPM.IsCashDeposit)
            {
                JournalLinePM debitLine = CreateDebitLineForCashCashbook(lineNumber);
                journal.JournalLines.Add(debitLine);
            }
            else
            {
                List<JournalLinePM> _journalLines = new List<JournalLinePM>();

                foreach (BankDepositLinePM depositLine in DepositPM.BankDepositLines)
                {
                    ARPaymentChequePM cheque = cheques.FirstOrDefault(d => d.Id == depositLine.ARPaymentChequeId);
                    JournalLinePM chequeDebitLine = CreateDebitLineForChequeDeposit(ref lineNumber, depositLine, cheque);
                    _journalLines.Add(chequeDebitLine);
                    if (_journalLines.FirstOrDefault().DueDate > TenantServerConfigration.GetCurrentDateTime(Tenant))
                    {
                        journal.JournalLines.Add(chequeDebitLine);
                    }

                }
                if (_journalLines.FirstOrDefault().DueDate <= TenantServerConfigration.GetCurrentDateTime(Tenant))
                {
                    AddSumOfDebitLinesForJournalLines(_journalLines);
                }
                
            }
        }

        private void AddSumOfDebitLinesForJournalLines(List<JournalLinePM> _journalLines)
        {
            JournalLinePM Sum_Line = _journalLines.FirstOrDefault();
            Sum_Line.DueDate = journal.AccountingDate;
            Sum_Line.ForeignAmount = _journalLines.Sum(s => s.ForeignAmount);
            Sum_Line.LocalAmount = _journalLines.Sum(s => s.LocalAmount);
            Sum_Line.ExternalOpenAmount = _journalLines.Sum(s => s.ExternalOpenAmount);
            Sum_Line.Reference1 = DepositPM.DepositNumber.ToString();
            Sum_Line.Reference2 = _journalLines.FirstOrDefault().Reference2;
            foreach (JournalLinePM line in _journalLines)
            {
                if (line.Reference2 != Sum_Line.Reference2)
                {
                    Sum_Line.Reference2 = null;
                    break;
                }

            }
            journal.JournalLines.Add(Sum_Line);
        }
        private JournalLinePM CreateDebitLineForChequeDeposit(ref int LineNumber, BankDepositLinePM item, ARPaymentChequePM cheque)
        {
            JournalLinePM chequeDebitLine = new JournalLinePM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = journal.Tenant,
                Line = ++LineNumber,
                ActionCode = "2", //Debit
                DueDate = cheque.ValueDate,
                LocalAmount = item.LocalAmount,
                ForeignAmount = item.ForeignAmount,
                CurrencyId = DepositPM.DepositCurrencyId,
                DocumentDate = DepositPM.AccountingDate,
                AccountingDate = DepositPM.AccountingDate,
                Reference1 = cheque.ChequeNumber,
                Reference2 = DepositPM.DepositNumber.ToString(),
                ExchangeRate = cheque.LocalAmount / cheque.ForeignAmount
            };

            DateTime todayDateTime = GetCurrentDateTime();
            if (cheque.ValueDate <= todayDateTime)
            {
                chequeDebitLine.DebitAccountId = BankAccount.GLAccountId;
            }
            else
            {
                chequeDebitLine.DebitAccountId = BankAccount.DeferredGLAccountId;
            }

            GLAccountPM gLAccount = GetGLAccountById(chequeDebitLine.DebitAccountId, true);
            chequeDebitLine.DebitControlAccountId = gLAccount?.ControlAccountId;

            //opposit account
            chequeDebitLine.CreditAccountId = CashbookPM.AccountId;
            return chequeDebitLine;
        }
        private JournalLinePM CreateDebitLineForCashCashbook(int LineNumber)
        {
            JournalLinePM newDebitJournalLine = new JournalLinePM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = journal.Tenant,
                Line = ++LineNumber,
                ActionCode = "2", //Debit
                DocumentDate = DepositPM.CreateDate,
                AccountingDate = DepositPM.AccountingDate,

                DebitAccountId = BankAccount.GLAccountId,
                DebitControlAccountId = BankGLAccount?.ControlAccountId,

                // opposit account
                CreditAccountId = CashbookPM.AccountId,

                DueDate = DepositPM.CreateDate,
                LocalAmount = DepositPM.LocalDepositAmount,
                ForeignAmount = DepositPM.ForeignAmount,
                CurrencyId = DepositPM.DepositCurrencyId,
                ExchangeRate = DepositPM.LocalDepositAmount / DepositPM.ForeignAmount,
                Reference1 = DepositPM.DepositNumber.ToString()
            };
            return newDebitJournalLine;
        }
        private void GetRelatedEntities()
        {
            CashbookPM = GetCashbookById(DepositPM.CashBookId);
            
            if(DepositPM.DepositBankAccountId != null)
                BankAccount = GetBankAccountById(DepositPM.DepositBankAccountId);
            else
                BankAccount = GetBankAccountByNumber(DepositPM.BankAccountNumber);
            
            BankGLAccount = GetGLAccountById(BankAccount.GLAccountId, true);
            BankDeferedGLAccount = GetGLAccountById(BankAccount.DeferredGLAccountId, true);
            CashbookGLAccount = GetGLAccountById(CashbookPM.AccountId, true);
        }

        private void SubmitJournal()
        {
            IAccountingContext MyContext = AccountingContext.GetContext(Tenant);
            var myJournalUpdateService = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), Tenant);
            myJournalUpdateService.Update(journal, true);
        }

        private CashBookPM GetCashbookById(string id)
        {
            CashBookQueryService cashBookQueryService = new CashBookQueryService(Tenant);
            CashBookPM cashBook = cashBookQueryService.GetSingle(id, false, true);
            return cashBook;
        }

        private JournalPM InitJournal()
        {
            JournalPM newJournal = new JournalPM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = DepositPM.Tenant,
                CreateDate = GetCurrentDateTime(),
                CreatedByUserId = DepositPM.CreatedByUserId,
                UpdateDate = GetCurrentDateTime(),
                UpdatedByUserId = DepositPM.UpdatedByUserId,
                AccountingDate = DepositPM.AccountingDate,
                TypeCode = "0", //Manual
                StatusCode = "2", // Approved
                AccountingEntityId = DepositPM.Id,
                AccountingEntityReference = DepositPM.DepositNumber.ToString(),
                ExternalNo = null,
                ApproveDate = DepositPM.CreateDate,
                ApprovedByUserId = DepositPM.CreatedByUserId,
                AccountingEntityCode = DepositPM.IsCashDeposit ? "7" : "6"
            };

            return newJournal;
        }
        private void CreateCreditLinesForCashbook(List<ARPaymentChequePM> cheques)
        {

            if (DepositPM.IsCashDeposit)
            {
                JournalLinePM newCreditJournalLine = CreateLineForCashCashbook();
                journal.JournalLines.Add(newCreditJournalLine);
            }
            else
            {
                int lineNumber = 0;

                foreach (BankDepositLinePM depositLine in DepositPM.BankDepositLines)
                {

                    ARPaymentChequePM cheque = cheques.FirstOrDefault(d => d.Id == depositLine.ARPaymentChequeId);

                    JournalLinePM newCreditJournalLine = CreateLineForCheque(ref lineNumber, depositLine, cheque);
                    journal.JournalLines.Add(newCreditJournalLine);
                }

            }
        }

        private JournalLinePM CreateLineForCheque(ref int LineNumber,BankDepositLinePM depositLine, ARPaymentChequePM cheque)
        {
            JournalLinePM newCreditJournalLine = new JournalLinePM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = journal.Tenant,
                Line = ++LineNumber,
                ActionCode = "1", //Credit
                DueDate = cheque.ValueDate,
                LocalAmount = depositLine.LocalAmount,
                ForeignAmount = depositLine.ForeignAmount,
                CurrencyId = DepositPM.DepositCurrencyId,
                DocumentDate = DepositPM.AccountingDate,
                AccountingDate = DepositPM.AccountingDate,
                ExchangeRate = cheque.LocalAmount / cheque.ForeignAmount,

                CreditAccountId = CashbookPM.AccountId
            };

            newCreditJournalLine.CreditControlAccountId = CashbookGLAccount?.ControlAccountId;


            // opposit account
            newCreditJournalLine.DebitAccountId
                = cheque.ValueDate <= TenantServerConfigration.GetCurrentDateTime(DepositPM.Tenant)
                                    ? BankGLAccount.Id : BankDeferedGLAccount.Id;

            newCreditJournalLine.Reference1 = cheque.ChequeNumber;
            newCreditJournalLine.Reference2 = DepositPM.DepositNumber.ToString();
            return newCreditJournalLine;
        }

        private JournalLinePM CreateLineForCashCashbook()
        {
            JournalLinePM newCreditJournalLine = new JournalLinePM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = journal.Tenant,
                Line = 1,
                ActionCode = "1", //Credit
                DueDate = DepositPM.CreateDate,
                LocalAmount = DepositPM.LocalDepositAmount,
                ForeignAmount = DepositPM.ForeignAmount,
                CurrencyId = DepositPM.DepositCurrencyId,
                DocumentDate = DepositPM.CreateDate,
                AccountingDate = DepositPM.AccountingDate,
                ExchangeRate = DepositPM.LocalDepositAmount / DepositPM.ForeignAmount,

                CreditAccountId = CashbookPM.AccountId
            };

            newCreditJournalLine.CreditControlAccountId = CashbookGLAccount?.ControlAccountId;

            // opposit account
            newCreditJournalLine.DebitAccountId = BankGLAccount.Id;

            newCreditJournalLine.Reference1 = DepositPM.DepositNumber.ToString();
            return newCreditJournalLine;
        }

        private GLAccountPM GetGLAccountById(string id, bool getFromCache = false)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(Tenant);
            GLAccountPM bankGLAccount = gLAccountQueryService.GetSingle(id, false, getFromCache);
            return bankGLAccount;
        }

        private BankAccountPM GetBankAccountByNumber(string id)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(Tenant);
            BankAccountPM bankAccount = bankAccountQueryService.GetByAccountNumber(id, Tenant);
            return bankAccount;
        }
        private BankAccountPM GetBankAccountById(string id)
        {
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(Tenant);
            BankAccountPM bankAccount = bankAccountQueryService.GetSingle(id,false,false);
            return bankAccount;
        }

        public virtual DateTime GetCurrentDateTime()
        {
            return TenantServerConfigration.GetCurrentDateTime(Tenant);
        }
    }
}
