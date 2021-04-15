using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Logitude.BL.DataContracts;
using Logitude.BL.Helpers;
using Logitude.BL.InfrastructureModel;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.DataMapping;
using Logitude.BL.InvoiceModel.Tools.TraceEvents;
using Logitude.BL.InvoiceModel.Tools.Validating;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Server.Tools;
using Logitude.Accounting.Def.EntityQueryServicesExt;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Logitude.BL.InvoiceModel.EntityOtherServices;
using Logitude.Accounting.Data.EntityListQueryServices;
using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Def.BLExt;
using Logitude.BL.Resolvers;

namespace Logitude.BL.InvoiceModel.CoreBL
{
    public class FullAccountingARPaymentApproveService
    {
        GLAccountPM paymentGLAccount = null;
        CashBookPM PaymentCashbook = null;
        ARPaymentPM paymentPM = null;
        int tenant;
        ARPaymentChequePM newlyAddedCheque = null;
        bool isNewEntity = false;
        bool IsCashPayment { get { return paymentPM.AccountingPaymentMethodCode == "CA"; } }
        bool IsChequePayment { get { return paymentPM.AccountingPaymentMethodCode == "CH"; } }


        public FullAccountingARPaymentApproveService(ARPaymentPM paymentPM, int tenant, bool isNewEntity)
        {
            this.paymentPM = paymentPM;
            this.tenant = tenant;
            this.isNewEntity = isNewEntity;

            GetPaymentRelatedEntities();
        }

        public void ApproveARPayment()
        {
            if (PaymentCashbook != null)
                AddChequesOrCashToCashbook();

            CreatePaymentJournalIfNotCreated();
        }

        private void AddChequesOrCashToCashbook()
        {
            if (IsCashPayment)
                AddCashToCashbook();
            else if (IsChequePayment)
                AddNewChequesToCashbook();
        }

        private void AddCashToCashbook()
        {
            var cashAmount = (decimal)paymentPM.AmountInPaymentCurrency;
            AddAmountToCashbook(cashAmount);
        }

        private void CreateJournal(ARPaymentPM arpaymentPM)
        {
            CreatePaymentJournalIfNotCreated();
        }
        private void AddNewChequesToCashbook()
        {
            bool haveReplica = paymentPM.ARPaymentChequeReplicas.Count > 0;
            if (haveReplica)
            {
                AddNewChequesForEachReplica();

                SetARPaymentFieldsForFirstReplica();

                ValidateIfAllReplicasHaveSameValueDate();
            }
            else
            {
                ARPaymentChequePM cheque = CreateARPaymentCheque();
                AddChequeToCashbook(cheque);
                newlyAddedCheque = cheque;
            }

        }
        private void SetARPaymentFieldsForFirstReplica()
        {
            var firstCheque = paymentPM.ARPaymentChequeReplicas.FirstOrDefault(p => p.LineNumber == 1);
            if (firstCheque != null)
            {
                SetPaymentValueDate(firstCheque.ValueDate);

                if (paymentPM.IsExternalEntity)
                    SetPaymentBank(firstCheque.BankId);
            }
        }


        private void SetPaymentBank(string bank)
        {
            paymentPM.Bank = bank;
        }

        private void SetPaymentValueDate(DateTime value)
        {
            paymentPM.ValueDate = value;
        }

        private void AddNewChequesForEachReplica()
        {
            int LineNumberCounter = GetInitialLineNumberForCheque(paymentPM);
            foreach (ARPaymentChequeReplicaPM chequeReplica in paymentPM.ARPaymentChequeReplicas)
            {
                ARPaymentChequePM cheque = CreateARPaymentChequeForReplica(paymentPM, ref LineNumberCounter, chequeReplica);
                AddChequeToCashbook(cheque);
            }
        }

        private void SetARPaymentBankFromFirstCheque()
        {
            var firstCheque = paymentPM.ARPaymentChequeReplicas.FirstOrDefault(p => p.LineNumber == 1);
            if (firstCheque != null)
                paymentPM.Bank = firstCheque.BankId;
        }

        private void ValidateIfAllReplicasHaveSameValueDate()
        {
            if (paymentPM.ARPaymentChequeReplicas.Any(c => c.ValueDate != paymentPM.ValueDate))
                throw new ApplicationException("value date should be the same for all payment cheques");
        }

