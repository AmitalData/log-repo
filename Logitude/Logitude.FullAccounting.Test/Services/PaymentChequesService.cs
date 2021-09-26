
using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Services.Preparation;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class PaymentChequesService
    {



        public PaymentChequePM Create(Table table)
        {
            dynamic paymentChequeTable = table.CreateDynamicInstance();
            var paymentCheque = new PaymentChequePM()
            {
                Tenant = UserTenant.Tenant,
                CurrencyId = BillingData.CurrencyNISId,
                ChequeNumber = DateTime.Now.Ticks.ToString().Substring(3),
                ValueDate = DateTime.Now,
                BankAccountId = new BankAccountPreparation().GetNewBankAccount(),
                LocalAmount = (decimal)paymentChequeTable.LocalAmount,
                PayToGLAccountId = new AccountPreparation().Create(),
                PayToName = paymentChequeTable.PayToName.ToString(),
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now

            };

            return paymentCheque;

        }


    }
}
