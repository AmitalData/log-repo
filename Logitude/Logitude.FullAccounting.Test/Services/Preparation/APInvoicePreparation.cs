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
    public class APInvoicePreparation
    {


        public void Prepare()
        {
            FullAccountingData.APInvoiceId = Create();
        }

        private string Create()
        {
            var apInvoice = CreateInstance();
            var response = APICaller.CallPost<APInvoicePM>(apInvoice, Urls.APInvoicesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private APInvoicePM CreateInstance()
        {
            List<APInvoiceLinePM> apInvoiceLines = new List<APInvoiceLinePM>()
            {
                CreateLine()
            };
            return new APInvoiceBuilder().WithDefualtValues()
                .BranchIdByCode("BerzeitU")
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .IsGeneralInvoice(true)
                .AccountingDate(DateTime.Now)
                .InvoiceCurrencyIdByCode("NIS")
                .InvoiceCurrencyExchangeRate(1)
                .PaymentTermId(BillingData.PaymentTermCashId)
                .ProfitCurrencyIdByCode("NIS")
                .ProfitCurrencyExchangeRate(1)
                .VATNumber("1")
                .SetApproved(true)
                .VendorId(FullAccountingData.VendorId)
                .DueDate(DateTime.Now.AddDays(1))
                .InvoiceNumber(DateTime.Now.Ticks + "")
                .WithAPInvoiceLines(apInvoiceLines)
                .CalculateAmmount()
                .Build();
        }
        private APInvoiceLinePM CreateLine()
        {
            return new APInvoiceLineBuilder().WithDefualtValues()
                .ChargesTypeIdByCode("ITMS")
                .Description("SpecFlowTest")
                .VatTypeId(BillingData.VATTypeZeroId)
                .VatPercentage(0)
                .InvoiceCurrencyAmount(1)
                .ForiegnCurrencyIdByCode("NIS")
                .ForiegnExchangeRate(1)
                .VendorId(FullAccountingData.VendorId)
                .ProfitCurrencyAmount(1)
                .LocalCurrencyAmount(1)
                .ForiegnCurrencyAmount(1)
                .Build();
        }


    }
}
