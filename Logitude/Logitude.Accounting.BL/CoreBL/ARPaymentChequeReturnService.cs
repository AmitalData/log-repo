using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.BL.InvoiceModel.EntityPMs;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;
using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Simplog.Data.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.Data;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data.EntityPOCOs;

namespace Logitude.Accounting.BL.CoreBL
{
    public class ARPaymentChequeReturnService
    {
        private const string ChequePaymentMethodCode = "2";
        private const string CreditLineNotes = "החזרת שיק ללקוח";
        private CardPM billTo;
        private CashBookPM paymentCashbook;
        private GLAccountPM cashbookGLAccount;
        private ARPaymentChequeReturnServiceArguments arguments;
        private ARPaymentChequePM cheque;
        private JournalPM createdJournal;
        private ARPaymentPM paymentPM;
        public ARPaymentChequeReturnService(ARPaymentChequeReturnServiceArguments arguments)
        {
            this.arguments = arguments;
            ValidateServiceArguments();
            GetRelatedEntities();
        }
        private void ValidateServiceArguments()
        {
            if (arguments.PaymentId is null || arguments.ChequeId is null)
                throw new ApplicationException("ARPaymentChequeReturnService parameters is incomplete");
        }
        private void GetRelatedEntities()
        {
            paymentPM = GetARPayment();
            cheque = GetPaymentCheque();
            billTo = GetPaymentBilltoAccount();
            paymentCashbook = GetPaymentCashbook();
            cashbookGLAccount = GetCashbookGLAccount(paymentCashbook);
        }

        public void ReturnChequeToCustomer()
        {
            ReturnChequeValidation();
            UpdateChequeStatusAsReturnedToCustomer(cheque);
            UpdateCashbookTotal(cheque);
            CreateJournal();
        }

        private void ReturnChequeValidation()
        {
            if(cheque.StatusCode != ARPaymentChequeStatusValues.InCashbook && cheque.StatusCode != ARPaymentChequeStatusValues.ReturnedFromBank)
                throw new ApplicationException("You can't return the cheque if it's status is not In cashbook or returned from bank");
        }

        private void AddAccountingEntitieJournal(JournalPM entityPM, string action, string ChildEntityId = null)
        {
            IAccountingEntityJournalUpdateServiceExt service = ContainerAccessor.Container.Resolve(typeof(IAccountingEntityJournalUpdateServiceExt), "AccountingEntityJournalUpdateServiceExt", new ParameterOverride("", 1)) as IAccountingEntityJournalUpdateServiceExt;
            service.AddAccountingEntitieJournal(entityPM, action, ChildEntityId);
        }

        private void UpdateChequeStatusAsReturnedToCustomer(ARPaymentChequePM cheque)
        {
            cheque.ChangeSetOp = ChangeSetOperation.Update;
            cheque.StatusCode = ARPaymentChequeStatusValues.ReturnedToCustomer;

            SubmitARPaymentCheque(cheque);
        }