        private void SetARPaymentValueDateFromFirstCheque()
        {
            var firstCheque = paymentPM.ARPaymentChequeReplicas.FirstOrDefault(p => p.LineNumber == 1);
            if (firstCheque != null)
                paymentPM.ValueDate = firstCheque.ValueDate;
        }

        private ARPaymentChequePM CreateARPaymentChequeForReplica(ARPaymentPM arpaymentPM, ref int LineNumberCounter, ARPaymentChequeReplicaPM chequeReplica)
        {
            bool exist = CheckIfPaymentChequeReplicaExist(chequeReplica.ChequeNumber, chequeReplica.LineNumber, arpaymentPM);
            if (!exist)
            {
                var arPaymentcheque = InitializeARPaymentChequeFromReplica(arpaymentPM, chequeReplica);
                arPaymentcheque.LineNumber = LineNumberCounter++;
                SubmitARPaymentCheque(arPaymentcheque);
                return arPaymentcheque;
            }
            return null;

        }

        private int GetInitialLineNumberForCheque(ARPaymentPM arpaymentPM)
        {
            int LineNumberCounter = 1;
            if (!isNewEntity)
                LineNumberCounter = arpaymentPM.ARPaymentChequeReplicas.Max(d => d.LineNumber) + 1;
            return LineNumberCounter;
        }

        private void AddChequeToCashbook(ARPaymentChequePM arPaymentcheque)
        {
            if (arPaymentcheque != null)
            {
                CreateCashbookLine(arPaymentcheque);
                AddAmountToCashbook(arPaymentcheque.ForeignAmount);
            }
        }

        private ARPaymentChequePM CreateARPaymentCheque()
        {
            var arPaymentcheque = InitializeARPaymanetCheque();
            SubmitARPaymentCheque(arPaymentcheque);
            return arPaymentcheque;
        }


        void UpdateCashCashbookTotal(ARPaymentPM arpaymentPM)
        {
            var paymentForCahCashbook = PaymentCashbook != null && arpaymentPM.AccountingPaymentMethodCode == "CA";
            if (paymentForCahCashbook)
                AddAmountToCashbook((decimal)arpaymentPM.AmountInPaymentCurrency);
        }

        private DateTime? CheckValueDate(ARPaymentPM arpaymentPM, int LineNumberCounter, DateTime? valueDate, ARPaymentChequeReplicaPM chequeReplica)
        {
            if (LineNumberCounter == 1)
            {
                valueDate = chequeReplica.ValueDate;
                arpaymentPM.ValueDate = valueDate;
            }

            if (valueDate != null && valueDate != chequeReplica.ValueDate)
                throw new ApplicationException("value date should be the same for all payment cheques");
            return valueDate;
        }

        private ARPaymentChequePM InitializeARPaymanetCheque()
        {
            ARPaymentChequePM arPaymentcheque = new ARPaymentChequePM
            {
                PaymentId = paymentPM.Id,
                Tenant = paymentPM.Tenant,
                LineNumber = 1,
                ChequeNumber = paymentPM.ChequeOrPaymentRef,
                ValueDate = paymentPM.ValueDate.Value,
                BankBranch = paymentPM.BankBranch,
                BankAccount = paymentPM.Account,
                BankId = paymentPM.Bank,
                CurrencyId = paymentPM.PaymentCurrencyId,
                LocalAmount = (decimal)paymentPM.AmountInLocalCurrency.Value,
                ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency.Value,
                ChangeSetOp = ChangeSetOperation.Insert,
                StatusCode = "1", // In Cashbook  
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                PaymentNumber = paymentPM.PaymentNo
            };
            return arPaymentcheque;
        }

        private void SubmitARPaymentCheque(ARPaymentChequePM arPaymentcheque)
        {
            IARPaymentChequeUpdateServiceExt paymentUpdate = ContainerAccessor.Container.Resolve(typeof(IARPaymentChequeUpdateServiceExt), "ARPaymentChequeUpdateServiceExt", new ParameterOverride("", 1)) as IARPaymentChequeUpdateServiceExt;
            paymentUpdate.Update(arPaymentcheque);
        }

