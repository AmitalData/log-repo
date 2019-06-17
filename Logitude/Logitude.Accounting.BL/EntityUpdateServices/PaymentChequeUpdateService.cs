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
        protected override void OnUpdating(PaymentChequePM entityPM)
        {

            if (entityPM.PaymentChequeStatusCode == "2")
            {
                BankAccountQueryService bankAccountService = new BankAccountQueryService(entityPM.Tenant);
                BankAccountPM bankAccount = bankAccountService.GetSingle(entityPM.BankAccountId, false, false);
                if (bankAccount.ChequeCounter == null)
                {
                    throw new Exception("The cheque counter did not defined for the choosen bank");
                }
                else
                {
                    entityPM.ChequeNumber = bankAccount.ChequeCounter.ToString();
                    entityPM.UniqueField = entityPM.ChequeNumber;
                    BankAccountUpdateService bankAccountUpdateService = new BankAccountUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                    bankAccount.ChequeCounter += 1;
                    bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                    bankAccountUpdateService.Update(bankAccount, true);
                }
              
                TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
                TenantPM currentTenant = tenantQuery.GetSinglePM(entityPM.Tenant);
                entityPM.ForeignAmount = entityPM.LocalAmount;
                JournalUpdateService journalUpdateService = new JournalUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
                JournalPM journal = new JournalPM
                {
                    CreateDate = entityPM.CreateDate,
                    TypeCode="0",
                    StatusCode="2",
                    CreatedByUserId = entityPM.CreatedByUserId,
                    AccountingEntityCode = "9",
                    AccountingEntityId = entityPM.Id,
                    AccountingEntityReference = entityPM.ChequeNumber,
                    UpdateDate= entityPM.UpdateDate,
                    UpdatedByUserId= entityPM.UpdatedByUserId,
                    ApproveDate = entityPM.CreateDate,
                    ApprovedByUserId = entityPM.CreatedByUserId,
                    ChangeSetOp= ChangeSetOperation.Insert,
                    Tenant =entityPM.Tenant,
                    AccountingDate = (DateTime)entityPM.CreateDate,

                };
                if (entityPM.ForeignAmount == null) entityPM.ForeignAmount = 0;
                string creditAccount = null;
                if (bankAccount.TransferGLAcccountId != null)
                {
                    creditAccount = bankAccount.TransferGLAcccountId;
                }
                else
                {
                    creditAccount = entityPM.BankAccountGLAccountId;
                }
               
                JournalLinePM journalLine1 = new JournalLinePM()
                {
                    ActionCode = "1",
                   CreditAccountId = creditAccount,
                   DocumentDate = entityPM.CreateDate,
                    AccountingDate = (DateTime)entityPM.CreateDate,

                    DueDate = (DateTime)entityPM.ValueDate,
                   LocalAmount=(decimal) entityPM.LocalAmount,
                   CurrencyId = entityPM.CurrencyId,
                   ForeignAmount = (decimal)entityPM.ForeignAmount,
                   ExchangeRate = entityPM.ExchangeRate,
                    ChangeSetOp = ChangeSetOperation.Insert


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
                    ChangeSetOp = ChangeSetOperation.Insert
                   

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
                journalUpdateService.Update(journal, true);
                entityPM.JournalNumber = journal.JournalNumber;
            }
            ValidateEntity(entityPM);
            entityPM.UpdateDate = DateTime.Now;
            entityPM.UpdatedByUserId = AuthenticationUtil.ResolveUserId(entityPM.Tenant);
        }

        protected override void Trace(PaymentChequePM entityPM, PaymentCheque entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

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
            ValidateGLAccountAccountType(entityPM);
          
          
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