        private void SubmitARPaymentCheque(ARPaymentChequePM cheque)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(arguments.Tenant);
            ARPaymentChequeUpdateService aRPaymentChequeUpdateService = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), arguments.Tenant);
            aRPaymentChequeUpdateService.Update(cheque, true);
        }

        private void UpdateCashbookTotal(ARPaymentChequePM cheque)
        {
            paymentCashbook.TotalAmount -= cheque.ForeignAmount;
            paymentCashbook.ChangeSetOp = ChangeSetOperation.Update;

            SubmitCashbook(paymentCashbook);
        }

        private void CreateJournal()
        {
            InitJournal();
            CreateJournalLines();
            AddAccountingEntitieJournal(createdJournal, AccountingEntityJournalActions.ARPaymentReturnToCustomer, cheque.Id);
            SubmitJournal();

        }
        private JournalPM InitJournal()
        {
            createdJournal = new JournalPM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = arguments.Tenant,
                CreateDate = GetCurrentDateTime(),
                CreatedByUserId = paymentPM.CreatedByUserId,
                UpdateDate = GetCurrentDateTime(),
                UpdatedByUserId = paymentPM.UpdatedByUserId,
                AccountingDate = DateTime.Now,
                TypeCode = JournalTypeValues.Regular,
                StatusCode = JournalStatusTypeValues.InProcessing,
                AccountingEntityId = paymentPM.Id,
                AccountingEntityReference = paymentPM.PaymentNo,
                ExternalNo = null,
                AccountingEntityCode = AccountingEntityValues.ARPayment
            };

            return createdJournal;
        }

        private void CreateJournalLines()
        {
            var creditLine = CreateCreditLineForCashbook();
            createdJournal.JournalLines.Add(creditLine);

            var debitLine =  CreateDebitLineForCustomer();
            createdJournal.JournalLines.Add(debitLine);
        }

        private JournalLinePM CreateCreditLineForCashbook()
        {
            return new JournalLinePM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = arguments.Tenant,
                Line = 1,
                ActionCode = "1",
                AccountingDate = DateTime.Now,
                DocumentDate = paymentPM.RegisterDate.Value,
                DueDate = cheque.ValueDate,
                LocalAmount = cheque.LocalAmount,
                ForeignAmount = cheque.ForeignAmount,
                CurrencyId = cheque.CurrencyId,
                ExchangeRate = cheque.LocalAmount / cheque.ForeignAmount,
                
                CreditAccountId = paymentCashbook.AccountId,
                DebitAccountId = billTo.GLAccountId,
                DebitControlAccountId = cashbookGLAccount.ControlAccountId,
                Reference1 = cheque.ChequeNumber,
                Reference2 = paymentPM.PaymentNo,
                Notes = CreditLineNotes
            };
        }
        private JournalLinePM CreateDebitLineForCustomer()
        {
            return new JournalLinePM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = arguments.Tenant,
                Line = 2,
                ActionCode = "2",
                AccountingDate = DateTime.Now,
                DocumentDate = paymentPM.RegisterDate.Value,
                DueDate = cheque.ValueDate,
                LocalAmount = cheque.LocalAmount,
                ForeignAmount = cheque.ForeignAmount,
                CurrencyId = cheque.CurrencyId,
                ExchangeRate = cheque.LocalAmount / cheque.ForeignAmount,
                
                DebitAccountId = billTo.GLAccountId,
                CreditAccountId = paymentCashbook.AccountId,
                DebitControlAccountId = cashbookGLAccount.ControlAccountId,
                Reference1 = cheque.ChequeNumber,
                Reference2 = paymentPM.PaymentNo,
                Notes = CreditLineNotes
            };
        }

        private void SubmitJournal()
        {
            IAccountingContext MyContext = AccountingContext.GetContext(arguments.Tenant);
            var myJournalUpdateService = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), arguments.Tenant);
            myJournalUpdateService.Update(createdJournal, true);
        }
        public DateTime GetCurrentDateTime()
        {
            return TenantServerConfigration.GetCurrentDateTime(arguments.Tenant);
        }


        private ARPaymentPM GetARPayment()
        {
            ARPaymentQuery aRPaymentQuery = new ARPaymentQuery(arguments.Tenant);
            return aRPaymentQuery.GetSinglePM(arguments.PaymentId, arguments.Tenant);
        }

        private GLAccountPM GetCashbookGLAccount(CashBookPM paymentCashbook)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(arguments.Tenant);
            var cashbookGLAccount = gLAccountQueryService.GetSinglePM(paymentCashbook.AccountId, arguments.Tenant);
            return cashbookGLAccount;
        }

        private CashBookPM GetPaymentCashbook()
        {
            CashBookQueryService cashBookQueryService = new CashBookQueryService(arguments.Tenant);
            var paymentCashbook = cashBookQueryService
                .GetByPaymentAndCurrencyAndBranch(paymentPM.PaymentCurrencyId,
                ChequePaymentMethodCode, paymentPM.BranchId, arguments.Tenant);
            return paymentCashbook;
        }

        private CardPM GetPaymentBilltoAccount()
        {
            CardQuery cardQuery = new CardQuery(arguments.Tenant);
            var billTo = cardQuery.GetSinglePM(paymentPM.BillToId, arguments.Tenant);
            return billTo;
        }

        private ARPaymentChequePM GetPaymentCheque()
        {
            ARPaymentChequeQueryService aRPaymentChequeQuery = new ARPaymentChequeQueryService(arguments.Tenant);
            return aRPaymentChequeQuery.GetSingle(arguments.ChequeId, false, false);
        }

        private void SubmitCashbook(CashBookPM cashBook)
        {
            ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
            cashBookUpdate.Update(cashBook);
        }

        private CashBookPM GetConnectedCashbook()
        {
            CashBookQueryService cashbookQuery = new CashBookQueryService(arguments.Tenant);
            return cashbookQuery.GetByPaymentAndCurrencyAndBranch(paymentPM.PaymentCurrencyId, ChequePaymentMethodCode, paymentPM.BranchId, arguments.Tenant);
        }

    }
    public class ARPaymentChequeReturnServiceArguments
    {
        public int Tenant { get; set; }
        public string ChequeId { get; set; }
        public string PaymentId { get; set; }
    }
}
