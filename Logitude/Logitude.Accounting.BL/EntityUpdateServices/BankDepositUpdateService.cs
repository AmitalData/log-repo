using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Simplog.Data.CommonDataModel;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Accounting.Data.EntityLists;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Accounting.BL.CoreBL;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityUpdateServicesExt;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityUpdateServices
{

    public partial class BankDepositUpdateService : EntityUpdateService<BankDeposit, BankDepositPM, EntityPM>
    {
        protected override void OnCreating(BankDepositPM entityPM, EntityPM entityParentPM)
        {

            BankDepositOnCreatingService bankDepositOnCreatingService = new BankDepositOnCreatingService(currentContext, entityPM.Tenant);
            bankDepositOnCreatingService.OnCreating(entityPM);



        }

        protected override void OnUpdating(BankDepositPM entityPM)
        {

            BankDepositOnUpdatingService bankDepositOnCreatingService = new BankDepositOnUpdatingService(currentContext);
            bankDepositOnCreatingService.OnUpdating(entityPM);

            #region old code

            /***********************************************************/

            // USERS
            //ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
            //ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("BankDeposit", 0, true);
            //DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            //entityPM.UpdateDate = todayDateTime;
            //ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            //ContactRepository contactRep = new ContactRepository(commonContext);
            //string email = "";
            //if (AuthenticationUtil.IsAuthenticatedUserExists())
            //{
            //    email = AuthenticationUtil.GetAuthenticatedUser();
            //}
            //else
            //{
            //    email = "system@tenant" + entityPM.Tenant + ".com";
            //}
            //string myLoggedUserId = null;
            //bool showLocal = false;

            //Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            //if (contact != null)
            //{
            //    myLoggedUserId = contact.Id;
            //    showLocal = !contact.DontShowLocalLabels;
            //}
            //entityPM.UpdatedByUserId = myLoggedUserId;
            ////



            //CashBookQueryService cashBookQueryService = new CashBookQueryService(entityPM.Tenant);
            //CashBookPM cashBook = cashBookQueryService.GetSingle(entityPM.CashBookId, true, false);

            //if (entityPM.ChangeSetOp == ChangeSetOperation.Insert) //New Deposit
            //{
            //    //code moved into on create
            //}
            //else if (entityPM.ChangeSetOp == ChangeSetOperation.Update) //Update Deposit - Creating Journal for Deleted Deposit Lines
            //{

            //    ////cancel deposit
            //    //BankDepositRepository repo = new BankDepositRepository(entityPM.Tenant);
            //    //BankDeposit entityPOCO = repo.GetSingle(entityPM.Id, entityPM.Tenant);
            //    //if (entityPM.IsCanceled != entityPOCO.IsCanceled)
            //    //{
            //    //    if(entityPM.IsCanceled == true)
            //    //        entityPM = CancelDeposit(entityPM, entityPM.Tenant);
            //    //}
            //    //else if (entityPM.BankDepositLines.Any(d=>d.IsOutOfDeposit == true))
            //    //{
            //    //    // case: return cheque
            //    //    // do nothing 
            //    //}
            //    //else
            //    {

            //        /*
            //         *  [!]
            //         *  [!]     This Code should not be written!!!  --- deleted
            //         *  [!]
            //         *  
            //         * */

            //        //if (entityPM.ForeignAmount > cashBook.TotalAmount)
            //        //{
            //        //    throw new ApplicationException(TextCodesTranslator.TranslateText("BankDeposit.O.DepositAmountmustbelessthanCashbook", 0, showLocal));
            //        //}

            //        //GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPM.Tenant);
            //        //GLAccountPM gLAccount;
            //        //JournalPM newJournal = new JournalPM();
            //        //newJournal.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            //        //newJournal.Tenant = entityPM.Tenant;
            //        //newJournal.CreateDate = DateTime.Now;
            //        //newJournal.AccountingDate = entityPM.AccountingDate;
            //        //newJournal.TypeCode = "0"; //Manual
            //        //newJournal.StatusCode = "2"; // Approved
            //        //newJournal.CreatedByUserId = entityPM.CreatedByUserId;
            //        //newJournal.AccountingEntityCode = "4"; //Deposit
            //        //newJournal.AccountingEntityId = entityPM.DepositNumber.ToString();
            //        //newJournal.ExternalNo = null;
            //        //newJournal.UpdateDate = DateTime.Now;
            //        //newJournal.UpdatedByUserId = entityPM.UpdatedByUserId;
            //        //newJournal.ApproveDate = entityPM.CreateDate;
            //        //newJournal.ApprovedByUserId = entityPM.CreatedByUserId;

            //        //int LineNumber = 0;


            //        ////Creating JournalLines for Bank Crediting 

            //        //if (entityPM.IsCashDeposit)  //Cash Deposit Bank Crediting in one line
            //        //{
            //        //    LineNumber++;
            //        //    JournalLinePM newDebitJournalLine = new JournalLinePM();
            //        //    newDebitJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            //        //    newDebitJournalLine.Tenant = newJournal.Tenant;
            //        //    newDebitJournalLine.Line = LineNumber;
            //        //    newDebitJournalLine.ActionCode = "1"; //Credit
            //        //    newDebitJournalLine.DocumentDate = entityPM.CreateDate;
            //        //    newDebitJournalLine.AccountingDate = entityPM.AccountingDate;
            //        //    newDebitJournalLine.CreditAccountId = entityPM.CashGLAccountId;

            //        //    gLAccount = gLAccountQueryService.GetSingle(entityPM.CashGLAccountId, false, true);
            //        //    if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
            //        //    {
            //        //        newDebitJournalLine.CreditControlAccountId = gLAccount.ControlAccountId;
            //        //    }
            //        //    decimal localDepositAmount = 0;
            //        //    decimal foreignDepositAmount = 0;
            //        //    newDebitJournalLine.DueDate = entityPM.CreateDate;
            //        //    newDebitJournalLine.CurrencyId = entityPM.DepositCurrencyId;

            //        //    foreach (BankDepositLinePM item in entityPM.BankDepositLines)
            //        //    {
            //        //        if (item.ChangeSetOp == ChangeSetOperation.Update)
            //        //        {
            //        //            localDepositAmount += item.LocalAmount;
            //        //            foreignDepositAmount += item.ForeignAmount;
            //        //        }
            //        //    }
            //        //    newDebitJournalLine.LocalAmount = localDepositAmount;
            //        //    newDebitJournalLine.ForeignAmount = foreignDepositAmount;
            //        //}
            //        //else  //Deffe Deposit - Creating Journal Lines For Bank Crediting 
            //        //{
            //        //    foreach (BankDepositLinePM item in entityPM.BankDepositLines)
            //        //    {
            //        //        if (item.ChangeSetOp == ChangeSetOperation.Update)
            //        //        {
            //        //            LineNumber++;
            //        //            JournalLinePM newDebitJournalLine = new JournalLinePM();
            //        //            newDebitJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            //        //            newDebitJournalLine.Tenant = newJournal.Tenant;
            //        //            newDebitJournalLine.Line = LineNumber;
            //        //            newDebitJournalLine.ActionCode = "2"; // Debit
            //        //            newDebitJournalLine.DueDate = item.DueDate;
            //        //            newDebitJournalLine.LocalAmount = item.LocalAmount;
            //        //            newDebitJournalLine.ForeignAmount = item.ForeignAmount;
            //        //            newDebitJournalLine.CurrencyId = entityPM.DepositCurrencyId;
            //        //            newDebitJournalLine.DocumentDate = entityPM.CreateDate;
            //        //            newDebitJournalLine.AccountingDate = entityPM.AccountingDate;
            //        //            newDebitJournalLine.CreditAccountId = entityPM.DeferredGLAccountId;

            //        //            gLAccount = gLAccountQueryService.GetSingle(entityPM.DeferredGLAccountId, false, true);
            //        //            if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
            //        //            {
            //        //                newDebitJournalLine.CreditControlAccountId = gLAccount.ControlAccountId;
            //        //            }

            //        //            newJournal.JournalLines.Add(newDebitJournalLine);
            //        //        }
            //        //    }
            //        //}


            //        ////CashBookQueryService cashBookQueryService = new CashBookQueryService(entityPM.Tenant);
            //        //cashBook = cashBookQueryService.GetSingle(entityPM.CashBookId, true, false);
            //        //if (cashBook != null)
            //        //{
            //        //    cashBook.ChangeSetOp = ChangeSetOperation.Update;
            //        //}

            //        ////Creating JournalLines For Cashbook Debiting
            //        //foreach (BankDepositLinePM item in entityPM.BankDepositLines)
            //        //{
            //        //    if (item.ChangeSetOp == ChangeSetOperation.Update)
            //        //    {
            //        //        LineNumber++;
            //        //        JournalLinePM newCreditJournalLine = new JournalLinePM();
            //        //        newCreditJournalLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            //        //        newCreditJournalLine.Tenant = newJournal.Tenant;
            //        //        newCreditJournalLine.Line = LineNumber;
            //        //        newCreditJournalLine.ActionCode = "2"; //Debit
            //        //        newCreditJournalLine.DueDate = item.DueDate;
            //        //        newCreditJournalLine.LocalAmount = item.LocalAmount;
            //        //        newCreditJournalLine.ForeignAmount = item.ForeignAmount;
            //        //        newCreditJournalLine.CurrencyId = entityPM.DepositCurrencyId;
            //        //        newCreditJournalLine.DocumentDate = entityPM.CreateDate;
            //        //        newCreditJournalLine.AccountingDate = entityPM.AccountingDate;
            //        //        newCreditJournalLine.DebitAccountId = entityPM.CashBookGLAccountId;
            //        //        gLAccount = gLAccountQueryService.GetSingle(entityPM.CashBookGLAccountId, false, true);
            //        //        if ((gLAccount != null) && (gLAccount.ControlAccountId != null))
            //        //        {
            //        //            newCreditJournalLine.DebitControlAccountId = gLAccount.ControlAccountId;
            //        //        }
            //        //        newJournal.JournalLines.Add(newCreditJournalLine);


            //        //        if (cashBook != null)
            //        //        {
            //        //            CashBookLinePM cashBookLine = cashBook.CashBookLines.Where(d => d.ARPChequeId == item.ARPaymentChequeId).FirstOrDefault();
            //        //            if (cashBookLine != null)
            //        //            {
            //        //                cashBookLine.IsDeposited = false;
            //        //                cashBookLine.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            //        //            }
            //        //        }

            //        //    }
            //        //}
            //        //if (cashBook != null)
            //        //{
            //        //    var myCashBookUpdateService = new CashBookUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
            //        //    myCashBookUpdateService.Update(cashBook, true);
            //        //}
            //        //var myJournalUpdateService = new JournalUpdateService(MainContext, AdditionalContexts, entityPM.Tenant);
            //        //myJournalUpdateService.Update(newJournal, true);
            //    }
            //}



            //// Log Activity
            //if (myLoggedUserId != null)
            //{
            //    ActivityLogger.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", myLoggedUserId);
            //}

            #endregion
        }

        protected override void Trace(BankDepositPM entityPM, BankDeposit entityPOCO, string changesXml)
        {
            ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {


                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    EntityId = entityPM.Id,
                    UserId = loggedContact.Id,
                    ObjectTableName = "BankDeposit",
                });
            }

            if (entityPM.IsCanceled)
            {

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CNEV",
                    EntityId = entityPM.Id,
                    UserId = loggedContact.Id,
                    ObjectTableName = "BankDeposit",
                });
            }

        }



        public ContactPM GetLoggedContact(int tenant)
        {

            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


        protected override void UpdateComposition(BankDepositPM entityPM)
        {
            BankDepositLineUpdateService bankDepositLineUpdateService = new BankDepositLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            bankDepositLineUpdateService.UpdateMulti(entityPM.BankDepositLines, entityPM.DeletedBankDepositLines, entityPM, false);
            base.UpdateComposition(entityPM);
        }

        protected override void AfterUpdating(BankDepositPM entityPM, EntityPM entityParentPM)
        {

        }

        #region other functions
        private void validateAccountingPeriod(BankDepositPM entityPM)
        {
            AccountingPeriodQueryService q = new AccountingPeriodQueryService(entityPM.Tenant);

            List<AccountingPeriodList> periodsList = q.GetAccountingPeriodListByYearAndType(DateTime.Now.Year, "1", entityPM.Tenant); // 1-regular
            AccountingPeriodList period = periodsList.FirstOrDefault();
            bool isValid = IsMonthOpenForAccountingDate(period, entityPM);
            if (!isValid)
            {
                throw new ApplicationException("חודש סגור!"); // closed month
            }

        }

        private bool IsMonthOpenForAccountingDate(AccountingPeriodList currentAccountingPeriodPM, BankDepositPM entityPM)
        {
            bool valid = true;
            if (currentAccountingPeriodPM == null)
            {
                valid = false;
                //errorsList.Add(transText);
            }
            else
            {
                var accountingDateMonth = entityPM.AccountingDate.Date.Month;

                if (accountingDateMonth > currentAccountingPeriodPM.ClosedMonth.GetValueOrDefault())
                {
                    //Valid ... AccountingDateMonth must be greater than close Mounth
                }
                else
                {
                    //Not Valid ... AccountingDateMonth must be greater than close Mounth
                    //not valid  8>=8 
                    //not valid  0>=1 - Must Open mounth before work on year !!
                    valid = false;
                    //errorsList.Add(transText); //ClosedMonth Must B
                }
                if (accountingDateMonth == currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  equal to OpenMonth
                }
                else if (accountingDateMonth < currentAccountingPeriodPM.OpenMonth)
                {
                    //valid ... accountingDateMonth can be  less than OpenMonth
                }
                else
                {
                    valid = false;
                    //errorsList.Add(transText);
                }
            }
            return valid;
        }

        public BankDepositPM CancelDeposit(BankDepositPM bankDeposit, int tenant)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(tenant);

            // GET logged contact, RTL
            ContactPM contact = GetLoggedContact(tenant);
            bool showLocals = !contact.DontShowLocal;

            using (TransactionScope scope = TransactionFactory.GetTransaction())
            {
                //
                //1- void the journal - select * from journals where Journals.AccountingEntityId == BankDeposits.Id

                //  (a) get the journal
                JournalPM journalPM;
                JournalQueryService journalQuery = new JournalQueryService(MyContext);
                IQueryable<JournalPM> journalPMs = journalQuery.GetJournalsByAccountingEntityId(bankDeposit.Id, tenant);
                if (journalPMs != null)
                {
                    journalPM = journalPMs.FirstOrDefault();
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
                }
                else
                {
                    throw new ApplicationException("Couldn't find the Journal for this deposit");
                }


                //
                //2- Update TotalAmount.Cashbooks = Cashbook.TotalAmount + BankDeposit.ForeignAmount
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


                //
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


                scope.Complete();
            }
            return bankDeposit;

        }
        #endregion

    }
}
