using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
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

namespace Logitude.FullAccounting.Test.Services.Preparation
{
    public class ARPaymentPreparation
    {


        public void Prepare()
        {
            FullAccountingData.ARPaymenId = Create();
        }

        private string Create()
        {
            var arPayment = CreateInstance();
            var response = APICaller.CallPost<ARPaymentPM>(arPayment, Urls.ARPaymentController, UserTenant.Token);
            return response.Data?.Id;
        }

        private ARPaymentPM CreateInstance()
        {
            return new ARPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode("BerzeitU")
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .PaymentCurrencyIdByCode("NIS")
                .PaymentCurrencyExchangeRate(1)
                .PartnerId(FullAccountingData.CustomerId)
                .IsFullAccounting(true)
                .AccountingPaymentMethodId(FullAccountingData.CashPaymentMethodId)
                .AccountingPaymentMethodCode("CA")
                .BillToId(FullAccountingData.CustomerId)
                .AmountInPaymentCurrency(1)
                .OpenAmount(1)
                .GLAccountId(FullAccountingData.CustomerGLAccountId)
                .BillToAddressId(FullAccountingData.CustomerMainAddressId)
                .CashbookId(FullAccountingData.CashBookCash1)
                .Build();
        }
       


    }
}