        private ARPaymentChequePM InitializeARPaymentChequeFromReplica(ARPaymentPM arpaymentPM, ARPaymentChequeReplicaPM chequeReplica)
        {
            ARPaymentChequePM arPaymentcheque = new ARPaymentChequePM
            {
                PaymentId = arpaymentPM.Id,
                Tenant = arpaymentPM.Tenant,
                ChequeNumber = chequeReplica.ChequeNumber,
                ValueDate = chequeReplica.ValueDate,
                BankBranch = chequeReplica.BankBranch,
                BankAccount = chequeReplica.BankAccount,
                BankId = chequeReplica.BankId,
                CurrencyId = arpaymentPM.PaymentCurrencyId,
                LocalAmount = (decimal)chequeReplica.LocalAmount,
                ForeignAmount = (decimal)chequeReplica.ForeignAmount,
                ChangeSetOp = ChangeSetOperation.Insert,
                StatusCode = "1", // In Cashbook  
                ExchangeRate = (decimal)arpaymentPM.PaymentCurrencyExchangeRate,
                PaymentNumber = arpaymentPM.PaymentNo
            };
            return arPaymentcheque;
        }



        private void AddAmountToCashbook(decimal amount)
        {
            if (PaymentCashbook.TotalAmount == null)
                PaymentCashbook.TotalAmount = 0;

            PaymentCashbook.TotalAmount += amount;
            PaymentCashbook.ChangeSetOp = ChangeSetOperation.Update;

            SubmitCashbook(PaymentCashbook);
        }

        private void SubmitCashbook(CashBookPM cashbookToSubmit)
        {
            ICashBookUpdateServiceExt cashBookUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookUpdateServiceExt), "CashBookUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookUpdateServiceExt;
            cashBookUpdate.Update(cashbookToSubmit);
        }


        private bool CheckIfPaymentChequeReplicaExist(string chequeNumber, int lineNumber, ARPaymentPM payment)
        {
            ARPaymentChequeReplicaQuery aRPaymentChequeReplicaQuery = new ARPaymentChequeReplicaQuery(payment.Tenant);
            return aRPaymentChequeReplicaQuery.ChequeIfPaymentChequeReplicaExist(payment.Id, chequeNumber, lineNumber, payment.Tenant);


        }

        private void MapPaymentChequeFieldsToPayment(ARPaymentChequeReplicaPM paymentCheque, ARPaymentPM payment)
        {
            payment.Bank = paymentCheque.BankId;


        }

        private void CreateCashbookLine(ARPaymentChequePM aRPaymentCheque)
        {
            CashBookLinePM cashBookLine = CreateCashbookLineForCheque(aRPaymentCheque);
            SubmitCashbookLine(cashBookLine);
        }

        private CashBookLinePM CreateCashbookLineForCheque(ARPaymentChequePM aRPaymentCheque)
        {
            return new CashBookLinePM
            {
                CashBookId = PaymentCashbook.Id,
                Tenant = tenant,
                ARPChequeId = aRPaymentCheque.Id,
                ChangeSetOp = ChangeSetOperation.Insert,
                IsDeposited = false,
                ChequeNumber = aRPaymentCheque.ChequeNumber,
                Bank = aRPaymentCheque.BankAccount
            };
        }

        private void SubmitCashbookLine(CashBookLinePM cashBookLine)
        {
            ICashBookLineUpdateServiceExt cashBookLineUpdate = ContainerAccessor.Container.Resolve(typeof(ICashBookLineUpdateServiceExt), "CashBookLineUpdateServiceExt", new ParameterOverride("", 1)) as ICashBookLineUpdateServiceExt;
            cashBookLineUpdate.Update(cashBookLine);
        }

        private void CreatePaymentJournalIfNotCreated()
        {
            JournalPM journal = GetPaymentJournal();
            if (journal == null)
                CreateNewPaymentJournal();

        }


        private JournalPM CreateNewPaymentJournal()
        {
            JournalPM journal = GetNewJournalForPayment();
            int counter = 0;

            CreateCreditLine(journal, ref counter);
            CreateDebitLines(journal, counter);

            CreateAutomaticReconcileForJournal(journal);

            SubmitJournal(journal);
            return journal;
        }

