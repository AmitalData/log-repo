using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityUpdateServices
{
   public class PaymentChequeOnUpdatingService : IPaymentChequeOnUpdatingService
    {

        private IAccountingContext _MainContext;
        public PaymentChequeOnUpdatingService(IAccountingContext mainContext)
        {
            _MainContext = mainContext;
        }

        public virtual void OnUpdating(PaymentChequePM entityPM, PaymentCheque entityPOCO)
        {

            if (entityPM.PaymentChequeStatusCode == "2")
            {
                //BankAccountQueryService bankAccountService = new BankAccountQueryService(entityPM.Tenant);
                //BankAccountPM bankAccount = bankAccountService.GetSingle(entityPM.BankAccountId, false, false);
                BankAccountPM bankAccount = GetSingleBankAccountPM(entityPM);
                if (bankAccount.ChequeCounter == null)
                {
                    throw new Exception("The cheque counter did not defined for the choosen bank");
                }
                else
                {
                    entityPM.ChequeNumber = bankAccount.ChequeCounter.ToString();
                    entityPM.UniqueField = entityPM.ChequeNumber;
                    BankAccountUpdateService bankAccountUpdateService = new BankAccountUpdateService(_MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    bankAccount.ChequeCounter += 1;
                    bankAccount.ChangeSetOp = ChangeSetOperation.Update;
                    bankAccountUpdateService.Update(bankAccount, true);
                }
               // ValidateEntity(entityPM);
                //TenantQuery tenantQuery = new TenantQuery(entityPM.Tenant);
                //TenantPM currentTenant = tenantQuery.GetSinglePM(entityPM.Tenant);
                entityPM.ForeignAmount = entityPM.LocalAmount;
                JournalPM journal= CreateJournalPM(entityPM);
            }

            entityPM.UpdateDate = DateTime.Now;
            entityPM.UpdatedByUserId = GetLogContactId(entityPM.Tenant);

        }


        public virtual string GetObjectTableId(int tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                 "PaymentCheque", tenant);
        }
    
        public virtual  string GetLogContactId(int tenant)
        {
            ContactRepository contactRep = new ContactRepository(tenant);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + tenant + ".com";
            }
            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            return myLoggedUserId;
        }

        public  virtual  DateTime  GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }

        public virtual JournalPM  CreateJournalPM(PaymentChequePM entityPM)
        {
            APPaymentPM paymentPM = GetAPPayment(entityPM);
            BankAccountPM bankAccount = GetSingleBankAccountPM(entityPM);
            JournalUpdateService journalUpdateService = new JournalUpdateService(_MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            JournalPM journal = new JournalPM
            {
                CreateDate = entityPM.CreateDate,
                TypeCode = "0",
                StatusCode = "2",
                CreatedByUserId = entityPM.CreatedByUserId,
                AccountingEntityCode = "9",
                AccountingEntityId = entityPM.Id,
                AccountingEntityReference = paymentPM != null?  paymentPM.PaymentNo : entityPM.ChequeNumber,
                UpdateDate = entityPM.UpdateDate,
                UpdatedByUserId = entityPM.UpdatedByUserId,
                ApproveDate = entityPM.CreateDate,
                ApprovedByUserId = entityPM.CreatedByUserId,
                ChangeSetOp = ChangeSetOperation.Insert,
                Tenant = entityPM.Tenant,
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
                LocalAmount = (decimal)entityPM.LocalAmount,
                CurrencyId = entityPM.CurrencyId,
                ForeignAmount = (decimal)entityPM.ForeignAmount,
                ExchangeRate = entityPM.ExchangeRate,
                ChangeSetOp = ChangeSetOperation.Insert,
                Reference1 = paymentPM != null? paymentPM.PaymentNo : null,
                Reference2 = entityPM.ChequeNumber,
                

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
                Reference1 = paymentPM != null ? paymentPM.PaymentNo : null,
                Reference2 = entityPM.ChequeNumber,


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

            return journal;
        }

        public virtual BankAccountPM GetSingleBankAccountPM(PaymentChequePM entityPM)
        {

            BankAccountQueryService bankAccountService = new BankAccountQueryService(entityPM.Tenant);
            BankAccountPM bankAccount = bankAccountService.GetSingle(entityPM.BankAccountId, false, false);
            return bankAccount;
        }

        public virtual APPaymentPM GetAPPayment(PaymentChequePM paymentCheque)
        {
            if (paymentCheque.APPaymentId != null)
            {
                APPaymentQuery paymentQuery = new APPaymentQuery(paymentCheque.Tenant);
                return paymentQuery.GetSingleAPPaymentPM(paymentCheque.APPaymentId, paymentCheque.Tenant);

            }
            else return null;
        }
    }

    public interface IPaymentChequeOnUpdatingService
    {

        void OnUpdating(PaymentChequePM entityPM, PaymentCheque entityPOCO);
       // string GetObjectTableId(int tenant);
        string GetLogContactId(int tenant);
        DateTime GetCurrentDateTime(int tenant);
        JournalPM  CreateJournalPM(PaymentChequePM entityPM);
        BankAccountPM GetSingleBankAccountPM(PaymentChequePM entityPM);
       APPaymentPM GetAPPayment(PaymentChequePM paymentCheque);
    }
}
