using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Builders;
using Logitude.FullAccounting.Test.Models.Codes;
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
    public class APPaymentPreparation
    {


        public void Prepare()
        {
            FullAccountingData.APPaymenId = Create();
        }

        private string Create()
        {
            var apPayment = CreateInstance();
            var response = APICaller.CallPost<APPaymentPM>(apPayment, Urls.APPaymentsController, UserTenant.Token);
            return response.Data?.Id;
        }

        private APPaymentPM CreateInstance()
        {
            return new APPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode(BranchCodes.BerzeitU)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .PaymentCurrencyIdByCode(CurrencyCodes.NIS)
                .PaymentCurrencyExchangeRate(1)
                .AccountingPaymentMethodId(FullAccountingData.CashPaymentMethodId)
                .AmountInPaymentCurrency(1)
                .OpenAmount(1)
                .AmountInLocalCurrency(1)
                .VendorId(FullAccountingData.VendorId)
                .VendorAddressId(FullAccountingData.VendorMainAddressId)
                .VendorPartnerTypeId("VD")
                .PaymentNo(DateTime.Now.Ticks + "")
                .TaxDeductionLocalAmount(1)
                .TaxDeductionPercentage(1)
                .Build();
        }
       


    }
}
