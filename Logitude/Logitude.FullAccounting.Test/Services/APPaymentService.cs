
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
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.FullAccounting.Test.Services
{
    public class APPaymentService
    {
        

        public APPaymentPM Create(Table table)
        {
            dynamic arPaymentTable = table.CreateDynamicInstance();
            var arPayment = new APPaymentPMBuilder().WithDefualtValues()
                .BranchIdByCode((string)arPaymentTable.Branch)
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .PaymentCurrencyIdByCode((string)arPaymentTable.PaymentCurrency)
                .PaymentCurrencyExchangeRate((double)arPaymentTable.PaymentCurrencyExchangeRate)
                .AccountingPaymentMethodId(FullAccountingData.CashPaymentMethodId)
                .AmountInPaymentCurrency((double)arPaymentTable.AmountInPaymentCurrency)
                .OpenAmount((double)arPaymentTable.AmountInPaymentCurrency)
                .AmountInLocalCurrency((double)arPaymentTable.AmountInLocalCurrency)
                .VendorId(FullAccountingData.VendorId)
                .VendorAddressId(FullAccountingData.VendorMainAddressId)
                .VendorPartnerTypeId("VD")
                .PaymentNo(DateTime.Now.Ticks +"")
                .TaxDeductionLocalAmount((decimal)arPaymentTable.TaxDeductionLocalAmount)
                .TaxDeductionPercentage((int)arPaymentTable.TaxDeductionPercentage)
                .Build();

            return arPayment;

        }


    }
}
