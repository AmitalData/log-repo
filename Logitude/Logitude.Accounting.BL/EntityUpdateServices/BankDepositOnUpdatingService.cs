using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.Server.Tools;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;
using Logitude.Accounting.BL.CloseTables;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
    public class BankDepositOnUpdatingService : IBankDepositOnUpdatingService
    {
        private IAccountingContext _MainContext;
        public BankDepositOnUpdatingService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        // Main Method
        public void OnUpdating(BankDepositPM entityPM)
        {
            // dates
            entityPM.UpdateDate = GetCurrentDateTime(entityPM.Tenant);

            // users:
            ContactPM user = GetLoggedContact(entityPM.Tenant);
            if (user != null)
            {
                entityPM.UpdatedByUserId = user.Id;
            }

            // Activity log
            LogActivity(entityPM);

            //Main Logic
            if (entityPM.ChangeSetOp == ChangeSetOperation.Update) // [!] the Update in Bank deposit only happens when cancle deposit and out of deposit - Abdullah
            {

                BankDepositRepository repo = new BankDepositRepository(entityPM.Tenant);
                BankDeposit entityPOCO = repo.GetSingle(entityPM.Id, entityPM.Tenant);

                //cancel deposit
                if (entityPM.IsCanceled != entityPOCO.IsCanceled)
                {
                    if (entityPM.IsCanceled == true)
                        entityPM = CancelDeposit(entityPM, entityPM.Tenant);
                }

                //out of deposit
                else if (entityPM.BankDepositLines.Any(d => d.IsOutOfDeposit == true))
                {
                    // case: return cheque
                    // do nothing :)
                }
                else
                {
                    throw new ApplicationException("Only cancel deposit or out of deposit is enabled for bank deposit update!!");
                }

            }

        }


        #region Cancel Deposit Logic

        public BankDepositPM CancelDeposit(BankDepositPM bankDeposit, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                //1- void the journal   
                VoidJournal(bankDeposit, tenant);
                
                //2- Update cashbook     
                CashBookPM cashbookPM = UpdateCashbook(bankDeposit, tenant);

                //3- Update BankDeposit
                bankDeposit = UpdateBankDeposit(bankDeposit, cashbookPM, tenant);

                scope.Complete();
            }
            return bankDeposit;

        }

        private BankDepositPM UpdateBankDeposit(BankDepositPM bankDeposit, CashBookPM cashbookPM, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            // users:
            bool showLocals = false;
            ContactPM user = GetLoggedContact(tenant);
            if (user != null)
            {
                showLocals = !user.DontShowLocal;
            }


            //3- Update BankDeposit.IsCancelled = true
            bankDeposit.IsCanceled = true;

            //
            //4- In case cheque deposit, update:
            //   *- For each cheque: Update StatusCode.ARPaymentCheques=1- In Cashbook
            //   *- Update IsDeposited.bankDepositLines=False
            if (!bankDeposit.IsCashDeposit)
            {
                foreach (var depositLine in bankDeposit.BankDepositLines)
                {
                    //(a) update cheques
                    ARPaymentChequeUpdateService chequeService = new ARPaymentChequeUpdateService(MyContext, new Dictionary<string, IContext>(), cashbookPM.Tenant);
                    ARPaymentChequeQueryService chequesQuery = new ARPaymentChequeQueryService(MyContext);
                    ARPaymentChequeRepository chequesRepo = new ARPaymentChequeRepository(MyContext);
                    ARPaymentCheque cheque = chequesRepo.GetSingle(depositLine.ARPaymentChequeId, tenant);
                    ARPaymentChequePM chequePM = chequesQuery.GetEntityPM(cheque);

                    if (chequePM != null)
                    {
                        //Task 44665: Cancel Deposits : check Cheque Status before canceling the deposit
                        if (chequePM.StatusCode == "6") // 6- Redeemed
                        {
                            throw new ApplicationException(TextCodesTranslator.TranslateText("BankDeposit.O.DepositCancelChequeMSG", tenant, showLocals));
                        }
                        else
                        {
                            chequePM.StatusCode = "1"; // 1- In Cashbook
                        }
                    }
                    else
                    {
                        throw new ApplicationException("Cannot find connected cheque for deposit line: " + depositLine.Line);
                    }

                    // update
                    chequeService.InitializeEntityPM(chequePM);
                    chequePM.ChangeSetOp = ChangeSetOperation.Update;
                    chequeService.Update(chequePM, true);


                    //(b) update lines
                    //depositLine.IsOutOfDeposit = true;
                }
            }

            return bankDeposit;

        }

        private CashBookPM UpdateCashbook(BankDepositPM bankDeposit, int tenant)
        {
            //-Update TotalAmount.Cashbooks = Cashbook.TotalAmount + BankDeposit.ForeignAmount

            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            CashBookRepository cashbookRepo = new CashBookRepository(MyContext);
            CashBookQueryService cashbookQuery = new CashBookQueryService(MyContext);

            //Get the connected cashbook
            CashBook cashbook = cashbookRepo.GetSingle(bankDeposit.CashBookId, tenant);
            CashBookPM cashbookPM = cashbookQuery.GetSingle(bankDeposit.CashBookId, true, false);

            //update cashbook
            if (cashbookPM != null)
            {
                cashbookPM.TotalAmount += Math.Round(bankDeposit.ForeignAmount, 2);

                // update cashbook rows if cashbook is cheques
                if (!bankDeposit.IsCashDeposit)
                {
                    bankDeposit.BankDepositLines.ForEach((depositLine) =>
                    {
                        CashBookLinePM cashbookLine = cashbookPM.CashBookLines.Find(d => d.ARPChequeId == depositLine.ARPaymentChequeId);
                        cashbookLine.ChangeSetOp = ChangeSetOperation.Update;
                        cashbookLine.IsDeposited = false;

                    });

                }

                //cashbookRepo.Update(cashbook);
                //cashbookRepo.SubmitChanges();

                // Save CashbookPM with its lines
                CashBookUpdateService service = new CashBookUpdateService(MyContext, new Dictionary<string, IContext>(), cashbookPM.Tenant);
                service.InitializeEntityPM(cashbookPM);
                cashbookPM.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(cashbookPM, true);
            }
            else
            {
                throw new ApplicationException("Couldn't find the connected cashbook!!!!!!");
            }

            return cashbookPM;
        }

        private void VoidJournal(BankDepositPM bankDeposit, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            JournalPM journalPM;
            JournalQueryService journalQuery = new JournalQueryService(MyContext);
            IQueryable<JournalPM> journalPMs = journalQuery.GetJournalsByAccountingEntityId(bankDeposit.Id, tenant);
            if (journalPMs != null)
            {
                var list = journalPMs.Where(r => r.AccountingEntityCode == AccountingEntityValues.CashDeposit || r.AccountingEntityCode == AccountingEntityValues.ChequeDeposit).ToList();//why itzik need to fix that ????  6   הפקדת המחאות    Cheque Deposit
                if (list.Count > 1)
                    throw new ApplicationException("Find more then 1 Journal for this deposit");
                journalPM = list.FirstOrDefault();
            }
            else
            {
                throw new ApplicationException("Couldn't find any Journal for this deposit");
            }

            //  (b) void the journal
            if (journalPM != null)
            {
                var service = new JournalVoidUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);

                var StornoOverrideM = new StornoOverrideM()
                {
                    AccountingEntityCode = journalPM.AccountingEntityCode,
                    AccountingEntityId = journalPM.AccountingEntityId,
                    AccountingEntityReference = journalPM.AccountingEntityReference,
                };
                journalPM = service.VoidJournal(journalPM.Id, tenant, StornoOverrideM);
                AddAccountingEntityJournal(journalPM, AccountingEntityJournalActions.BankDepositCancel);
            }
            else
            {
                throw new ApplicationException("Couldn't find the Journal for this deposit");
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


        public virtual void LogActivity(BankDepositPM entityPM)
        {
            //Activity Log
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
            var myLoggedUser = GetLoggedContact(entityPM.Tenant);
            if (myLoggedUser != null)
            {
                ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", myLoggedUser.Id);
            }
        }


        #endregion

        public void AddAccountingEntityJournal(JournalPM journal,string actionName, string childEntityId = null)
        {
            IAccountingContext context = AccountingContext.GetContext(journal.Tenant);
            AccountingEntityJournalUpdateService service = new AccountingEntityJournalUpdateService(context, new Dictionary<string, IContext>(), journal.Tenant);
            service.AddAccountingEntitieJournal(journal, actionName, childEntityId);
        }
    }


    public interface IBankDepositOnUpdatingService
    {
        void OnUpdating(BankDepositPM entityPM);
        DateTime GetCurrentDateTime(int tenant);
        ContactPM GetLoggedContact(int tenant);
        void LogActivity(BankDepositPM entityPM);
    }
}
