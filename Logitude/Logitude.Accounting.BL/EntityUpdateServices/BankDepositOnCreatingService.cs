using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankDepositOnCreatingService: IBankDepositOnCreatingService
    {
        private IAccountingContext _MainContext;
        public BankDepositOnCreatingService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        // Main Method
        public void OnCreating(BankDepositPM entityPM)
        {
            // Id 
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant);

            // code
            if (entityPM.DepositNumber == 0) entityPM.DepositNumber = CodeCounterWrapperGetNumber(entityPM.Tenant);

            // users:
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {

                entityPM.UpdatedByUserId = user.Id;

                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = user.Id;
                }
            }

            // dates
            entityPM.UpdateDate = GetCurrentDateTime(entityPM.Tenant);
            entityPM.CreateDate = GetCurrentDateTime(entityPM.Tenant);

            // LOGIC
            foreach (BankDepositLinePM item in entityPM.BankDepositLines)
            {
                item.DepositId = entityPM.Id;
            }

            CreateJournal(entityPM, entityPM.Tenant);

            // Activity log
            LogActivity(entityPM);
         
        }

        #region Logic
        //
        // Create journal and its lines for cashbook and bank
        void CreateJournal(BankDepositPM entityPM, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);
            CashBookQueryService cashBookQueryService = new CashBookQueryService(entityPM.Tenant);
            
            // 1- Creating a New Journal
            JournalPM newJournal = new JournalPM();
            InitJournal(entityPM,  newJournal);

            // 2- Get cashbook and Validate
            CashBookPM cashBook = cashBookQueryService.GetSingle(entityPM.CashBookId, true, false);
            if (entityPM.ForeignAmount > cashBook.TotalAmount)
            {
                //showlocal
                bool showLocal = false;
                ContactPM user = LoggedContactResolver.GetLoggedContact(tenant);//GetLoggedContact(entityPM.Tenant);
                if (user != null)
                    showLocal = !user.DontShowLocal;

                throw new ApplicationException(TextCodesTranslator.TranslateText("BankDeposit.O.DepositAmountmustbelessthanCashbook", 0, showLocal));
            }


            // 3- Creating JournalLines for Cashbook Crediting
            int LineNumber = 0;
            CreateCreditJournalLines(entityPM, LineNumber, cashBook, newJournal);


            // 4- Creating JournalLines For Bank Debiting
            CreateDebitJournalLines(entityPM, LineNumber, cashBook, newJournal);

            // 5- Save Journal
            var myJournalUpdateService = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            myJournalUpdateService.Update(newJournal, true);

            // 6- Update Cashbook
            if (cashBook != null)
            {
                cashBook.ChangeSetOp = ChangeSetOperation.Update;

                // update cashbook total sum
                cashBook.TotalAmount = cashBook.TotalAmount - Math.Round(entityPM.ForeignAmount, 2); //- entityPM.LocalDepositAmount;

                var myCashBookUpdateService = new CashBookUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                myCashBookUpdateService.Update(cashBook, true);
            }

        }
        
        public void InitJournal(BankDepositPM entityPM, JournalPM newJournal)
        {
            newJournal.ChangeSetOp = ChangeSetOperation.Insert;
            newJournal.Tenant = entityPM.Tenant;
            newJournal.CreateDate = GetCurrentDateTime(entityPM.Tenant);
            newJournal.CreatedByUserId = entityPM.CreatedByUserId;
            newJournal.UpdateDate = GetCurrentDateTime(entityPM.Tenant);
            newJournal.UpdatedByUserId = entityPM.UpdatedByUserId;
            newJournal.AccountingDate = entityPM.AccountingDate;
            newJournal.TypeCode = "0"; //Manual
            newJournal.StatusCode = "2"; // Approved
            newJournal.AccountingEntityId = entityPM.Id;
            newJournal.AccountingEntityReference = entityPM.DepositNumber.ToString();
            newJournal.ExternalNo = null;
            newJournal.ApproveDate = entityPM.CreateDate;
            newJournal.ApprovedByUserId = entityPM.CreatedByUserId;


            if (entityPM.IsCashDeposit)
            {
                newJournal.AccountingEntityCode = "7"; // Cash Deposit
            }
            else
            {
                newJournal.AccountingEntityCode = "6"; // Cheque Deposit
            }
        }
        void CreateCreditJournalLines(BankDepositPM entityPM,int LineNumber, CashBookPM cashBook, JournalPM newJournal)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            ARPaymentChequeQueryService arpChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPM.Tenant);
            GLAccountPM gLAccount;

            if (entityPM.IsCashDeposit)  //Cash Deposit
            {

                LineNumber++;
                JournalLinePM newCreditJournalLine = new JournalLinePM();
                newCreditJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                newCreditJournalLine.Tenant = newJournal.Tenant;
                newCreditJournalLine.Line = LineNumber;
                newCreditJournalLine.ActionCode = "1"; //Credit
                newCreditJournalLine.DueDate = entityPM.CreateDate;
                newCreditJournalLine.LocalAmount = entityPM.LocalDepositAmount;
                newCreditJournalLine.ForeignAmount = entityPM.ForeignAmount;
                newCreditJournalLine.CurrencyId = entityPM.DepositCurrencyId;
                newCreditJournalLine.DocumentDate = entityPM.CreateDate;
                newCreditJournalLine.AccountingDate = entityPM.AccountingDate;
                newCreditJournalLine.ExchangeRate = entityPM.LocalDepositAmount / entityPM.ForeignAmount;

                newCreditJournalLine.CreditAccountId = cashBook.AccountId;
                gLAccount = gLAccountQueryService.GetSingle(cashBook.AccountId, false, true);
                if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
                {
                    newCreditJournalLine.CreditControlAccountId = gLAccount.ControlAccountId;
                }

                newCreditJournalLine.Reference1 = entityPM.DepositNumber.ToString();


                newJournal.JournalLines.Add(newCreditJournalLine);

            }
            else
            {
                foreach (BankDepositLinePM item in entityPM.BankDepositLines)
                {
                    CashBookLinePM cashBookLine = cashBook.CashBookLines.Where(d => d.ARPChequeId == item.ARPaymentChequeId).FirstOrDefault();
                    ARPaymentChequePM cheque = arpChequeQueryService.GetSingle(item.ARPaymentChequeId, false, false);

                    if (cashBook != null)
                    {
                        if (cashBookLine != null)
                        {
                            cashBookLine.IsDeposited = true;
                            cashBookLine.ChangeSetOp = ChangeSetOperation.Update;
                        }
                    }

                    LineNumber++;
                    JournalLinePM newCreditJournalLine = new JournalLinePM();
                    newCreditJournalLine.ChangeSetOp = ChangeSetOperation.Insert;
                    newCreditJournalLine.Tenant = newJournal.Tenant;
                    newCreditJournalLine.Line = LineNumber;
                    newCreditJournalLine.ActionCode = "1"; //Credit
                    newCreditJournalLine.DueDate = cheque.ValueDate;
                    newCreditJournalLine.LocalAmount = item.LocalAmount;
                    newCreditJournalLine.ForeignAmount = item.ForeignAmount;
                    newCreditJournalLine.CurrencyId = entityPM.DepositCurrencyId;
                    newCreditJournalLine.DocumentDate = entityPM.AccountingDate;
                    newCreditJournalLine.AccountingDate = entityPM.AccountingDate;
                    newCreditJournalLine.ExchangeRate = cheque.LocalAmount / cheque.ForeignAmount;

                    newCreditJournalLine.CreditAccountId = cashBook.AccountId;
                    gLAccount = gLAccountQueryService.GetSingle(cashBook.AccountId, false, true);
                    if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
                    {
                        newCreditJournalLine.CreditControlAccountId = gLAccount.ControlAccountId;
                    }

                    newCreditJournalLine.Reference1 = cheque.ChequeNumber;
                    newCreditJournalLine.Reference2 = entityPM.DepositNumber.ToString();


                    newJournal.JournalLines.Add(newCreditJournalLine);

                    if (cheque != null)
                    {
                        cheque.StatusCode = (cheque.ValueDate > todayDateTime ? "2" : "3");  // 2-In Bank , 3-In Bank Account
                        cheque.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        var myChequeUpdateService = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        myChequeUpdateService.Update(cheque, true);
                    }
                }
            }
        }
        void CreateDebitJournalLines(BankDepositPM entityPM, int LineNumber, CashBookPM cashBook, JournalPM newJournal)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            ARPaymentChequeQueryService arpChequeQueryService = new ARPaymentChequeQueryService(entityPM.Tenant);
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPM.Tenant);
            GLAccountPM gLAccount;
            BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(entityPM.Tenant);
            BankAccountPM bankAccount = bankAccountQueryService.GetSingle(entityPM.DepositBankAccountId, true, false);


            if (entityPM.IsCashDeposit)  //Cash Deposit
            {

                LineNumber++;
                JournalLinePM newDebitJournalLine = new JournalLinePM();
                newDebitJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                newDebitJournalLine.Tenant = newJournal.Tenant;
                newDebitJournalLine.Line = LineNumber;
                newDebitJournalLine.ActionCode = "2"; //Debit
                newDebitJournalLine.DocumentDate = entityPM.CreateDate;
                newDebitJournalLine.AccountingDate = entityPM.AccountingDate;

                newDebitJournalLine.DebitAccountId = bankAccount.GLAccountId;
                gLAccount = gLAccountQueryService.GetSingle(bankAccount.GLAccountId, false, true); ///??????
                if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
                {
                    newDebitJournalLine.DebitControlAccountId = gLAccount.ControlAccountId;
                }

                newDebitJournalLine.DueDate = entityPM.CreateDate;
                newDebitJournalLine.LocalAmount = entityPM.LocalDepositAmount;
                newDebitJournalLine.ForeignAmount = entityPM.ForeignAmount;
                newDebitJournalLine.CurrencyId = entityPM.DepositCurrencyId;
                newDebitJournalLine.ExchangeRate = entityPM.LocalDepositAmount / entityPM.ForeignAmount;
                newDebitJournalLine.Reference1 = entityPM.DepositNumber.ToString();

                newJournal.JournalLines.Add(newDebitJournalLine);

            }
            else  //Deffe Deposit
            {
                foreach (BankDepositLinePM item in entityPM.BankDepositLines)
                {
                    ARPaymentChequePM cheque = arpChequeQueryService.GetSingle(item.ARPaymentChequeId, false, false);

                    LineNumber++;
                    JournalLinePM newDebitJournalLine = new JournalLinePM();
                    newDebitJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                    newDebitJournalLine.Tenant = newJournal.Tenant;
                    newDebitJournalLine.Line = LineNumber;
                    newDebitJournalLine.ActionCode = "2"; //Debit
                    newDebitJournalLine.DueDate = cheque.ValueDate;
                    newDebitJournalLine.LocalAmount = item.LocalAmount;
                    newDebitJournalLine.ForeignAmount = item.ForeignAmount;
                    newDebitJournalLine.CurrencyId = entityPM.DepositCurrencyId;
                    newDebitJournalLine.DocumentDate = entityPM.AccountingDate;
                    newDebitJournalLine.AccountingDate = entityPM.AccountingDate;
                    newDebitJournalLine.Reference1 = cheque.ChequeNumber;
                    newDebitJournalLine.Reference2 = entityPM.DepositNumber.ToString();
                    newDebitJournalLine.ExchangeRate = cheque.LocalAmount / cheque.ForeignAmount;

                    if (cheque.ValueDate <= todayDateTime)
                    {
                        newDebitJournalLine.DebitAccountId = bankAccount.GLAccountId;
                    }
                    else
                    {
                        newDebitJournalLine.DebitAccountId = bankAccount.DeferredGLAccountId;
                    }

                    gLAccount = gLAccountQueryService.GetSingle(newDebitJournalLine.DebitAccountId, false, true);
                    if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
                    {
                        newDebitJournalLine.DebitControlAccountId = gLAccount.ControlAccountId;
                    }

                    newJournal.JournalLines.Add(newDebitJournalLine);

                    if (cheque != null)
                    {
                        cheque.StatusCode = (cheque.ValueDate > todayDateTime ? "2" : "3");  // 2-In Bank , 3-In Bank Account
                        cheque.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        var myChequeUpdateService = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        myChequeUpdateService.Update(cheque, true);
                    }
                }
            }
        }
        #endregion

        #region Others functions

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }


        public virtual Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }


        public virtual ContactPM GetLoggedContact(int tenant)
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


        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "BankDeposit", Tenant);
        }

        public virtual int CodeCounterWrapperGetNumber(int Tenant)
        {
            return (new CodeCounterWrapper(false)).GetNumber(
                    "BankDeposit", Tenant);
        }

        public virtual void LogActivity(BankDepositPM entityPM)
        {
            //Activity Log
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
            var myLoggedUser = GetLoggedContact(entityPM.Tenant);
            if (myLoggedUser != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "N", myLoggedUser.Id);
            }
        }

        #endregion

    }


    public interface IBankDepositOnCreatingService {
        void OnCreating(BankDepositPM entityPM);
        DateTime GetCurrentDateTime(int tenant);
        ContactPM GetLoggedContact(int tenant);
        string IdCounterWrapperGetNumber(int Tenant);
        int CodeCounterWrapperGetNumber(int Tenant);
        void LogActivity(BankDepositPM entityPM);
    }
}
