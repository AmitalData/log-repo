using Logitude.Accounting.BL.EntityQueryServices;
using Logitude.Accounting.Data;
using Logitude.Accounting.Def.EntityPMs;

using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools;
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
   public class PaymentChequeOnCreatingService: IPaymentChequeOnCreatingService
    {
        private IAccountingContext _MainContext;
        public PaymentChequeOnCreatingService(IAccountingContext mainContext)
        {
           _MainContext = mainContext;
        }

        public  void OnCreating(PaymentChequePM entityPM)
        {
            
            if (entityPM.Id == null || entityPM.Id == "") entityPM.Id = IdCounterWrapperGetNumber(entityPM.Tenant); //IdCounter.GetNumber("CashBook", entityPM.Tenant);

          //  entityPM.SearchFields = entityPM.EnglishName + "," + entityPM.LocalName + "," + entityPM.AccountNumber + "," + entityPM.AccountName + "," + entityPM.CashBookTypeName;
           

            DateTime todayDateTime = GetCurrentDateTime(entityPM.Tenant);

            entityPM.CreateDate = todayDateTime;
            entityPM.UpdateDate = todayDateTime;


            string myLoggedUserId = GetLogContactId(entityPM.Tenant);
            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.CreatedByUserId == null)
            {
                entityPM.CreatedByUserId = myLoggedUserId;
            }
            
            entityPM.InternalNumber = CodeCounterWrapperGetNumber(entityPM.Tenant).ToString();
            if(entityPM.PaymentChequeStatusCode == null)
            entityPM.PaymentChequeStatusCode = "1";
            entityPM.UniqueField = entityPM.Id;
            //ValidateEntity(entityPM);
            //TenantRepository tenantRepository = new TenantRepository(entityPM.Tenant);
            //TenantQuery tenantQuery = new TenantQuery(tenantRepository);
            //TenantPM currentTenant = tenantQuery.GetSinglePM(entityPM.Tenant);
            //BankAccountQueryService bankQuery = new BankAccountQueryService(entityPM.Tenant);
            //BankAccountPM bank = bankQuery.GetSingle(entityPM.BankAccountId, false, true);
            //if (currentTenant.CurrencyId == bank.GLAccountCurrencyId)
            //{

            //    entityPM.ExchangeRate = 1;
            //    entityPM.ForeignAmount = entityPM.LocalAmount;
            //}
            if (!string.IsNullOrEmpty(entityPM.Notes))
            {
                PaymentChequeLinePM paymentChequeLine = new PaymentChequeLinePM()
                {
                    PaymentChequeId = entityPM.Id,
                    Amount = entityPM.ForeignAmount,
                    Line = 1,
                    ChangeSetOp = ChangeSetOperation.Insert,
                    Tenant = entityPM.Tenant,
                    Notes = entityPM.Notes,
                    SequenceNumeric = 1
                };
                entityPM.PaymentChequeLines.Add(paymentChequeLine);
            }
        }


        public virtual string IdCounterWrapperGetNumber(int Tenant)
        {
            return (new IdCounterWrapper()).GetNumber(
                    "PaymentCheque", Tenant);
        }

        public virtual string CodeCounterWrapperGetNumber(int Tenant)
        {
            return (new CodeCounterWrapper(false)).GetNumber(
                    "PaymentCheque", Tenant).ToString();
        }

        public virtual string GetLogContactId(int tenant)
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

        public virtual DateTime GetCurrentDateTime(int tenant)
        {
            return TenantServerConfigration.GetCurrentDateTime(tenant);
        }
    }


    public interface IPaymentChequeOnCreatingService
    {
        void OnCreating(PaymentChequePM entityPM);
      
        string GetLogContactId(int tenant);
        DateTime GetCurrentDateTime(int tenant);
    }
}
