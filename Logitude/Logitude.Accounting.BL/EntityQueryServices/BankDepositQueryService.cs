using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using System.Web;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.EntityQueryServices
{
    public partial class BankDepositQueryService : EntityQueryService<BankDeposit, BankDepositKeys, BankDepositPM, object, BankDepositKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, BankDepositPM entityPM)
        {
            entityPM.BankDepositLines = GetLines(entityPM);

            //IAccountingContext context = MainContext as AccountingContext;
            //BankDepositKeys bankDepositKeys = entityKeys as BankDepositKeys;
            //BankDepositLineQueryService bankDepositLineQueryService = new BankDepositLineQueryService(context);
            //entityPM.BankDepositLines = bankDepositLineQueryService.GetMulti(bankDepositKeys, true);

        }

        public List<BankDepositLinePM> GetLines(BankDepositPM depositPM)
        {
            BankDepositLineQueryService bankDepositLineQuery = new BankDepositLineQueryService(context);
            return bankDepositLineQuery.GetLinesJoinedWithCheques(new List<string>() { depositPM.Id }, depositPM.Tenant);
        }

        public void ReturnCheque(string bankDepositId, string arpChequeId, string returnType, string notes, int tenant)
        {
            if (notes == "undefined" || notes == "null") notes = null;
            // -- RETURN LOGIC
            //
            // (returnType): Cashbook
            //   Update StatusCode.ARPaymentCheques = 4 - Cheque out of deposit
            //   Update the fields IsOutOfDeposit = True & OutOfDepositDate = Current date & Notes
            //   Update TotalAmount.Cashbooks += ForeignAmount.ARPaymentCheques
            //   Update the field IsDeposited.CashbookLines = False
            //   Update ForeignAmount.BankDeposits -= ForeignAmount.ARPaymentCheques
            //   Update LocalAmount.BankDeposits -= LocalAmount.ARPaymentCheques
            //   Event(for the cheque): Cheque out of deposit-returned to cashbook-המחאה הוצאה מהפקדה
            //   Event(for the Deposit): Cheque returned to cashbook-Out of deposit-המחאה XXXX הוצאה מהפקדה
            //   create journal and refresh the page
            //
            // (returnType): Customer
            //   Update StatusCode.ARPaymentCheques = 5 - Returned to Customer
            //   Update the fields IsOutOfDeposit = True & OutOfDepositDate = Current date & Notes
            //   Update the field IsDeposited.CashbookLines = False
            //   Update TotalAmount.Cashbooks += ForeignAmount.ARPaymentCheques
            //   Update ForeignAmount.BankDeposits -= ForeignAmount.ARPaymentCheques
            //   Update LocalAmount.BankDeposits -= LocalAmount.ARPaymentCheques
            //   Event(for the ARPayment): Cheque out of deposit-returned to Customer המחאה הוצאה מהפקדה והוחזרה ללקוח
            //   Event(for the Deposit): Cheque Out of deposit and returned to Custome-המחאה XXXX הוצאה מהפקדה והוחזרה ללקוח
            //   Create Journal with 4 lines
            //   Debit Cashbook and Credit Bank -See Here
            //   Debit Customer Credit Cashbook - See here
            //   Refresh the page

            bool isValid = CheckCheque(arpChequeId, tenant);
            if (!isValid) return;

            if (returnType == "Cashbook")
            {
                ReturnChequeToCashbook(bankDepositId, arpChequeId, notes, tenant);
            }

            //
            // RETURN TO CUSTOMER 
            //
            else if (returnType == "Customer")
            {
                throw new ApplicationException("Cannot return cheque to customer right now!"); // due to a task for that
                //ReturnChequeToCustomer(bankDepositId, arpChequeId, notes, tenant);
            }






        }

        private bool CheckCheque(string arpChequeId, int tenant)
        {
            bool isValid = true;
            bool showLocal = false;

            //show local 
            ContactPM loggedContact = GetLoggedContact(tenant);
            if(loggedContact != null) showLocal = !loggedContact.DontShowLocal;

            //get cheque
            ARPaymentChequeQueryService aRPaymentChequeQuery = new ARPaymentChequeQueryService(tenant);
            ARPaymentChequePM cheque = aRPaymentChequeQuery.GetSingle(arpChequeId, false, false);
            if (cheque != null)
            {

                // Check reedemed cheuqe , Task 44667: Deposits: New validation before out of deposit action
                if (cheque.StatusCode == "6") // 6- redemmed
                {
                    isValid = false;
                    throw new ApplicationException(TextCodesTranslator.TranslateText("Accounting.O.RedeemedChequeMSG", tenant, showLocal));
                }
            }

            return isValid;
        }

        private void ReturnChequeToCashbook(string bankDepositId, string arpChequeId, string notes, int tenant)
        {
            if (bankDepositId == null || arpChequeId == null) throw new ApplicationException("Some fields are missing! check bankDepositId, arpChequeId");

            // 1- get ARPaymentCheque
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            ARPaymentChequeUpdateService chequeUpdateService = new ARPaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            ARPaymentChequeQueryService chequeQuery = new ARPaymentChequeQueryService(tenant);
            ARPaymentChequePM chequePM = chequeQuery.GetSingle(arpChequeId, false, false);
            if (chequePM == null) throw new ApplicationException("Cannot find AR Payment Cheque! " + arpChequeId);

            //payment
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM paymentPM = paymentQuery.GetSinglePM(chequePM.PaymentId, tenant);

            // 2- Update StatusCode.ARPaymentCheques = 4 - Cheque out of deposit
            chequePM.StatusCode = "4"; // 4- Returned From Bank
            chequePM.ChangeSetOp = ChangeSetOperation.Update;
            chequeUpdateService.Update(chequePM, true);

            // 3- get bank deposit
            BankDepositQueryService depositQuery = new BankDepositQueryService(tenant);
            BankDepositUpdateService bankDepositUpdateService = new BankDepositUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            BankDepositPM depositPM = depositQuery.GetSingle(bankDepositId, true, false);
            BankDepositLinePM depositLinePM = depositPM.BankDepositLines.Find(d => d.ARPaymentChequeId == arpChequeId);

            // 4- update bank deposit
            depositLinePM.IsOutOfDeposit = true;
            depositLinePM.OutOfDepositeDate = DateTime.Now;
            depositLinePM.Notes = notes;
            depositLinePM.ChangeSetOp = ChangeSetOperation.Update;

            // 5- get and update cashbook
            CashBookQueryService cashBookQuery = new CashBookQueryService(tenant);
            CashBookUpdateService cashBookUpdateService = new CashBookUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            CashBookPM cashBookPM = cashBookQuery.GetSingle(depositPM.CashBookId, true, false);
            CashBookLinePM cashbookLinePM = cashBookPM.CashBookLines.Find(d => d.ARPChequeId == arpChequeId);

            // 6- update cashbook line
            cashbookLinePM.IsDeposited = false;
            cashbookLinePM.ChangeSetOp = ChangeSetOperation.Update;

			// 7- update totals
			if (cashBookPM.CashBookTypeCode != "1") { 
              cashBookPM.TotalAmount += chequePM.ForeignAmount;
            }
            //// depositPM.ForeignAmount -= chequePM.ForeignAmount;
            //// depositPM.LocalDepositAmount -= chequePM.LocalAmount;

            // 8- update entities
            cashBookPM.ChangeSetOp = ChangeSetOperation.Update;
            cashBookUpdateService.Update(cashBookPM, true);
            depositPM.ChangeSetOp = ChangeSetOperation.Update;
            bankDepositUpdateService.Update(depositPM, true);

            // 8- create events
            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;

            //EventTracer.CreateTraceEvent(new EventTracerArgs()
            //{
            //    EntityId = arpChequeId,
            //    Tenant = tenant,
            //    UserId = contact.Id,
            //    ObjectTableName = "ARPaymentCheque",
            //    IsAddedManually = false,
            //    EventTypeCode = "ARPR",
            //    Notes = chequePM.ChequeNumber,

            //});
            EventTracer.CreateTraceEvent(new EventTracerArgs()
            {
                Tenant = tenant,
                EventDateTime = DateTime.Now,
                EntityId = paymentPM.Id,
                UserId = contact.Id,
                ObjectTableName = "ARPayment",
                IsAddedManually = false,
                EventTypeCode = "R2CB",
            });
            //EventTracer.CreateTraceEvent(new EventTracerArgs()
            //{
            //    EntityId = bankDepositId,
            //    Tenant = tenant,
            //    UserId = contact.Id,
            //    ObjectTableName = "BankDeposit",
            //    IsAddedManually = false,
            //    EventTypeCode = "BDRC",
            //    Notes = chequePM.ChequeNumber,

            //});

            // 9- create journal
            JournalPM journalPM = new JournalPM()
            {
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                UpdateDate = DateTime.Now,
                UpdatedByUserId = contact.Id,
                ApproveDate = DateTime.Now,
                ApprovedByUserId = contact.Id,
                CreateDate = DateTime.Now,
                CreatedByUserId = contact.Id,
                IsVoided = false,
                QueueId = null,

                AccountingDate = DateTime.Now,                //depositPM.AccountingDate,
                TypeCode = "0",                     // 0- Manual
                StatusCode = "6",                   // 2- Approved
                AccountingEntityCode = "6",         // 6- Deposit
                AccountingEntityId = depositPM.Id,
                AccountingEntityReference = depositPM.DepositNumber.ToString(),

                //JournalNumber = xxx,
                //AccountingEntityReference = xxxx,
                //OriginalJournalId = xxxx,
                //UpdatedByUserName = xxxx,
                //ApprovedByUserName = xxxx,
                //SearchFields = xxxx,
                //VoidedByUserId = xxxx,
                //VoidDate = xxxx,
                //OriginalJournalName = xxxx,
                //VoidedByUserName = xxxx,
                //VoidedByJournalId = xxxx,
                //ExternalSystem = xxxx,
                //StatusLocalName = xxxx,
            };

            // create credit journal line
            JournalLinePM journalLineCredit = new JournalLinePM()
            {
                Line = 1,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ActionCode = "1", // 1- Credit
                Notes = notes,

                AccountingDate = DateTime.Now,                //depositPM.AccountingDate,
                DueDate = depositPM.AccountingDate,
                DocumentDate = depositPM.AccountingDate,
                ForeignAmount = chequePM.ForeignAmount,
                LocalAmount = chequePM.LocalAmount,
                CurrencyId = chequePM.CurrencyId,
                ExchangeRate = chequePM.ExchangeRate,
                Reference1 = chequePM.ChequeNumber,

                //JournalId = journal.Id,
                //DebitAccountId = glAccountId,
                //DebitControlAccountId = xxxx,
                //CreditControlAccountId = xxxx,
                //DocumentDate = _referenceDate,
                //Reference2 = xxxx,
                //Reference3 = xxxx,
                //ActionName = xxxx,
                //DebitControlAccountName = xxxx,
                //CreditAccountName = xxxx,
                //DebitAccountName = xxxx,
                //CreditControlAccountName = xxxx,
                //CreditControlAccountNumber = xxxx,
                //DebitControlAccountNumber = xxxx,
                ////CreditAccountNumber = xxxx,
                //DebitAccountNumber = xxxx,
                //CurrencyName = xxxx,
                //CurrencyCode = xxxx,
                //ActionTypeCode = xxxx,
                //ExternalOpenAmount = xxxx,
                //IsCreditAccountMulti = false,
                //IsDebitAccountMulti = xxxx,

            };
            //select account
            if (chequePM.ValueDate < DateTime.Now)
            {
                BankAccountQueryService banksQuery = new BankAccountQueryService(accountingContext);
                BankAccountPM bankPM = banksQuery.GetSingle(depositPM.DepositBankAccountId, false, false);

                journalLineCredit.CreditAccountId = bankPM.GLAccountId;
            }
            else
            {
                journalLineCredit.CreditAccountId = depositPM.DeferredGLAccountId;
            }


            GLAccountQueryService glaQuery = new GLAccountQueryService(accountingContext);
            GLAccountPM glaccountPM = glaQuery.GetSingle(journalLineCredit.CreditAccountId, false, false);
            journalLineCredit.CreditControlAccountId = glaccountPM.ControlAccountId;




            // create debit journal line
            JournalLinePM journalLineDebit = new JournalLinePM()
            {
                Line = 2,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ActionCode = "2", // 2- Debit
                Notes = notes,

                AccountingDate = DateTime.Now,                //depositPM.AccountingDate,
                DueDate = depositPM.AccountingDate,
                DocumentDate = depositPM.AccountingDate,
                ForeignAmount = chequePM.ForeignAmount,
                LocalAmount = chequePM.LocalAmount,
                CurrencyId = chequePM.CurrencyId,
                ExchangeRate = chequePM.ExchangeRate,
                Reference1 = chequePM.ChequeNumber,


                //JournalId = journal.Id,
                //DebitAccountId = glAccountId,
                //DebitControlAccountId = xxxx,
                //CreditControlAccountId = xxxx,
                //DocumentDate = _referenceDate,
                //Reference2 = xxxx,
                //Reference3 = xxxx,
                //ActionName = xxxx,
                //DebitControlAccountName = xxxx,
                //CreditAccountName = xxxx,
                //DebitAccountName = xxxx,
                //CreditControlAccountName = xxxx,
                //CreditControlAccountNumber = xxxx,
                //DebitControlAccountNumber = xxxx,
                ////CreditAccountNumber = xxxx,
                //DebitAccountNumber = xxxx,
                //CurrencyName = xxxx,
                //CurrencyCode = xxxx,
                //ActionTypeCode = xxxx,
                //ExternalOpenAmount = xxxx,
                //IsCreditAccountMulti = false,
                //IsDebitAccountMulti = xxxx,

            };
            //select account
            CashBookQueryService cashbookQuery = new CashBookQueryService(accountingContext);
            CashBookPM cashbookPM = cashbookQuery.GetSingle(depositPM.CashBookId, false, false);
            journalLineDebit.DebitAccountId = cashbookPM.AccountId;

            GLAccountPM glaccountPM2 = glaQuery.GetSingle(cashbookPM.AccountId, false, false);
            journalLineDebit.DebitControlAccountId = glaccountPM2.ControlAccountId;

            // opposit account
            journalLineDebit.CreditAccountId = journalLineCredit.CreditAccountId;
            journalLineCredit.DebitAccountId = journalLineDebit.DebitAccountId;
            journalPM.JournalLines.Add(journalLineCredit);
            journalPM.JournalLines.Add(journalLineDebit);

            AutoReconcileChequesTransactions(bankDepositId, tenant, chequePM, journalPM);


            // save journal
            JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            journalUpdateService.Update(journalPM, true);

            AddAccountingEntityJournal(journalPM, AccountingEntityJournalActions.BankDepositOutOfDeposit, arpChequeId);

        }


        public void AddAccountingEntityJournal(JournalPM journal,string actionName, string childEntityId = null)
        {
            IAccountingContext context = AccountingContext.GetContext(journal.Tenant);
            AccountingEntityJournalUpdateService service = new AccountingEntityJournalUpdateService(context, new Dictionary<string, IContext>(), journal.Tenant);
            service.AddAccountingEntitieJournal(journal, actionName, childEntityId);
        }

        private void AutoReconcileChequesTransactions(string bankDepositId, int tenant, ARPaymentChequePM chequePM, JournalPM journalPM)
        {
            List<LedgerTransactionPM> chequeTransactions = GetChequeTransactionsByDeposit(bankDepositId, tenant, chequePM);

            bool haveReconciledTransactions = chequeTransactions.Any(transaction => transaction.IsReconciled == true);
            if (haveReconciledTransactions)
                return;

            CreateJournalReconcileForTransactions(journalPM, chequeTransactions);
        }

        private void CreateJournalReconcileForTransactions(JournalPM journalPM, List<LedgerTransactionPM> chequeTransactions)
        {
            var count = 1;
            foreach (var transaction in chequeTransactions)
            {
                var journalReconcile = new JournalReconcilePM()
                {
                    ChangeSetOp = ChangeSetOperation.Insert,
                    Tenant = journalPM.Tenant,
                    JournalId = journalPM.Id,
                    LedgerTransactionId = transaction.Id,
                    Line = count++,
                    CurrencyId = transaction.CurrencyId,
                    ReconciliationAmount = transaction.ForeignAmountDebit - transaction.ForeignAmountCredit,
                    IsPartial = false
                };
                journalPM.JournalReconciles.Add(journalReconcile);
            }
        }

        private List<LedgerTransactionPM> GetChequeTransactionsByDeposit(string bankDepositId, int tenant, ARPaymentChequePM chequePM)
        {
            List<LedgerTransactionPM> depositTransactions = GetDepositTransactions(bankDepositId, tenant);
            BankDepositLineQueryService bankDepositLineQueryService = new BankDepositLineQueryService(tenant);
            List<int> bankDepositLineNumbers = bankDepositLineQueryService.GetDepositLineNumbersByDepositIdChequeId(bankDepositId, chequePM.Id, tenant);
            List<LedgerTransactionPM> chequeTransactions;
            if (bankDepositLineNumbers != null)
            {
                chequeTransactions = depositTransactions.Where(transaction => transaction.Reference2 == chequePM.ChequeNumber && bankDepositLineNumbers.Contains(transaction.JournalLineNumber)).ToList();
            }
            else
            {
                chequeTransactions = depositTransactions.Where(transaction => transaction.Reference2 == chequePM.ChequeNumber).ToList();
            }
            return chequeTransactions;
        }

        private List<LedgerTransactionPM> GetDepositTransactions(string bankDepositId, int tenant)
        {
            LedgerTransactionQueryService ledgerTransactionQuery = new LedgerTransactionQueryService(tenant);
            List<LedgerTransactionPM> depositTransactions = ledgerTransactionQuery.GetTransactionBySourceEntity(bankDepositId, AccountingEntityValues.ChequeDeposit, tenant);
            return depositTransactions;
        }

        private void ReturnChequeToCustomer(string bankDepositId, string arpChequeId, string notes, int tenant)
        {
            if (bankDepositId == null || arpChequeId == null) throw new ApplicationException("Some fields are missing! check bankDepositId, arpChequeId");



            // 1- get ARPaymentCheque
            IAccountingContext accountingContext = AccountingContext.GetContext(tenant);
            ARPaymentChequeUpdateService chequeUpdateService = new ARPaymentChequeUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            ARPaymentChequeQueryService chequeQuery = new ARPaymentChequeQueryService(tenant);
            ARPaymentChequePM chequePM = chequeQuery.GetSingle(arpChequeId, false, false);
            if (chequePM == null) throw new ApplicationException("Cannot find AR Payment Cheque! " + arpChequeId);

            //payment
            IInvoiceContext invoiceContext = InvoiceContext.GetContext(tenant);
            ARPaymentQuery paymentQuery = new ARPaymentQuery(tenant);
            ARPaymentPM paymentPM = paymentQuery.GetSinglePM(chequePM.PaymentId, tenant);


            // 2- Update StatusCode.ARPaymentCheques = 5 
            chequePM.StatusCode = "5"; // 5 - Returned to Customer
            chequePM.ChangeSetOp = ChangeSetOperation.Update;
            chequeUpdateService.Update(chequePM, true);

            // 3- get bank deposit
            BankDepositQueryService depositQuery = new BankDepositQueryService(tenant);
            BankDepositUpdateService bankDepositUpdateService = new BankDepositUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            BankDepositPM depositPM = depositQuery.GetSingle(bankDepositId, true, false);
            BankDepositLinePM depositLinePM = depositPM.BankDepositLines.Find(d => d.ARPaymentChequeId == arpChequeId);

            // 4- update bank deposit
            depositLinePM.IsOutOfDeposit = true;
            depositLinePM.OutOfDepositeDate = DateTime.Now;
            depositLinePM.Notes = notes;
            depositLinePM.ChangeSetOp = ChangeSetOperation.Update;

            // 5- get and update cashbook
            CashBookQueryService cashBookQuery = new CashBookQueryService(tenant);
            CashBookUpdateService cashBookUpdateService = new CashBookUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            CashBookPM cashBookPM = cashBookQuery.GetSingle(depositPM.CashBookId, true, false);
            CashBookLinePM cashbookLinePM = cashBookPM.CashBookLines.Find(d => d.ARPChequeId == arpChequeId);

            // 6- update cashbook line
            cashbookLinePM.IsDeposited = false;
            cashbookLinePM.ChangeSetOp = ChangeSetOperation.Update;

            // 7- update totals
            if (cashBookPM.CashBookTypeCode != "1")
            {
                cashBookPM.TotalAmount += chequePM.ForeignAmount;
            }
            //// depositPM.ForeignAmount -= chequePM.ForeignAmount;
            //// depositPM.LocalDepositAmount -= chequePM.LocalAmount;

            // 8- update entities
            cashBookPM.ChangeSetOp = ChangeSetOperation.Update;
            cashBookUpdateService.Update(cashBookPM, true);
            depositPM.ChangeSetOp = ChangeSetOperation.Update;
            bankDepositUpdateService.Update(depositPM, true);

            // 8- create events
            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;


			EventTracer.CreateTraceEvent(new EventTracerArgs()
			{
				Tenant = tenant,
				EventDateTime = DateTime.Now,
				EntityId = paymentPM.Id,
				UserId = contact.Id,
				ObjectTableName = "ARPayment",
				IsAddedManually = false,
				EventTypeCode = "R2CS",
			});
			//EventTracer.CreateTraceEvent(new EventTracerArgs()
			//{
			//    EntityId = bankDepositId,
			//    Tenant = tenant,
			//    UserId = contact.Id,
			//    ObjectTableName = "BankDeposit",
			//    IsAddedManually = false,
			//    EventTypeCode = "BDRC",
			//    Notes = chequePM.ChequeNumber,

			//});


			// 9- create journal
			JournalPM journalPM = new JournalPM()
            {
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                UpdateDate = DateTime.Now,
                UpdatedByUserId = contact.Id,
                ApproveDate = DateTime.Now,
                ApprovedByUserId = contact.Id,
                CreateDate = DateTime.Now,
                CreatedByUserId = contact.Id,
                IsVoided = false,
                QueueId = null,

                AccountingDate = depositPM.AccountingDate,
                TypeCode = "0",                     // 0- Manual
                StatusCode = "6",                   // 2- Approved
                AccountingEntityCode = "6",         // 6- Deposit
                AccountingEntityId = depositPM.Id,
                AccountingEntityReference = depositPM.DepositNumber.ToString(),

            };

            // --------------------------------------------------------------
            // ** create (Credit-Bank) journal line
            JournalLinePM journalLineCreditBank = new JournalLinePM()
            {
                Line = 1,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ActionCode = "1", // 1- Credit
                Notes = notes,

                AccountingDate = depositPM.AccountingDate,
                DueDate = DateTime.Now,
                DocumentDate = DateTime.Now,
                ForeignAmount = chequePM.ForeignAmount,
                LocalAmount = chequePM.LocalAmount,
                CurrencyId = chequePM.CurrencyId,
                ExchangeRate = chequePM.ExchangeRate,

                Reference1 = chequePM.ChequeNumber,
                Reference2 = depositPM.DepositNumber.ToString(),

            };
            BankAccountQueryService banksQuery = new BankAccountQueryService(accountingContext);
            BankAccountPM bankPM = banksQuery.GetSingle(depositPM.DepositBankAccountId, false, false);
            journalLineCreditBank.CreditAccountId = bankPM.GLAccountId;
            //control
            GLAccountQueryService glaQuery = new GLAccountQueryService(accountingContext);
            GLAccountPM glaccountPM = glaQuery.GetSingle(journalLineCreditBank.CreditAccountId, false, false);
            journalLineCreditBank.CreditControlAccountId = glaccountPM.ControlAccountId;
            journalPM.JournalLines.Add(journalLineCreditBank);
            //

            // --------------------------------------------------------------
            // ** create (debit cashbook) journal line
            JournalLinePM journalLineDebitCashbook = new JournalLinePM()
            {
                Line = 2,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ActionCode = "2", // 2- Debit
                Notes = notes,

                AccountingDate = depositPM.AccountingDate,
                DueDate = DateTime.Now,
                DocumentDate = DateTime.Now,
                ForeignAmount = chequePM.ForeignAmount,
                LocalAmount = chequePM.LocalAmount,
                CurrencyId = chequePM.CurrencyId,
                ExchangeRate = chequePM.ExchangeRate,
			  Reference1 = chequePM.ChequeNumber,


		  };
            //select account
            CashBookQueryService cashbookQuery = new CashBookQueryService(accountingContext);
            CashBookPM cashbookPM = cashbookQuery.GetSingle(depositPM.CashBookId, false, false);
            journalLineDebitCashbook.DebitAccountId = cashbookPM.AccountId;
            GLAccountPM glaccountPM2 = glaQuery.GetSingle(cashbookPM.AccountId, false, false);
            journalLineDebitCashbook.DebitControlAccountId = glaccountPM2.ControlAccountId;

            journalPM.JournalLines.Add(journalLineDebitCashbook);

            // --------------------------------------------------------------

            // **
            // ** create (debit custoemr) journal line
            JournalLinePM journalLineDebitCustomer = new JournalLinePM()
            {
                Line = 2,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ActionCode = "2", // 2- Debit
                Notes = notes,

                AccountingDate = depositPM.AccountingDate,
                DueDate = DateTime.Now,
                DocumentDate = DateTime.Now,
                ForeignAmount = chequePM.ForeignAmount,
                LocalAmount = chequePM.LocalAmount,
                CurrencyId = chequePM.CurrencyId,
                ExchangeRate = chequePM.ExchangeRate,
			  Reference1 = chequePM.ChequeNumber,


		  };
            //select account

            CardQuery cardsQuery = new CardQuery(tenant);
            CardPM cardPM = cardsQuery.GetSinglePM(paymentPM.BillToId, tenant);
            if (cardPM.GLAccountId == null) throw new ApplicationException("NO CARD / GLAccountId!!");
            journalLineDebitCustomer.DebitAccountId = cardPM.GLAccountId;

            GLAccountPM glaccountPM3 = glaQuery.GetSingle(cardPM.GLAccountId, false, false);
            journalLineDebitCustomer.DebitControlAccountId = glaccountPM3.ControlAccountId;

            journalPM.JournalLines.Add(journalLineDebitCustomer);
            //
            // --------------------------------------------------------------

            // create (credit cashbook) journal line
            JournalLinePM journalLineCreditCashbook = new JournalLinePM()
            {
                Line = 1,
                Tenant = tenant,
                ChangeSetOp = ChangeSetOperation.Insert,
                ActionCode = "1", // 1- Credit
                Notes = notes,

                AccountingDate = depositPM.AccountingDate,
                DueDate = DateTime.Now,
                DocumentDate = DateTime.Now,
                ForeignAmount = chequePM.ForeignAmount,
                LocalAmount = chequePM.LocalAmount,
                CurrencyId = chequePM.CurrencyId,
                ExchangeRate = chequePM.ExchangeRate,
			  Reference1 = chequePM.ChequeNumber,

		  };
            //select account
            journalLineCreditCashbook.CreditAccountId = cashbookPM.AccountId;
            GLAccountPM glaccountPM5 = glaQuery.GetSingle(cashbookPM.AccountId, false, false);
            journalLineCreditCashbook.CreditControlAccountId = glaccountPM5.ControlAccountId;
            journalPM.JournalLines.Add(journalLineCreditCashbook);

            // --------------------------------------------------------------


            // save journal
            JournalUpdateService journalUpdateService = new JournalUpdateService(accountingContext, new Dictionary<string, IContext>(), tenant);
            journalUpdateService.Update(journalPM, true);
        }

        public void CancelDeposit(string bankDepositId, int tenant)
        {

            if (bankDepositId == null) throw new ApplicationException("No bankDepositId to cancel!!!");

            //get entity
            BankDepositPM depositPM = GetSingle(bankDepositId, true, false);

            //update
            depositPM.IsCanceled = true;
            depositPM.ChangeSetOp = ChangeSetOperation.Update;
            BankDepositUpdateService depositUpdateService = new BankDepositUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            depositUpdateService.Update(depositPM, true);
        }

        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }

        public List<ARPaymentChequePM> GetChequesOfDeposit(string banDepositId, int tenant)
        {
            BankDepositLineQueryService bankDepositLineQueryService = new BankDepositLineQueryService(context);
            List<string> paymentChequeIds = (from a in context.BankDepositLines
                                             where a.DepositId == banDepositId && a.Tenant == tenant
                                             select a.ARPaymentChequeId).ToList();

            List<ARPaymentChequePM> paymentCheques = (from a in context.ARPaymentCheques
                                                      where paymentChequeIds.Contains(a.Id) && a.Tenant == tenant
                                                      select new ARPaymentChequePM()
                                                      {
                                                          Id = a.Id,
                                                          Tenant = a.Tenant,
                                                          CurrencyCode = a.Currency.Code,
                                                          SearchFields = a.SearchFields,
                                                          LineNumber = a.LineNumber,
                                                          ChequeNumber = a.ChequeNumber,
                                                          ValueDate = a.ValueDate,
                                                          LocalAmount = a.LocalAmount,
                                                          ForeignAmount = a.ForeignAmount,
                                                          BankId = a.BankId,
                                                          BankBranch = a.BankBranch,
                                                          BankAccount = a.BankAccount,
                                                          StatusName = a.ARPaymentChequeStatus != null ? a.ARPaymentChequeStatus.EnglishName : "",
                                                          PaymentId = a.PaymentId,
                                                          ExchangeRate = a.ExchangeRate,
                                                          StatusCode = a.StatusCode,
                                                          CurrencyId = a.CurrencyId,


                                                      }).ToList();
            return paymentCheques;
        }



        private ContactPM GetLoggedContact(int tenant)
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
