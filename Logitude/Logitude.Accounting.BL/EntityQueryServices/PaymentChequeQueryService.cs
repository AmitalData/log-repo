using Logitude.Accounting.Data;
using Logitude.Accounting.Data.EntityKeys;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Def.EntityPMs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.BL.EntityQueryServices
{
  public partial  class PaymentChequeQueryService
    {

        public bool CheckIfPaymentChequeExists( string id,string bankAccountId, string uniqueField, int tenant)
        {
            return (from a in context.PaymentCheques where
                    a.BankAccountId == bankAccountId && a.UniqueField == uniqueField && a.Tenant == tenant && a.Id != id
                    select a).Any();
        }

        public PaymentChequePM GetPaymentChequeByChequeNoAndBankAccount(string BankAccountId, string chequeNumber  , int tenant)
        {
           PaymentCheque poco = (from a in context.PaymentCheques
                    where
                          a.BankAccountId == BankAccountId && a.ChequeNumber == chequeNumber && a.Tenant == tenant
                    select a).FirstOrDefault();

            return this.GetEntityPM(poco);
        }

        public PaymentChequePM GetPaymentChequeByChequeNo( string chequeNumber, int tenant)
        {
            PaymentCheque poco = (from a in context.PaymentCheques
                                  where
                                       a.ChequeNumber == chequeNumber && a.Tenant == tenant
                                  select a).FirstOrDefault();

            return this.GetEntityPM(poco);
        }


        public List<PaymentChequePM> GetPaymentChequesByPaymentId(string paymentId, int tenant)
        {
            List<PaymentCheque> list = (from a in context.PaymentCheques
                                        where a.APPaymentId == paymentId && a.Tenant == tenant
                                        select a).ToList();

            return list.Select(rec => this.GetEntityPM(rec)).ToList();
        }



        public override void GetComposition(EntityKeyFields entityKeys, PaymentChequePM entityPM)
        {
            IAccountingContext context = MainContext as AccountingContext;
            PaymentChequeKeys paymentChequeKeys = entityKeys as PaymentChequeKeys;

            PaymentChequeLineQueryService paymentChequeLineQueryService = new PaymentChequeLineQueryService(context);


            entityPM.PaymentChequeLines = paymentChequeLineQueryService.GetMulti(paymentChequeKeys, true);
            if (entityPM.PaymentChequeLines.Count > 0)
            {
                entityPM.PaymentChequeLineLastLine = paymentChequeLineQueryService.GetMaxLineNumber(entityPM.Id, entityPM.Tenant);
            }

        }

    }
}
