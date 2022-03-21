using Logitude.FullAccounting.Test.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class PaymentChequesPreparation
    {
        
        public void Prepare()
        {
            FullAccountingData.PaymentCheque1 = Create();
        }

        public string Create()
        {
            var paymentCheque = CreateInstance();
            var response = APICaller.CallPost<PaymentChequePM>(paymentCheque, Urls.PaymentChequesController, UserTenant.Token);
            return response.Data?.Id;
        }
        private PaymentChequePM CreateInstance()
        {
            return CreatePaymentChequeNISInstance();
        }

        private PaymentChequePM CreatePaymentChequeNISInstance()
        {
            return new PaymentChequePM()
            {
                Tenant = UserTenant.Tenant,
                CurrencyId = BillingData.CurrencyNISId,
                ChequeNumber = DateTime.Now.Ticks.ToString().Substring(3),
                ValueDate = DateTime.Now,
                BankAccountId = new BankAccountPreparation().GetNewBankAccount(),
                LocalAmount = 100,
                PayToGLAccountId = new AccountPreparation().Create(),
                PayToName = "SPC F Test ",
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };
        }
        
    }
}
