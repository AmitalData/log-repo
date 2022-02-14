using Logitude.Accounting.BL.CloseTables;
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
    class BankDepositingService
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

        public BankDepositingService(BankDepositPM _depositPM)
        {
            DepositPM = _depositPM;
            Tenant = _depositPM.Tenant;

            ShowLocals = LoggedContactResolver.GetLoggedContactShowLocal(Tenant);

            GetRelatedEntities();

        }

        public void DepositCheques()
        {
                DepositCashbookCheques();
                UpdateChequesStatuses();
                SetChequesFieldsForBankDepositLines();
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

        private void SubmitCashbook()
        {
            CashbookPM.ChangeSetOp = ChangeSetOperation.Update;

            IAccountingContext MyContext2 = AccountingContext.GetContext(Tenant);
            var myCashBookUpdateService = new CashBookUpdateService(MyContext2, new Dictionary<string, IContext>(), Tenant);
            myCashBookUpdateService.Update(CashbookPM, true);
        }

        private CashBookPM GetCashbookById(string id)
        {
            CashBookQueryService cashBookQueryService = new CashBookQueryService(Tenant);
            CashBookPM cashBook = cashBookQueryService.GetSingle(id, false, true);
            return cashBook;
        }

        private void SetChequesFieldsForBankDepositLines()
        {
            List<ARPaymentChequePM> cheques = GetDepositCheques();

            if (!DepositPM.IsCashDeposit)
            {
                foreach (BankDepositLinePM depositLine in DepositPM.BankDepositLines)
                {
                    ARPaymentChequePM cheque = cheques.FirstOrDefault(d => d.Id == depositLine.ARPaymentChequeId);
                    UpdateBankDepositLineStatus(depositLine, cheque);
                }

            }
        }
        private void DepositCashbookCheques()
        {
            List<CashBookLinePM> cashBookLinesPMs = GetDepositCashbookLines();
            foreach (BankDepositLinePM depositLine in DepositPM.BankDepositLines)
            {
                CashBookLinePM cashBookLinePM = cashBookLinesPMs.FirstOrDefault(d => d.ARPChequeId == depositLine.ARPaymentChequeId);
                SetCashbookLineAsDeposited(cashBookLinePM);
            }

            SubmitCashbook();

        }
        private void UpdateChequesStatuses()
        {
            List<ARPaymentChequePM> cheques = GetDepositCheques();

            ValidateCheques(cheques);

            List<ARPaymentChequePM> chequesToUpdate = new List<ARPaymentChequePM>();
            foreach (BankDepositLinePM depositLine in DepositPM.BankDepositLines)
            {
                ARPaymentChequePM cheque = cheques.FirstOrDefault(d => d.Id == depositLine.ARPaymentChequeId);
                SetChequeStatusToInBank(cheque);
                chequesToUpdate.Add(cheque);
            }
            SubmitCheques(chequesToUpdate);
        }

        private void ValidateCheques(List<ARPaymentChequePM> cheques)
        {
            if (cheques.Any(ch => ch.StatusCode == ARPaymentChequeStatusValues.InBankAccount))
            {
                throw new ApplicationException(TextCodesTranslator.TranslateText("BankDeposit.O.AlreadyDeposited", DepositPM.Tenant));
            }
        }

        private List<CashBookLinePM> GetDepositCashbookLines()
        {
            List<string> chequesIds = DepositPM.BankDepositLines.Select(d => d.ARPaymentChequeId).ToList();

            List<CashBookLinePM> cashBookLinesPMs = GetCashbookLinesByChequesIds(chequesIds);
            return cashBookLinesPMs;
        }

        private List<ARPaymentChequePM> GetDepositCheques()
        {
            List<string> chequeIds = DepositPM.BankDepositLines.Select(d => d.ARPaymentChequeId).ToList();
            List<ARPaymentChequePM> cheques = GetARPaymentChequesById(chequeIds);
            return cheques;
        }

        private void SetChequeStatusToInBank(ARPaymentChequePM cheque)
        {
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(Tenant);
            cheque.StatusCode = (cheque.ValueDate > todayDateTime ? "2" : "3");  // 2-In Bank , 3-In Bank Account
            cheque.ChangeSetOp = ChangeSetOperation.Update;
        }

        private void SubmitCheques(List<ARPaymentChequePM> cheques)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(Tenant);
            var myChequeUpdateService = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), Tenant);
            myChequeUpdateService.UpdateMulti(cheques, new List<ARPaymentChequePM>(), new ARPaymentChequePM() , true);
        }

        private List<ARPaymentChequePM> GetARPaymentChequesById(List<string> ids)
        {
            ARPaymentChequeQueryService arpChequeQueryService = new ARPaymentChequeQueryService(Tenant);
            List<ARPaymentChequePM> cheque = arpChequeQueryService.GetChequesByIds(ids, Tenant);
            return cheque;
        }
        private void SetCashbookLineAsDeposited(CashBookLinePM cashBookLinePM)
        {
            cashBookLinePM.IsDeposited = true;

            SubmitCashbookLine(cashBookLinePM);

            //CashBookLinePM cashBookLine = CashbookPM.CashBookLines.Where(d => d.ARPChequeId == depositLine.ARPaymentChequeId).FirstOrDefault();
            //if (CashbookPM != null)
            //{
            //    if (cashBookLine != null)
            //    {
            //        cashBookLine.IsDeposited = true;
            //        cashBookLine.ChangeSetOp = ChangeSetOperation.Update;
            //    }
            //}

        }

        private void SubmitCashbookLine(CashBookLinePM cashBookLinePM)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(Tenant);
            CashBookLineUpdateService cashBookLineUpdateService = new CashBookLineUpdateService(MyContext, new Dictionary<string, IContext>(), Tenant);
            cashBookLinePM.ChangeSetOp = ChangeSetOperation.Update;
            cashBookLineUpdateService.Update(cashBookLinePM, true);
        }

        private List<CashBookLinePM> GetCashbookLinesByChequesIds(List<string> chequeIds)
        {
            CashBookLineQueryService cashBookLineQueryService = new CashBookLineQueryService(Tenant);
            List<CashBookLinePM> cashBookLines = cashBookLineQueryService.GetLinesByChequesIds(chequeIds, CashbookPM.Id, Tenant);
            return cashBookLines;
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

        private void UpdateBankDepositLineStatus(BankDepositLinePM depositLine, ARPaymentChequePM cheque)
        {
            ARPaymentChequeStatusPM status = GetChequeStatusFromCache(cheque.StatusCode);

            depositLine.ChequeStatusName = ShowLocals ? status.LocalName : status.EnglishName;
            depositLine.ChequeStatusCode = cheque.StatusCode;

            // submit later in deposit update service - see stacktrace for this method

        }

        private ARPaymentChequeStatusPM GetChequeStatusFromCache(string code)
        {
            ARPaymentChequeStatusQueryService paymentChequeStatusQueryService = new ARPaymentChequeStatusQueryService(Tenant);
            ARPaymentChequeStatusPM status = paymentChequeStatusQueryService.GetSingle(code, true, true);
            return status;
        }


        private JournalLinePM CreateDebitLineForChequeDeposit( ref int LineNumber, BankDepositLinePM item, ARPaymentChequePM cheque)
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

        public virtual DateTime GetCurrentDateTime()
        {
            return TenantServerConfigration.GetCurrentDateTime(Tenant);
        }
    }
}