        private JournalPM GetPaymentJournal()
        {
            IJournalQueryServiceExt journalQueryService = ContainerAccessor.Container.Resolve(typeof(IJournalQueryServiceExt), "JournalQueryServiceExt", new ParameterOverride("", 1)) as IJournalQueryServiceExt;
            var journal = journalQueryService.GetJournalByAccountingEntityIdAndCode(paymentPM.Id, "3", tenant);

            bool isStornoJournal = journal != null && journal.OriginalJournalId != null;
            if (isStornoJournal)
                return null;

            return journal;
        }

        private void SubmitJournal(JournalPM journal)
        {
            IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
            journalUpdate.Update(journal);
        }
        private void CreateAutomaticReconcileForJournal(JournalPM journal)
        {

            if (paymentPM.PaymentInvoices.Any())
            {
                var AutoReconcileARPaymentServiceExt = ContainerAccessor.Container.Resolve(typeof(IAutoReconcileServiceExt), "AutoReconcileServiceExt", new ParameterOverride("", 1)) as IAutoReconcileServiceExt;


                var AutoReconcileRecordList = new List<AutoReconcileRecord>();
                paymentPM.PaymentInvoices.ForEach(r =>
                {
                    var item = new AutoReconcileRecord()
                    {
                        AccountingEntityId = r.ARInvoiceId,
                        LocalAmountToReconcile = Convert.ToDecimal(r.LocalAmount.GetValueOrDefault()),
                        ForeignAmountToReconcile = Convert.ToDecimal(r.ForeignAmount.GetValueOrDefault()),
                        ForeignCurrencyIdReconcile = r.ForeignCurrencyId,


                    };
                    AutoReconcileRecordList.Add(item);
                }
            );
                AutoReconcileARPaymentServiceExt.InitMust(paymentGLAccount, journal, AutoReconcileRecordList);
                AutoReconcileARPaymentServiceExt.InsertJournalReconcile();
            }
        }

        private int CreateDebitLines(JournalPM journal, int counter)
        {
            if (paymentPM.AccountingPaymentMethodCode == "CH")
                counter = CreateDebitLinesForEachCheque(journal, counter);
            else
                counter = CreateDebitLineForNonChequePayment(journal, counter);
            return counter;
        }

