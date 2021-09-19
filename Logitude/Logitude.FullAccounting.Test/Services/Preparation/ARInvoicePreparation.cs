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
    public class ARInvoicePreparation
    {


        public void Prepare()
        {
            FullAccountingData.ARInvoiceId = Create();
        }

        private string Create()
        {
            var arInvoice = CreateInstance();
            var response = APICaller.CallPost<ARInvoicePM>(arInvoice, Urls.ARInvoicesController, UserTenant.Token);
            return response.Data?.Id;
        }

        private ARInvoicePM CreateInstance()
        {
            List<ARInvoiceLinePM> aRInvoiceLines = new List<ARInvoiceLinePM>()
            {
                CreateLine()
            };
            return new ARInvoicePMBuilder().WithDefualtValues()
                .BranchIdByCode("BerzeitU")
                .LocalCurrencyId(UserTenant.LocalCurrencyId)
                .ARInvoiceTypeCode("IN")
                .IsFullAccounting(true)
                .IsGeneralInvoice(true)
                .InvoiceCurrencyIdByCode("NIS")
                .InvoiceCurrencyExchangeRate(1)
                .PaymentTermId(BillingData.PaymentTermCashId)
                .ProfitCurrencyIdByCode("NIS")
                .BillToId(FullAccountingData.CustomerId)
                .BillToPartnerTypeId("CS")
                .ProfitCurrencyExchangeRate(1)
                .SetApproved(true)
                .DueDate(DateTime.Now.AddDays(1))
                .WithaRInvoiceLines(aRInvoiceLines)
                .CalculateAmmount()
                .VatNumber("1")
                .Build();
        }
        private ARInvoiceLinePM CreateLine()
        {
            return new ARInvoiceLinePMBuilder().WithDefualtValues()
                .ForiegnCurrencyIdByCode("NIS")
                .ForiegnCurrencyCode("NIS")
                .ForiegnExchangeRate(1)
                .LineActionCode(ARInvoiceLineActionsEnum.Revenue)
                .InvoiceCurrencyCode("NIS")
                .InvoiceLocalCurrencyCode("NIS")
                .ChargesTypeIdByCode("ITMS")
                .Description("SpecFlowTest")
                .LocalDescription("SpecFlowTest")
                .VatTypeId(BillingData.VATTypeZeroId)
                .VatPercentage(0)
                .ExchangeRateDate(DateTime.Now)
                .Quantity(1)
                .InvoiceCurrencyAmount(1)
                .UnitPrice(1)
                .ProfitCurrencyAmount(1)
                .LocalCurrencyAmount(1)
                .ForiegnCurrencyAmount(1)
                .Build();
        }


    }
}
