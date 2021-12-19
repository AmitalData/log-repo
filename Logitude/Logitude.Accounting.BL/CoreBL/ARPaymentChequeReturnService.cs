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

namespace Logitude.Accounting.BL.CoreBL
{
    public class ARPaymentChequeReturnService
    {
        private const string ChequePaymentMethodCode = "2";
        ARPaymentChequeReturnServiceArguments arguments;
        public ARPaymentChequeReturnService(ARPaymentChequeReturnServiceArguments arguments)
        {
            this.arguments = arguments;
        }

        public void ReturnChequeToCustomer()
        {
            ARPaymentChequePM cheque = GetChequeById(arguments.chequeId, arguments.tenant);
            UpdateChequeStatusAsReturnedToCustomer(cheque);
            UpdateCashbookTotal(cheque);
            CreateJournal();
        }

        private void UpdateChequeStatusAsReturnedToCustomer(ARPaymentChequePM cheque)
        {
            cheque.ChangeSetOp = ChangeSetOperation.Update;
            cheque.StatusCode = ARPaymentChequeStatusValues.ReturnedToCustomer;

            IARPaymentChequeUpdateServiceExt paymentUpdate = ContainerAccessor.Container.Resolve(typeof(IARPaymentChequeUpdateServiceExt), "ARPaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IARPaymentChequeUpdateServiceExt;
            paymentUpdate.Update(cheque);
        }
        private void UpdateCashbookTotal(ARPaymentChequePM cheque)
        {
            CashBookPM cashBook = GetConnectedCashbook();

            cashBook.TotalAmount -= cheque.ForeignAmount;
            cashBook.ChangeSetOp = ChangeSetOperation.Update;

            SubmitCashbook(cashBook);
        }

        private void CreateJournal()
        {

        }
        private JournalPM InitJournal()
        {
            JournalPM newJournal = new JournalPM
            {
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = arguments.tenant,
                CreateDate = GetCurrentDateTime(),
                CreatedByUserId = arguments.paymentPM.CreatedByUserId,
                UpdateDate = GetCurrentDateTime(),
                UpdatedByUserId = arguments.paymentPM.UpdatedByUserId,
                AccountingDate = arguments.paymentPM.RegisterDate.Value,
                TypeCode = JournalTypeValues.Regular,
                StatusCode = JournalStatusTypeValues.Approved,
                AccountingEntityId = arguments.paymentPM.Id,
                AccountingEntityReference = arguments.paymentPM.PaymentNo,
                ExternalNo = null,
                AccountingEntityCode = AccountingEntityValues.ARPayment
            };

            return newJournal;
        }


        public DateTime GetCurrentDateTime()
        {
            return TenantServerConfigration.GetCurrentDateTime(Tenant);
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


        private ARPaymentChequePM GetChequeById(string chequeId, int tenant)
        {
            ARPaymentChequeQueryService aRPaymentChequeQuery = new ARPaymentChequeQueryService(tenant);
            return aRPaymentChequeQuery.GetSingle(chequeId, false, false);
        }

        private void SubmitCashbook(CashBookPM cashBook)
        {
            ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
            cashBookUpdate.Update(cashBook);
        }

        private CashBookPM GetConnectedCashbook()
        {
            CashBookQueryService cashbookQuery = new CashBookQueryService(arguments.tenant);
            return cashbookQuery.GetByPaymentAndCurrencyAndBranch(arguments.paymentPM.PaymentCurrencyId, ChequePaymentMethodCode, arguments.paymentPM.BranchId, arguments.tenant);
        }
    }
    public class ARPaymentChequeReturnServiceArguments
    {
        public int tenant { get; set; }
        public string chequeId { get; set; }
        public ARPaymentPM paymentPM { get; set; }
    }
}