        private int CreateDebitLineForNonChequePayment(JournalPM journal, int counter)
        {
            var debitLine = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = ++counter,
                ActionCode = "2",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                DocumentDate = paymentPM.RegisterDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = paymentPM.ValueDate.Value,
                LocalAmount = (decimal)paymentPM.AmountInLocalCurrency,
                CurrencyId = paymentPM.PaymentCurrencyId,
                ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency,
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                Reference1 = paymentPM.PaymentNo,
                Reference2 = paymentPM.ChequeOrPaymentRef,
                DebitAccountId = GetGLAccountIdByPaymentMethodCode(paymentPM),
                CreditAccountId = paymentGLAccount != null ? paymentGLAccount.Id : null,
                ChangeSetOp = ChangeSetOperation.Insert
            };
            journal.JournalLines.Add(debitLine);
            return counter;
        }
        private string GetGLAccountIdByPaymentMethodCode(ARPaymentPM entityPm)
        {
            string glaAccountId = "";

            if (IsCashPayment)
            {
                glaAccountId = PaymentCashbook.AccountId;
            }
            else if (entityPm.AccountingPaymentMethodCode == "CC")
            {
                CreditCardTypeQuery creditCardQuery = new CreditCardTypeQuery(entityPm.Tenant);
                CreditCardTypePM creditCard = new CreditCardTypePM();
                creditCard = creditCardQuery.GetSinglePM(entityPm.CreditCardTypeId, entityPm.Tenant);
                if (creditCard != null)
                {
                    IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                    BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(creditCard.BankAccountId, entityPm.Tenant);
                    if (bankAccount != null)
                    {
                        glaAccountId = bankAccount.GLAccountId;
                    }
                }
            }
            else if (entityPm.AccountingPaymentMethodCode == "BT")
            {
                IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
                BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(entityPm.BankAccountId, entityPm.Tenant);
                if (bankAccount != null)
                {
                    glaAccountId = bankAccount.GLAccountId;
                }
            }
            return glaAccountId;
        }

        private int CreateDebitLinesForEachCheque(JournalPM journal, int counter)
        {
            if (paymentPM.ARPaymentChequeReplicas.Count > 0)
                CreateDebitLineForEachReplica(journal, ref counter);
            else
                CreateDebitLineForARPaymentCheque(newlyAddedCheque, journal, ref counter);
            return counter;
        }

        private int CreateDebitLineForARPaymentCheque(ARPaymentChequePM arPaymentcheque, JournalPM journal, ref int counter)
        {
            GLAccountPM paymentGLAccount = GetPaymentGLAccount();

            var debitLine = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = ++counter,
                ActionCode = "2",
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                DocumentDate = paymentPM.RegisterDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = arPaymentcheque.ValueDate,
                LocalAmount = arPaymentcheque.LocalAmount,
                CurrencyId = arPaymentcheque.CurrencyId,
                ForeignAmount = arPaymentcheque.ForeignAmount,
                ExchangeRate = arPaymentcheque.ExchangeRate,
                Reference1 = arPaymentcheque.PaymentNumber,
                Reference2 = arPaymentcheque.ChequeNumber,
                DebitAccountId = PaymentCashbook.AccountId,
                CreditAccountId = paymentGLAccount != null ? paymentGLAccount.Id : null,
                ChangeSetOp = ChangeSetOperation.Insert,
                Notes = paymentPM.PrintNotes
            };
            journal.JournalLines.Add(debitLine);
            return counter;
        }


        private JournalLinePM CreateCreditLine(JournalPM journal, ref int counter)
        {
            GLAccountPM paymentGLAccount = GetPaymentGLAccount();

            JournalLinePM creditLine = new JournalLinePM
            {
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                Line = ++counter,
                JournalId = journal.Id,
                ActionCode = "1",//- Credit
                ActionTypeCodeEnum = MyJournalActionTypeEnum.Credit,
                CreditAccountId = paymentGLAccount != null ? paymentGLAccount.Id : null,
                DocumentDate = paymentPM.RegisterDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                LocalAmount = (decimal)paymentPM.AmountInLocalCurrency,
                CurrencyId = paymentPM.PaymentCurrencyId,
                ForeignAmount = (decimal)paymentPM.AmountInPaymentCurrency,
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                Reference1 = paymentPM.PaymentNo,
                Notes = paymentPM.PrintNotes,

                DueDate = GetDueDate(),
                Reference2 = GetChequeReference(paymentPM)
            };
            journal.JournalLines.Add(creditLine);


            return creditLine;
        }

        private string GetChequeReference(ARPaymentPM paymentPM)
        {
            string reference;

            if (paymentPM.AccountingPaymentMethodCode == "BT")
            {
                reference = paymentPM.ChequeOrPaymentRef;
            }
            else
            {
                if (paymentPM.ARPaymentChequeReplicas.Count > 0)
                    reference = GetReferencesForChequeReplicas(paymentPM);
                else
                    reference = newlyAddedCheque != null ? newlyAddedCheque.ChequeNumber : paymentPM.ChequeOrPaymentRef;
            }

            return reference;
        }

        private string GetReferencesForChequeReplicas(ARPaymentPM paymentPM)
        {
            string reference = "";
            int count = 0;
            foreach (ARPaymentChequeReplicaPM cheque in paymentPM.ARPaymentChequeReplicas)
            {
                count++;
                if (count == paymentPM.ARPaymentChequeReplicas.Count())
                {
                    reference += cheque.ChequeNumber;
                }
                else
                {
                    reference = reference + cheque.ChequeNumber + ",";
                }
                if (count == 2)
                {
                    break;
                }

            }

            if (count == 2 && reference.Length > 30)
            {
                reference = reference.Split(',')[0].Trim();
            }

            return reference;
        }

        private DateTime GetDueDate()
        {
            DateTime dueDate;
            if (paymentPM.AccountingPaymentMethodCode == "CA")
                dueDate = (DateTime)paymentPM.RegisterDate;
            else
                dueDate = newlyAddedCheque != null ? newlyAddedCheque.ValueDate : paymentPM.ValueDate.Value;
            return dueDate;
        }

        private JournalPM GetNewJournalForPayment()
        {
            return new JournalPM
            {
                Tenant = tenant,
                JournalNumber = "1",
                CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                AccountingDate = paymentPM.RegisterDate.Value,
                TypeCode = "0",
                StatusCode = "2",
                CreatedByUserId = paymentPM.CreatedByUserId,
                AccountingEntityCode = "3",
                AccountingEntityId = paymentPM.Id,
                AccountingEntityReference = paymentPM.PaymentNo,
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                UpdatedByUserId = paymentPM.UpdatedByUserId,
                ApproveDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                ApprovedByUserId = paymentPM.UpdatedByUserId,
                ChangeSetOp = ChangeSetOperation.Insert
            };
        }

        private void CreateDebitLineForEachReplica(JournalPM journal, ref int counter)
        {
            foreach (ARPaymentChequeReplicaPM cheque in paymentPM.ARPaymentChequeReplicas)
            {
                var journalLine = new JournalLinePM
                {
                    Tenant = tenant,
                    JournalId = journal.Id,
                    Line = ++counter,
                    ActionCode = "2",
                    ActionTypeCodeEnum = MyJournalActionTypeEnum.Debit,
                    DocumentDate = paymentPM.RegisterDate.Value,
                    AccountingDate = paymentPM.RegisterDate.Value,
                    DueDate = cheque.ValueDate,
                    LocalAmount = cheque.LocalAmount,
                    CurrencyId = paymentPM.PaymentCurrencyId,
                    ForeignAmount = cheque.ForeignAmount,
                    ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                    Reference1 = paymentPM.PaymentNo,
                    Reference2 = cheque.ChequeNumber,
                    DebitAccountId = PaymentCashbook.AccountId,
                    CreditAccountId = paymentGLAccount != null ? paymentGLAccount.Id : null,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    Notes = paymentPM.PrintNotes
                };
                journal.JournalLines.Add(journalLine);
            }
        }




        private void GetPaymentRelatedEntities()
        {
            GetPaymentGLAccount();
            GetPaymentCashbook();
        }
        private GLAccountPM GetPaymentGLAccount()
        {
            GLAccountPM glAccount = null;
            if (paymentPM.IsFullAccounting)
            {
                glAccount = GetGLAccount(paymentPM.BillToId, paymentPM.Tenant);
                paymentGLAccount = glAccount;
            }
            return glAccount;
        }
        private CashBookPM GetPaymentCashbook()
        {
            ICashBookQueryServiceExt cashQuery = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
            CashBookPM cashBook = cashQuery.GetByPaymentAndCurrencyAndBranch(paymentPM.PaymentCurrencyId, IsCashPayment ? "1" : "2", paymentPM.BranchId, tenant);
            PaymentCashbook = cashBook;
            return cashBook;
        }
        private GLAccountPM GetGLAccount(string billToId, int tenant)
        {
            GLAccountPM glaAccount = null;
            CardRepository cardRep = new CardRepository(tenant);
            Card card = cardRep.GetSingleCard(billToId, tenant);
            if (card != null)
            {
                IGLAccountQueryServiceExt glAccountQuery = ContainerAccessor.Container.Resolve(typeof(IGLAccountQueryServiceExt), "GLAccountQueryServiceExt", new ParameterOverride("", 1)) as IGLAccountQueryServiceExt;
                glaAccount = glAccountQuery.GetSingleGLAccountPM(card.GLAccountId, tenant);

                if (glaAccount != null && glaAccount.IsMultiCurrency.Value)
                {
                    string splitByCurrencyAccountId = GetAccountIdForGLAccountCurrency(glaAccount, paymentPM.PaymentCurrencyId);
                    glaAccount = glAccountQuery.GetSingleGLAccountPM(splitByCurrencyAccountId, tenant);
                }
                else return glaAccount;
            }


            return glaAccount;
        }

        private string GetAccountIdForGLAccountCurrency(GLAccountPM gLAccount, string paymentCurrencyId)
        {
            GLAccountCurrencyRepository glAccountCurrencyRepository = new GLAccountCurrencyRepository(gLAccount.Tenant);
            GLAccountCurrency gLAccountCurrency = glAccountCurrencyRepository.GetEntityByCurrencyAndGLAccountId(gLAccount.Id, paymentCurrencyId, gLAccount.Tenant);
            if (gLAccountCurrency != null)
            {
                return gLAccountCurrency.GLAccountId;
            }
            else return gLAccount.Id;

        }
    }
}
