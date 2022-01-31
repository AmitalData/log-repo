using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.BL.Interfaces;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Microsoft.Practices.Unity;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public partial class PaymentChequeUpdateService
    {
        private const string Cancelled = "4";
        protected override void OnCreating(PaymentChequePM entityPM, EntityPM entityParentPM)
        {

            PaymentChequeOnCreatingService paymentChequeOnCreatingService = new PaymentChequeOnCreatingService(MainContext as IAccountingContext);
            paymentChequeOnCreatingService.OnCreating(entityPM);
            //entityPM.Id = IdCounter.GetNumber("PaymentCheque", entityPM.Tenant);
            //entityPM.CreateDate = DateTime.Now;
            //entityPM.CreatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            //entityPM.UpdateDate = DateTime.Now;
            //entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
            //entityPM.InternalNumber = CodeCounter.GetNumber("PaymentCheque.InternalNumber", entityPM.Tenant).ToString();
            //entityPM.PaymentChequeStatusCode = "1";
            //entityPM.UniqueField = entityPM.Id;
             ValidateEntity(entityPM);
            //TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
            //TenantPM currentTenant = tenantQuery.GetSinglePM(entityPM.Tenant);
            //BankAccountQueryService bankQuery = new BankAccountQueryService(entityPM.Tenant);
            //BankAccountPM bank = bankQuery.GetSingle(entityPM.BankAccountId, false, true);
            //if (currentTenant.CurrencyId == bank.GLAccountCurrencyId)
            //{

            //    entityPM.ExchangeRate = 1;
            //    entityPM.ForeignAmount = entityPM.LocalAmount;
            //}
            //if (!string.IsNullOrEmpty(entityPM.Notes))
            //{
            //    PaymentChequeLinePM paymentChequeLine =  new PaymentChequeLinePM()
            //    { PaymentChequeId = entityPM.Id,
            //        Amount = entityPM.ForeignAmount,
            //        Line = 1,
            //        ChangeSetOp = ChangeSetOperation.Insert,
            //        Tenant = entityPM.Tenant,
            //        Notes = entityPM.Notes,
            //        SequenceNumeric=1
            //    };
            //    entityPM.PaymentChequeLines.Add(paymentChequeLine);
            //}
        }
        protected override void OnUpdating(PaymentChequePM paymentChequePM)
        {

            if (paymentChequePM.PaymentChequeStatusCode == "2")
            {
                if (paymentChequePM.BankAccountId != null)
                {

                    paymentChequePM.ForeignAmount = paymentChequePM.LocalAmount;

                    BankAccountPM bankAccount = UpdateBankAccount(paymentChequePM);
                    APPaymentPM paymentPM = GetAPPayment(paymentChequePM);

                    if (paymentPM == null)
                    {

                        bool exist  = CheckIfPaymentChequeHasAjournal(paymentChequePM);
                        if (!exist)
                        {
                            JournalPM journal = GetNewJournal(paymentChequePM, paymentPM);

                            if (paymentChequePM.ForeignAmount == null) paymentChequePM.ForeignAmount = 0;

                            string creditAccount = FillCreditAccount(paymentChequePM, bankAccount);

                            AddJournalLines(paymentChequePM, paymentPM, journal, creditAccount);

                            JournalUpdateService journalUpdateService = new JournalUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);

                            journalUpdateService.Update(journal, true);
                            AddAccountingEntitieJournal(journal, AccountingEntitieActions.PaymentCheque);
                            paymentChequePM.JournalNumber = journal.JournalNumber;
                        }
                    }
                }
                else
                {
                    throw new ApplicationException("No bank account connected to payment cheque");
                }
            }

            ValidateEntity(paymentChequePM);
            paymentChequePM.UpdateDate = DateTime.Now;
            var contact = LoggedContactResolver.GetLoggedContact(paymentChequePM.Tenant);
            if (contact != null)
            {
                paymentChequePM.UpdatedByUserId = contact.Id;
            }
        }
        private void AddAccountingEntitieJournal(JournalPM entityPM, string action, string ChildEntityId = null)
        {
            IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
            AccountingEntitiesJournalUpdateService service = new AccountingEntitiesJournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            service.AddAccountingEntitieJournal(entityPM, action, ChildEntityId);
        }
        private static bool CheckIfPaymentChequeHasAjournal(PaymentChequePM paymentCheque)
        {
            JournalQueryService journalQueryService = new JournalQueryService(paymentCheque.Tenant);
            JournalPM journal = journalQueryService.GetByAccountingEntityIdAndAccountingEntityCode(paymentCheque.Id, "9", paymentCheque.Tenant);
            if (journal != null)
            {
                return true;
            }
            else return false;

        }

        private static string FillCreditAccount(PaymentChequePM paymentChequePM, BankAccountPM bankAccount)
        {
            string creditAccount;
            if (bankAccount.TransferGLAcccountId != null)
                creditAccount = bankAccount.TransferGLAcccountId;
            else
                creditAccount = paymentChequePM.BankAccountGLAccountId;
            return creditAccount;
        }

        private static void AddJournalLines(PaymentChequePM entityPM, APPaymentPM paymentPM, JournalPM journal, string creditAccount)
        {
            JournalLinePM journalLine1 = new JournalLinePM()
            {
                ActionCode = "1",
                CreditAccountId = creditAccount,
                DocumentDate = entityPM.CreateDate,
                AccountingDate = (DateTime)entityPM.CreateDate,

                DueDate = (DateTime)entityPM.ValueDate,
                LocalAmount = (decimal)entityPM.LocalAmount,
                CurrencyId = entityPM.CurrencyId,
                ForeignAmount = (decimal)entityPM.ForeignAmount,
                ExchangeRate = entityPM.ExchangeRate,
                ChangeSetOp = ChangeSetOperation.Insert,
                Reference1 = entityPM.ChequeNumber,
                Reference2 = null,
                Notes = entityPM.PaymentChequeLines.First().Notes

            };
            JournalLinePM journalLine2 = new JournalLinePM()
            {
                ActionCode = "2",
                DebitAccountId = entityPM.PayToGLAccountId,
                DocumentDate = entityPM.CreateDate,
                AccountingDate = (DateTime)entityPM.CreateDate,
                DueDate = (DateTime)entityPM.ValueDate,
                LocalAmount = (decimal)entityPM.LocalAmount,
                CurrencyId = entityPM.CurrencyId,
                ForeignAmount = (decimal)entityPM.ForeignAmount,
                ExchangeRate = entityPM.ExchangeRate,
                ChangeSetOp = ChangeSetOperation.Insert,
                Reference1 = entityPM.ChequeNumber,
                Reference2 = null,
                Notes = entityPM.PaymentChequeLines.First().Notes


            };

            //if(currentTenant.CurrencyId == entityPM.BankGLAccountCurrencyId)
            //{
            //    journalLine1.ExchangeRate = 1;
            //    journalLine2.ExchangeRate = 1;
            //    journalLine1.ForeignAmount =(decimal) entityPM.LocalAmount;
            //    journalLine2.ForeignAmount = (decimal)entityPM.LocalAmount;

            //}

            journal.JournalLines.Add(journalLine1);
            journal.JournalLines.Add(journalLine2);
        }

        private static JournalPM GetNewJournal(PaymentChequePM entityPM, APPaymentPM paymentPM)
        {
            return new JournalPM
            {
                CreateDate = entityPM.CreateDate,
                TypeCode = "0",
                StatusCode = "2",
                CreatedByUserId = entityPM.CreatedByUserId,
                AccountingEntityCode = paymentPM != null ? "5" : "9",
                AccountingEntityId = entityPM.APPaymentId != null ? entityPM.APPaymentId : entityPM.Id,
                AccountingEntityReference = paymentPM != null ? paymentPM.PaymentNo : entityPM.ChequeNumber,
                UpdateDate = entityPM.UpdateDate,
                UpdatedByUserId = entityPM.UpdatedByUserId,
                ApproveDate = entityPM.CreateDate,
                ApprovedByUserId = entityPM.CreatedByUserId,
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = entityPM.Tenant,
                AccountingDate = (DateTime)entityPM.CreateDate,

            };
        }

        private BankAccountPM UpdateBankAccount(PaymentChequePM entityPM)
        {
            BankAccountQueryService bankAccountService = new BankAccountQueryService(entityPM.Tenant);
            BankAccountPM bankAccount = bankAccountService.GetSingle(entityPM.BankAccountId, false, false);
            if (bankAccount.ChequeCounter == null)
            {
                throw new Exception("The cheque counter did not defined for the choosen bank");
            }
            else
            {
                entityPM.ChequeNumber = entityPM.ChequeNumber == null?  bankAccount.ChequeCounter.ToString() : entityPM.ChequeNumber;
                entityPM.UniqueField = entityPM.ChequeNumber;

                BankAccountUpdateService bankAccountUpdateService = new BankAccountUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                bankAccount.ChequeCounter += 1;
                bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                bankAccountUpdateService.Update(bankAccount, true);
            }

            return bankAccount;
        }

        public  APPaymentPM GetAPPayment(PaymentChequePM paymentCheque)
        {
            if (paymentCheque.APPaymentId != null)
            {
                APPaymentQuery paymentQuery = new APPaymentQuery(paymentCheque.Tenant);
                return paymentQuery.GetSingleAPPaymentPM(paymentCheque.APPaymentId, paymentCheque.Tenant);

            }
            else return null;
        }

        protected override void Trace(PaymentChequePM entityPM, PaymentCheque entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            //Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
            ContactPM contact = LoggedContactResolver.GetLoggedContact(entityPM.Tenant);

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "PaymentCheque",
                    Notes = changesXml
                });

                if(!entityPOCO.IsCancelled && entityPM.IsCancelled)
                {

                    if (entityPM.IsCancelled)
                    {
                        EventTracer.CreateTraceEvent(new EventTracerArgs()
                        {
                            Tenant = entityPM.Tenant,
                            EventTypeCode = "CNEV",
                            UserId = contact.Id,
                            EntityId = entityPM.Id,
                            ObjectTableName = "PaymentCheque",
                            Notes = entityPM.CancellationRemarks,
                            
                        });
                    }

                    }
                }
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "PaymentCheque",
                    Notes = changesXml
                });

                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "OPOP",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "PaymentCheque",
                    Notes = changesXml
                });
            }

           
            

        }
        protected override void UpdateComposition(PaymentChequePM entityPM)
        {
            PaymentChequeLineUpdateService paymentChequeLineUpdateService = new PaymentChequeLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            paymentChequeLineUpdateService.UpdateMulti(entityPM.PaymentChequeLines, entityPM.DeletedPaymentChequeLines, entityPM, false);


            base.UpdateComposition(entityPM);
        }
        internal void ValidateEntity(PaymentChequePM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            PaymentChequeQueryService service = new PaymentChequeQueryService(context);
            bool exist = service.CheckIfPaymentChequeExists(entityPM.Id, entityPM.BankAccountId, entityPM.UniqueField, entityPM.Tenant);
            showLocals= SetShowLocalLabels(entityPM);
            if (exist)
            {
                
               
                throw new Exception(TranslateTextsClass.Translate("Accounting.General.O.PaymentChequeExist", entityPM.Tenant, showLocals));


            }
            if (entityPM.APPaymentId == null)
            {
                ValidateGLAccountAccountType(entityPM);
             
            }
             if( entityPM.IsCancelled || entityPM.PaymentChequeStatusCode == "4")
            {
                ValidateCancellingPaymentCheque(entityPM, EntityPOCO);
            }
          
          
        }
        private void  ValidateCancellingPaymentCheque(PaymentChequePM paymentChequePM , PaymentCheque paymentChequePoco)
        {
            if((paymentChequePM.IsCancelled || paymentChequePM.PaymentChequeStatusCode == Cancelled) && !paymentChequePoco.IsCancelled)
            {
                if (paymentChequePM.APPaymentId != null  && !paymentChequePM.CancelledByAPPayment)
                {
                    PreventCancellingPaymentCheque(paymentChequePM);
                }
            }
        }

        public void PreventCancellingPaymentCheque(PaymentChequePM paymentCheque)
        {
            throw new Exception("You cant cancel an internal payment cheque, you need to cancel APPayment.");
        }
        public static Func<int, ContactPM> OverrideGetLoggedContactFunc { get; set; }
        bool showLocals;
        public bool SetShowLocalLabels(PaymentChequePM entityPM)
        {
            ContactPM contact = GetLoggedContact(entityPM.Tenant) ?? new ContactPM();
             showLocals = !contact.DontShowLocal;
            return showLocals;
        }

        public void ValidateGLAccountAccountType(PaymentChequePM entityPM)
        {
            GLAccountPM account=  GetGLAccountById(entityPM);

            if (account != null)
            {
                if (account.AccountTypeCode == "3")
                {
                    throw new Exception(TranslateTextsClass.Translate("Accounting.General.O.VendorsGLAccount", entityPM.Tenant, showLocals));
                }
            }

        }
        public GLAccountPM GetGLAccountById(PaymentChequePM entityPM)
        {
            GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPM.Tenant);
            return gLAccountQueryService.GetSingle(entityPM.PayToGLAccountId, false, false);
        }
        public static ContactPM GetLoggedContact(int tenant)
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
