
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Security;
using Logitude.BL.Interfaces;
using Microsoft.Practices.Unity;
using Logitude.BL.Helpers;
using Logitude.BL.Resolvers;

namespace Logitude.Accounting.BL.EntityDataMappings
{
   
   public partial class PaymentChequeDataMapping: IMapping<PaymentChequePM, PaymentCheque>
   {

        public void CustomPMToPOCO(PaymentChequePM entityPM, PaymentCheque entityPOCO)
        {


            PaymentChequeCustomDataMapping paymentChequeCustomDataMapping = new PaymentChequeCustomDataMapping();
            paymentChequeCustomDataMapping.PMToPOCO(entityPM, entityPOCO, this.CustomMappedPOCOProperties);

            //    AddPOCOPropertyName(POCOPropertyNames.Id);
            //    AddPOCOPropertyName(POCOPropertyNames.Tenant);
            //    if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            //    {
            //        entityPOCO.Id = entityPM.Id;
            //        entityPOCO.Tenant = entityPM.Tenant;
            //    }



            //    this.CustomMappedPOCOProperties.Add(POCOPropertyNames.SearchFields);
            //    BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert);
            //    entityPOCO.SearchFields = entityPM.SearchFields;

        }
        private static void BuildSearchFields(PaymentChequePM entityPM, PaymentCheque poco, bool isNewEntity)
        {
            string result = "";


            if (!string.IsNullOrEmpty(entityPM.ChequeNumber))
            {
                    result = string.IsNullOrEmpty(result) ? entityPM.ChequeNumber : result + "," + entityPM.ChequeNumber;
               
            }
            if (!string.IsNullOrEmpty(entityPM.GLAccountNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.GLAccountNumber : result + "," + entityPM.GLAccountNumber;

            }
            if (!string.IsNullOrEmpty(entityPM.PayToName))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.PayToName : result + "," + entityPM.PayToName;

            }
            entityPM.SearchFields = result;
            poco.SearchFields = result;

        }

        public void CustomPOCOToPM(PaymentChequePM entityPM, PaymentCheque entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.BankAccountGLAccountId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.PayToGLAccountId);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.PaymentChequeStatusCode);
            JournalPM journal = GetJournalByEntityIdAndEntityCode(entityPM);
            if(journal != null)
            {
                entityPM.JournalId = journal.Id;
                entityPM.JournalNumber = journal.JournalNumber;
            }


            if (entityPOCO.PayToGLAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM account = gLAccountQueryService.GetSingle(entityPOCO.PayToGLAccountId, false, false);
               if(account != null)
                {
                    entityPM.GLAccountCurrencyId = account.CurrencyId;
                    if (account.IsMultiCurrency == null) { account.IsMultiCurrency = false; }
                    entityPM.IsGLAccountMultiCurrency =(bool) account.IsMultiCurrency;
                    entityPM.GLAccountName = account.LocalName;
                }

            }

            if (entityPOCO.BankAccountGLAccountId != null)
            {
                GLAccountQueryService gLAccountQueryService = new GLAccountQueryService(entityPOCO.Tenant);
                GLAccountPM account = gLAccountQueryService.GetSingle(entityPOCO.BankAccountGLAccountId, false, false);
                if (account != null)
                {
                    entityPM.BankGLAccountCurrencyId = account.CurrencyId;
                    entityPM.IsBankGlAccountMultiCur = (bool)account.IsMultiCurrency;

                }

            }
            if (entityPOCO.BankAccountId != null)
            {
                BankAccountQueryService bankAccountQueryService = new BankAccountQueryService(entityPOCO.Tenant);
                BankAccountPM bankAccount = bankAccountQueryService.GetSingle(entityPOCO.BankAccountId, false, false);
                if (bankAccount != null)
                {
                    entityPM.BankLocalName = bankAccount.LocalName;
                    entityPM.BankEnglishName = bankAccount.EnglishName;

                }

            }
            if (entityPOCO.PaymentChequeStatusCode != null)
            {
                PaymentChequeStatusQueryService paymentChequeStatusQueryService = new PaymentChequeStatusQueryService(entityPOCO.Tenant);
                PaymentChequeStatusPM PaymentChequeStatus = paymentChequeStatusQueryService.GetSingle(entityPOCO.PaymentChequeStatusCode, false, false);
                if (PaymentChequeStatus != null)
                {
                    ContactPM loggedContact = GetLoggedContact(entityPM.Tenant);

                    if (loggedContact.DontShowLocal)
                    {

                        entityPM.PaymentChequeStatusName = PaymentChequeStatus.EnglishName;
                    }
                    else { entityPM.PaymentChequeStatusName = PaymentChequeStatus.LocalName; }
                   
                }

            }
            if (entityPOCO.CurrencyId != null)
            {
                CurrencyQuery currencyQuery = new CurrencyQuery(entityPOCO.Tenant);
                CurrencyPM currency = currencyQuery.GetSinglePM(entityPOCO.CurrencyId, entityPOCO.Tenant);
                if (currency != null)
                {
                    entityPM.CurrencyCode = currency.Code;

                }

            }

        }
        private JournalPM GetJournalByEntityIdAndEntityCode(PaymentChequePM paymentCheque) {
            JournalQueryService journalService = new JournalQueryService(paymentCheque.Tenant);
            string accountingEntityCode = paymentCheque.APPaymentId == null ? "9" : "5";
            string accountingEntityId = paymentCheque.APPaymentId != null ? paymentCheque.APPaymentId : paymentCheque.Id;
            return journalService.GetByAccountingEntityIdAndAccountingEntityCode(accountingEntityId, accountingEntityCode, paymentCheque.Tenant);
            
        }
        private ContactPM GetLoggedContact(int tenant)
        {
            //ILoggedContactUtil loggedContactUtil = ContainerAccessor.Container.Resolve(typeof(ILoggedContactUtil), "LoggedContactUtil", new ParameterOverride("", tenant)) as ILoggedContactUtil;
            //ContactPM loggedcontact = loggedContactUtil.GetLoggedContact(tenant);

            ContactPM loggedcontact = LoggedContactResolver.GetLoggedContact(tenant);
            return loggedcontact;
        }


    }


}
   