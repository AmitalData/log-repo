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
        const string PartnerTypeId_Customer = "CS";
        GLAccountPM paymentGLAccount = null;
        CashBookPM PaymentCashbook = null;
        ARPaymentPM paymentPM = null;
        int tenant;
        ARPaymentChequePM newlyAddedCheque = null;
        bool isNewEntity = false;
        bool IsCashPayment { get { return paymentPM.AccountingPaymentMethodCode == "CA"; } }
        bool IsChequePayment { get { return paymentPM.AccountingPaymentMethodCode == "CH"; } }
        bool IsBankTransferPayment { get { return paymentPM.AccountingPaymentMethodCode == "BT"; } }
        private ARPaymentBankTranferRepository paymentBankTranferRepository;

        JournalPM journal;
        public FullAccountingARPaymentApproveService(ARPaymentPM paymentPM, int tenant, bool isNewEntity)
        {
            this.paymentPM = paymentPM;
            this.tenant = tenant;
            this.isNewEntity = isNewEntity;
            paymentBankTranferRepository = new ARPaymentBankTranferRepository(tenant);

            GetPaymentRelatedEntities();
        }

        public void ApproveARPayment()
        {
            if (PaymentCashbook != null)
                AddChequesOrCashToCashbook();
            if (IsBankTransferPayment)
            {
                AddNewBankTransfers();
            }
            AddARPaymentJounal();
            CreatePaymentJournalIfNotCreated();
        }

        private void AddARPaymentJounal()
        {
            ARPaymentsJournalRepository arPaymentsJournalRepository = new ARPaymentsJournalRepository(tenant);
            ARPaymentsJournal arPaymentsJournal = new ARPaymentsJournal();
            arPaymentsJournal.Tenant = tenant;
            arPaymentsJournal.IsVoided = false;
            arPaymentsJournal.PaymentId = paymentPM.Id;
            arPaymentsJournalRepository.Add(arPaymentsJournal);
            arPaymentsJournalRepository.SubmitChanges();
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

                // ValidateIfAllReplicasHaveSameValueDate();
            }
            else
            {
                ARPaymentChequePM cheque = CreateARPaymentCheque();
                CreateInterestTransactionLine(cheque, paymentPM);
                AddChequeToCashbook(cheque);
                newlyAddedCheque = cheque;
            }

        }
        private int originalEntityLineNumber = 0;
        private void CreateInterestTransactionLine(ARPaymentChequePM cheque, ARPaymentPM payment)
        {
            if (payment.BillToPartnerTypeId == PartnerTypeId_Customer)
            {
                InterestTransactionPM interestTransaction = MapInterestTransactionPMFromARPaymentPM(cheque, payment);
                IInterestTransactionUpdateServiceExt interestTransactionUpdateService = ContainerAccessor.Container.Resolve(typeof(IInterestTransactionUpdateServiceExt), "InterestTransactionUpdateServiceExt", new ParameterOverride("", 1)) as IInterestTransactionUpdateServiceExt;
                interestTransactionUpdateService.Create(interestTransaction);

            }
        }

        private void CreateInterestTransactionLineForBankTransfer(ARPaymentBankTranferPM bankTranfer, ARPaymentPM payment)
        {
            if (payment.BillToPartnerTypeId == PartnerTypeId_Customer)
            {
                InterestTransactionPM interestTransaction = MapInterestTransactionPMFromBankTransferARPaymentPM(bankTranfer, payment);
                IInterestTransactionUpdateServiceExt interestTransactionUpdateService = ContainerAccessor.Container.Resolve(typeof(IInterestTransactionUpdateServiceExt), "InterestTransactionUpdateServiceExt", new ParameterOverride("", 1)) as IInterestTransactionUpdateServiceExt;
                interestTransactionUpdateService.Create(interestTransaction);
            }
        }

        private InterestTransactionPM MapInterestTransactionPMFromARPaymentPM(ARPaymentChequePM cheque, ARPaymentPM payment)
        {
            DateTime? dateForInterest = payment.ValueDate == null ? DateTime.Now : payment.ValueDate;
            GLAccountPM account = GetGLAccount(payment.BillToId, payment.Tenant);
            InterestTransactionPM interestTransaction = new InterestTransactionPM()
            {
                InterestEntityTypeCode = "2",
                EntityId = payment.Id,
                OriginalEntityLineNumber = cheque.LineNumber,
                LocalAmount = cheque.LocalAmount * -1,
                ForeignAmount = cheque.ForeignAmount * -1,

                InterestValueDate = (DateTime)cheque.ValueDate,
                Tenant = paymentPM.Tenant,
                GLAccountId = account != null ? account.Id : null,
                ChangeSetOp = ChangeSetOperation.Insert,
                CurrencyId = payment.PaymentCurrencyId,
            };
            return interestTransaction;
        }

        private InterestTransactionPM MapInterestTransactionPMFromBankTransferARPaymentPM(ARPaymentBankTranferPM bankTranfer, ARPaymentPM payment)
        {
            GLAccountPM account = GetGLAccount(payment.BillToId, payment.Tenant);
            InterestTransactionPM interestTransaction = new InterestTransactionPM()
            {
                InterestEntityTypeCode = "2",
                EntityId = payment.Id,
                OriginalEntityLineNumber = bankTranfer.LineNumber,
                LocalAmount = bankTranfer.LocalAmount * -1,
                ForeignAmount = bankTranfer.ForeignAmount * -1,
                InterestValueDate = (DateTime)bankTranfer.ValueDate,
                Tenant = paymentPM.Tenant,
                GLAccountId = account != null ? account.Id : null,
                ChangeSetOp = ChangeSetOperation.Insert,
                CurrencyId = payment.PaymentCurrencyId,
            };
            return interestTransaction;
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
                CreateInterestTransactionLine(cheque, paymentPM);
            }
        }

        public void AddNewBankTransfers()
        {
            int LineNumberCounter = GetInitialLineNumberForBankTransfer(paymentPM);
            if (paymentPM.ARPaymentBankTranfers != null && paymentPM.ARPaymentBankTranfers.Any()) {
                foreach (ARPaymentBankTranferPM bankTransfer in paymentPM.ARPaymentBankTranfers)
                {
                    ARPaymentBankTranferPM aRPaymentBankTranferPM = InitializeARPaymentBankTransfer(paymentPM, bankTransfer, ref LineNumberCounter);
                    SaveARPaymentBankTranfer(aRPaymentBankTranferPM);
                    CreateInterestTransactionLineForBankTransfer(aRPaymentBankTranferPM, paymentPM);
                }
            }
            else
            {
                ARPaymentBankTranferPM aRPaymentBankTranferPM = CreateFirstARPaymentBankTransfer(paymentPM);
                SaveARPaymentBankTranfer(aRPaymentBankTranferPM);
                CreateInterestTransactionLineForBankTransfer(aRPaymentBankTranferPM, paymentPM);
            }
        }
        private void SaveARPaymentBankTranfer(ARPaymentBankTranferPM aRPaymentBankTranfer)
        {   
            ARPaymentBankTranfer poco = new ARPaymentBankTranfer();
            ARPaymentBankTranferMapping.MapEntity(aRPaymentBankTranfer, poco, true);
            
            paymentBankTranferRepository.Add(poco);
            paymentBankTranferRepository.SubmitChanges();
        }

        private int GetInitialLineNumberForBankTransfer(ARPaymentPM arpaymentPM)
        {
            int LineNumberCounter = 1;
            if (!isNewEntity)
                LineNumberCounter = arpaymentPM.ARPaymentBankTranfers.Max(d => d.LineNumber) + 1;
            return LineNumberCounter;
        }

        private ARPaymentBankTranferPM InitializeARPaymentBankTransfer(ARPaymentPM arpaymentPM, ARPaymentBankTranferPM bankTranfer, ref int lineNumberCounter)
        {
            ARPaymentBankTranferPM aRPaymentBankTranfer = new ARPaymentBankTranferPM
            {
                Id = IdCounter.GetNumber("ARPaymentBankTranfer", arpaymentPM.Tenant).ToString(),
                LineNumber = lineNumberCounter++,
                PaymentId = arpaymentPM.Id,
                Tenant = arpaymentPM.Tenant,
                PaymentRef = bankTranfer.PaymentRef,
                ValueDate = bankTranfer.ValueDate,
                BankAccountId = bankTranfer.BankAccountId,
                ExchageRate = (decimal)arpaymentPM.PaymentCurrencyExchangeRate,
                CurrencyId = arpaymentPM.PaymentCurrencyId,
                LocalAmount = (bankTranfer.ForeignAmount * (decimal)arpaymentPM.PaymentCurrencyExchangeRate),
                ForeignAmount = (decimal)bankTranfer.ForeignAmount,
                ChangeSetOp = ChangeSetOperation.Insert
            };
            return aRPaymentBankTranfer;
        }

        private ARPaymentBankTranferPM CreateFirstARPaymentBankTransfer(ARPaymentPM arpaymentPM)
        {
            ARPaymentBankTranferPM aRPaymentBankTranfer = new ARPaymentBankTranferPM
            {
                Id = IdCounter.GetNumber("ARPaymentBankTranfer", arpaymentPM.Tenant).ToString(),
                LineNumber = 1,
                PaymentId = arpaymentPM.Id,
                Tenant = arpaymentPM.Tenant,
                PaymentRef = arpaymentPM.ChequeOrPaymentRef,
                ValueDate = arpaymentPM.ValueDate.Value,
                BankAccountId = arpaymentPM.BankAccountId,
                ExchageRate = (decimal)arpaymentPM.PaymentCurrencyExchangeRate,
                CurrencyId = arpaymentPM.PaymentCurrencyId,
                LocalAmount = (decimal)arpaymentPM.AmountInLocalCurrency.Value,
                ForeignAmount = (decimal)arpaymentPM.AmountInPaymentCurrency.Value,
                ChangeSetOp = ChangeSetOperation.Insert
            };
            paymentPM.ARPaymentBankTranfers.Add(aRPaymentBankTranfer);
            return aRPaymentBankTranfer;
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
            return InitializeARPaymentChequeFromReplica(arpaymentPM, chequeReplica);

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
                LocalAmount = (chequeReplica.ForeignAmount * (decimal)arpaymentPM.PaymentCurrencyExchangeRate),
                ForeignAmount = (decimal)chequeReplica.ForeignAmount,
                ChangeSetOp = ChangeSetOperation.Insert,
                StatusCode = "1", // In Cashbook  
                ExchangeRate = (decimal)arpaymentPM.PaymentCurrencyExchangeRate,
                PaymentNumber = arpaymentPM.PaymentNo,
                Id = chequeReplica.Id,
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
            ICashBookQueryServiceExt cashBookQueryService = ContainerAccessor.Container.Resolve(typeof(ICashBookQueryServiceExt), "CashBookQueryServiceExt", new ParameterOverride("", 1)) as ICashBookQueryServiceExt;
            bool paymentChequeCashbookLineExist = cashBookQueryService.CheckIfCashbookLineCreatedForPaymentCheque(cashBookLine.CashBookId, cashBookLine.ARPChequeId, cashBookLine.Tenant);
            if (!paymentChequeCashbookLineExist)
            {
                SubmitCashbookLine(cashBookLine);
            }
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
            journal = GetNewJournalForPayment();
            int counter = 0;

            CreateCreditLines(ref counter);
            CreateDebitLines(counter);
            CheckAbiltiyOfCreatingAutomaticReconcileForJournal();

            AutoExternalReconcileBankTransferPageLines();

            SubmitJournal();
            return journal;
        }

        private void AutoExternalReconcileBankTransferPageLines()
        {
            int JournalExternalReconcileLine = 1;

            if (paymentPM.ReconcileExternalPagesIds == null)
                return;

            List<JournalExternalReconcilePM> externalJournalReconciles =
                paymentPM.ReconcileExternalPagesIds.Split(',')?.Select(lineId => new JournalExternalReconcilePM()
                {
                    Tenant = journal.Tenant,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    JournalId = journal.Id,
                    Line = JournalExternalReconcileLine++,
                    //LedgerTransactionId = myLedgerTransactionTransferPM.Id,
                    ReconcileExternalPageLineId = lineId,
                    SkipAccountsValidation = true
                }).ToList();

            journal.JournalExternalReconciles = externalJournalReconciles;
        }

        private void CheckAbiltiyOfCreatingAutomaticReconcileForJournal()
        {
            if ((paymentPM.ARPaymentChequeReplicas.Count() > 1 || (paymentPM.ARPaymentBankTranfers != null && paymentPM.ARPaymentBankTranfers.Count() > 1)) && paymentPM.PaymentInvoices.Count() > 0)
            {
                //  throw new Exception("Can't Perform ARPayment Reconciliation  in multiple Cheques case");
            } 
            else
            {
                CreateAutomaticReconcileForJournal();
            }
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

        private void SubmitJournal()
        {
            IJournalUpdateServiceExt journalUpdate = ContainerAccessor.Container.Resolve(typeof(IJournalUpdateServiceExt), "JournalUpdateServiceExt", new ParameterOverride("", 1)) as IJournalUpdateServiceExt;
            journalUpdate.Update(journal);
        }
        private void CreateAutomaticReconcileForJournal()
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

        private string GetGLAccountIdByBankAccountId(ARPaymentBankTranferPM bankTranfer) {
            string glaAccountId = "";
            IBankAccountQueryServiceExt bankAccountQuery = ContainerAccessor.Container.Resolve(typeof(IBankAccountQueryServiceExt), "BankAccountQueryServiceExt", new ParameterOverride("", 1)) as IBankAccountQueryServiceExt;
            BankAccountPM bankAccount = bankAccountQuery.GetByFirstOrDefault(bankTranfer.BankAccountId, bankTranfer.Tenant);
            if (bankAccount != null)
            {
                glaAccountId = bankAccount.GLAccountId;
            }
            return glaAccountId;
        }

        private int CreateDebitLinesForEachCheque(JournalPM journal, int counter)
        {
            if (paymentPM.ARPaymentChequeReplicas.Count > 0)
                CreateJournalLineForEachReplica(ref counter, "2");
            else
                CreateJournalLineForARPaymentCheque(newlyAddedCheque, "2", ref counter);
            return counter;
        }

        private int CreateDebitLinesForEachBankTransfer(JournalPM journal, int counter)
        {
            if (paymentPM.ARPaymentBankTranfers != null && paymentPM.ARPaymentBankTranfers.Count > 0)
                CreateJournalLineForEachBankTransfer(ref counter, "2");
            return counter;
        }
        
        private int CreateJournalLineForARPaymentCheque(ARPaymentChequePM arPaymentcheque, string actionCode, ref int counter)
        {
            GLAccountPM paymentGLAccount = GetPaymentGLAccount();

            var debitLine = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = ++counter,
                ActionCode = actionCode,
                ActionTypeCodeEnum = actionCode == "2" ? JournalActionTypeEnum.Debit : JournalActionTypeEnum.Credit,
                DocumentDate = paymentPM.RegisterDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = arPaymentcheque.ValueDate,
                LocalAmount = arPaymentcheque.ForeignAmount * (decimal)paymentPM.PaymentCurrencyExchangeRate,
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

        private int CreateCreditLinesForEachCheque(ref int counter)
        {
            if (paymentPM.ARPaymentChequeReplicas.Count > 0)
                CreateJournalLineForEachReplica(ref counter, "1");
            else
                CreateJournalLineForARPaymentCheque(newlyAddedCheque, "1", ref counter); return counter;
        }

        private void CreateCreditLinesForEachBankTransfer(ref int counter)
        {
            if (paymentPM.ARPaymentBankTranfers != null && paymentPM.ARPaymentBankTranfers.Count > 0)
                CreateJournalLineForEachBankTransfer(ref counter, "1");
        }

        private void CreateCreditLines(ref int counter)
        {
            if (paymentPM.AccountingPaymentMethodCode == "CH")
            {
                CreateCreditLinesForEachCheque(ref counter);
            } else if (paymentPM.AccountingPaymentMethodCode == "BT")
            {
                CreateCreditLinesForEachBankTransfer(ref counter);
            }
            else
            {
                CreateCreditJournalLineForNonChequePaymnet(ref counter);
            }

        }
        private void CreateCreditJournalLineForNonChequePaymnet(ref int counter)
        {
            GLAccountPM paymentGLAccount = GetPaymentGLAccount();

            JournalLinePM creditLine = new JournalLinePM
            {
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                Line = ++counter,
                JournalId = journal.Id,
                ActionCode = "1",//- Credit
                ActionTypeCodeEnum = JournalActionTypeEnum.Credit,
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

        }
        private int CreateDebitLines(int counter)
        {
            if (paymentPM.AccountingPaymentMethodCode == "CH")
                counter = CreateDebitLinesForEachCheque(journal, counter);
            else if (paymentPM.AccountingPaymentMethodCode == "BT")
                counter = CreateDebitLinesForEachBankTransfer(journal, counter);
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
                ActionTypeCodeEnum = JournalActionTypeEnum.Debit,
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
                ChangeSetOp = ChangeSetOperation.Insert,
                Notes = paymentPM.PrintNotes,
            };
            journal.JournalLines.Add(debitLine);
            return counter;
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


        private void CreateJournalLineForEachReplica(ref int counter, string actionCode)
        {
            foreach (ARPaymentChequeReplicaPM cheque in paymentPM.ARPaymentChequeReplicas)
            {
                JournalLinePM journalLine = MapPaymentChequeJournalLineFields(cheque, actionCode, ref counter);
                journal.JournalLines.Add(journalLine);
            }
        }

        private void CreateJournalLineForEachBankTransfer(ref int counter, string actionCode)
        {
            if (paymentPM.ARPaymentBankTranfers == null)
            {
                return;
            }
            foreach (ARPaymentBankTranferPM bankTransfer in paymentPM.ARPaymentBankTranfers)
            {
                JournalLinePM journalLine = MapPaymentBankTransferJournalLineFields(bankTransfer, actionCode, ref counter);
                journal.JournalLines.Add(journalLine);
            }
        }

        private JournalLinePM MapPaymentChequeJournalLineFields(ARPaymentChequeReplicaPM cheque, string actionCode, ref int counter)
        {
            var journalLine = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = ++counter,
                ActionCode = actionCode,
                ActionTypeCodeEnum = actionCode == "2" ? JournalActionTypeEnum.Debit : JournalActionTypeEnum.Credit,
                DocumentDate = paymentPM.RegisterDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = cheque.ValueDate,
                LocalAmount = cheque.ForeignAmount * (decimal)paymentPM.PaymentCurrencyExchangeRate,
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
            return journalLine;
        }

        private JournalLinePM MapPaymentBankTransferJournalLineFields(ARPaymentBankTranferPM bankTransfer, string actionCode, ref int counter)
        {
            var journalLine = new JournalLinePM
            {
                Tenant = tenant,
                JournalId = journal.Id,
                Line = ++counter,
                ActionCode = actionCode,
                ActionTypeCodeEnum = actionCode == "2" ? JournalActionTypeEnum.Debit : JournalActionTypeEnum.Credit,
                DocumentDate = paymentPM.RegisterDate.Value,
                AccountingDate = paymentPM.RegisterDate.Value,
                DueDate = bankTransfer.ValueDate,
                LocalAmount = bankTransfer.ForeignAmount * (decimal)paymentPM.PaymentCurrencyExchangeRate,
                CurrencyId = paymentPM.PaymentCurrencyId,
                ForeignAmount = bankTransfer.ForeignAmount,
                ExchangeRate = (decimal)paymentPM.PaymentCurrencyExchangeRate,
                Reference1 = paymentPM.PaymentNo,
                Reference2 = bankTransfer.PaymentRef,
                DebitAccountId = GetGLAccountIdByBankAccountId(bankTransfer),
                CreditAccountId = paymentGLAccount != null ? paymentGLAccount.Id : null,
                ChangeSetOp = ChangeSetOperation.Insert,
                Notes = paymentPM.PrintNotes
            };
            return journalLine;
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
