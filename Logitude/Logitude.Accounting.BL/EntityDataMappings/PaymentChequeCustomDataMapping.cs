using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityDataMappings
{
    public class PaymentChequeCustomDataMapping : IPaymentChequeCustomDataMapping
    {

        public void PMToPOCO(PaymentChequePM entityPM, PaymentCheque entityPOCO, List<PaymentChequeDataMapping.POCOPropertyNames> customMappedPOCOProperties)
        {
            customMappedPOCOProperties.Add(PaymentChequeDataMapping.POCOPropertyNames.Id);
            customMappedPOCOProperties.Add(PaymentChequeDataMapping.POCOPropertyNames.Tenant);
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }



           customMappedPOCOProperties.Add(PaymentChequeDataMapping.POCOPropertyNames.SearchFields);
            
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void POCOToPM(PaymentChequePM entityPM, PaymentCheque entityPOCO, List<PaymentChequeDataMapping.PMPropertyNames> customMappedPMProperties)
        {
            customMappedPMProperties.Add(PaymentChequeDataMapping.PMPropertyNames.BankAccountGLAccountId);
            customMappedPMProperties.Add(PaymentChequeDataMapping.PMPropertyNames.PayToGLAccountId);
            customMappedPMProperties.Add(PaymentChequeDataMapping.PMPropertyNames.PaymentChequeStatusCode);

            //JournalPM journal = GetSingleJournalPM(entityPOCO.Id, entityPOCO.Tenant);

            //if (journal != null)
            //{
            //    entityPM.JournalId = journal.Id;
            //    entityPM.JournalNumber = journal.JournalNumber;
            //}


            if (entityPOCO.PayToGLAccountId != null)
            {
             
                GLAccountPM account = GetSingleGLAccountPM(entityPOCO.PayToGLAccountId, entityPOCO.Tenant);
                if (account != null)
                {
                    entityPM.GLAccountCurrencyId = account.CurrencyId;
                    if (account.IsMultiCurrency == null) { account.IsMultiCurrency = false; }
                    entityPM.IsGLAccountMultiCurrency = (bool)account.IsMultiCurrency;
                    entityPM.GLAccountName = account.LocalName;
                }

            }

            if (entityPOCO.BankAccountGLAccountId != null)
            {

                GLAccountPM account = GetSingleGLAccountPM(entityPOCO.BankAccountGLAccountId, entityPOCO.Tenant);
                if (account != null)
                {
                    entityPM.BankGLAccountCurrencyId = account.CurrencyId;
                    entityPM.IsBankGlAccountMultiCur = (bool)account.IsMultiCurrency;

                }

            }
            if (entityPOCO.BankAccountId != null)
            {

                BankAccountPM bankAccount = GetSingleBankAccountPM(entityPOCO.BankAccountId, entityPOCO.Tenant, false);
                if (bankAccount != null)
                {
                    entityPM.BankLocalName = bankAccount.LocalName;
                    entityPM.BankEnglishName = bankAccount.EnglishName;

                }

            }
            if (entityPOCO.PaymentChequeStatusCode != null)
            {

                PaymentChequeStatusPM PaymentChequeStatus = GetSinglePaymentChequeStatusPM(entityPOCO.PaymentChequeStatusCode, entityPOCO.Tenant, false);
                if (PaymentChequeStatus != null)
                {
                    ContactPM loggedContact = GetLogContact(entityPM.Tenant);

                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.PaymentChequeStatusName = PaymentChequeStatus.EnglishName;
                    }
                    else { entityPM.PaymentChequeStatusName = PaymentChequeStatus.LocalName; }

                }

            }
            if (entityPOCO.CurrencyId != null)
            {

                CurrencyPM currency = GetSingleCurrencyPM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyCode = currency.Code;

                }

            }

        }

        public virtual GLAccountPM GetSingleGLAccountPM(string gLAccountId, int tenant)
        {
            GLAccountQueryService glaService = new GLAccountQueryService(tenant);
            GLAccountPM account = glaService.GetSinglePM(gLAccountId, tenant);
            return account;

        }

        public virtual BankAccountPM GetSingleBankAccountPM(string bankId, int tenant, bool getFromCache)
        {
            BankAccountQueryService bankService = new BankAccountQueryService(tenant);
            BankAccountPM bankAccount = bankService.GetSingle(bankId, false, getFromCache);
            return bankAccount;

        }

        public virtual JournalPM GetSingleJournalPM(string entityId, int tenant)
        {
            JournalQueryService JournalService = new JournalQueryService(tenant);
            JournalPM Journal = JournalService.GetByAccountingEntityId(entityId, tenant);
            return Journal;

        }

       public virtual  PaymentChequeStatusPM GetSinglePaymentChequeStatusPM(string statusCode, int tenant, bool getFromCache)
        {

            PaymentChequeStatusQueryService  paymentChequeStatusService = new PaymentChequeStatusQueryService(tenant);
            PaymentChequeStatusPM status = paymentChequeStatusService.GetSingle(statusCode, false, getFromCache);
            return status;
        }

        public virtual CurrencyPM GetSingleCurrencyPM(string currencyId, int tenant)
        {

            CurrencyQuery currencyService = new CurrencyQuery(tenant);
            CurrencyPM currency = currencyService.GetSinglePM(currencyId, tenant);
            return currency;
        }


        public virtual ContactPM GetLogContact(int tenant)
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
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
            ContactPM contact = contactQuery.GetSinglePM(email, tenant);
           

            return contact;
        }

    }




    public interface IPaymentChequeCustomDataMapping
    {

        void PMToPOCO(PaymentChequePM entityPM, PaymentCheque entityPOCO, List<PaymentChequeDataMapping.POCOPropertyNames> customMappedPOCOProperties);
        void POCOToPM(PaymentChequePM entityPM, PaymentCheque entityPOCO, List<PaymentChequeDataMapping.PMPropertyNames> customMappedPMProperties);
        GLAccountPM GetSingleGLAccountPM(string gLAccountId, int tenant);
        BankAccountPM GetSingleBankAccountPM(string bankId, int tenant, bool getFromCache);
        JournalPM GetSingleJournalPM(string journalId, int tenant);
        PaymentChequeStatusPM GetSinglePaymentChequeStatusPM(string statusId, int tenant, bool getFromCache);
        CurrencyPM GetSingleCurrencyPM(string currencyId, int tenant);
        ContactPM GetLogContact(int tenant);
    }


}
