using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.FullAccounting.Test.Models;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class ARPaymentPMBuilder
    {
        private ARPaymentPM arPaymentPM;

        public ARPaymentPMBuilder()
        {
            this.Reset();
        }

        public ARPaymentPM Build()
        {
            ARPaymentPM result = arPaymentPM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            arPaymentPM = new ARPaymentPM();
        }

        public ARPaymentPMBuilder BranchId(string branchId)
        {
            arPaymentPM.BranchId = branchId;
            return this;
        }
        public ARPaymentPMBuilder BranchIdByCode(string code)
        {
            arPaymentPM.BranchId = MapBranchCode(code);
            return this;
        }
        public ARPaymentPMBuilder AccountingPaymentMethodCode(string accountingPaymentMethodCode)
        {
            arPaymentPM.AccountingPaymentMethodCode = accountingPaymentMethodCode;
            return this;
        }

        public ARPaymentPMBuilder LocalCurrencyId(string localCurrencyId)
        {
            arPaymentPM.LocalCurrencyId = localCurrencyId;
            return this;
        }
        

        public ARPaymentPMBuilder PaymentCurrencyId(string PaymentCurrencyId)
        {
            arPaymentPM.PaymentCurrencyId = PaymentCurrencyId;
            return this;
        }
        public ARPaymentPMBuilder PaymentCurrencyIdByCode(string PaymentCurrencyCode)
        {
            arPaymentPM.PaymentCurrencyId = MapCurrencyCode(PaymentCurrencyCode);
            return this;
        }
        public ARPaymentPMBuilder PaymentCurrencyExchangeRate(double? PaymentCurrencyExchangeRate)
        {
            arPaymentPM.PaymentCurrencyExchangeRate = PaymentCurrencyExchangeRate;
            return this;
        }
        public ARPaymentPMBuilder ProfitCurrencyExchangeRate(double? profitCurrencyExchangeRate)
        {
            arPaymentPM.ProfitCurrencyExchangeRate = profitCurrencyExchangeRate;
            return this;
        }
        
        public ARPaymentPMBuilder AmountInLocalCurrency(double? amountInLocalCurrency)
        {
            arPaymentPM.AmountInLocalCurrency = amountInLocalCurrency;
            return this;
        }
        public ARPaymentPMBuilder AmountInPaymentCurrency(double? amountInPaymentCurrency)
        {
            arPaymentPM.AmountInPaymentCurrency = amountInPaymentCurrency;
            return this;
        }
        public ARPaymentPMBuilder AmountInProfitCurrency(double? amountInProfitCurrency)
        {
            arPaymentPM.AmountInProfitCurrency = amountInProfitCurrency;
            return this;
        }
        
       
        public ARPaymentPMBuilder SetApproved(bool setApproved)
        {
            arPaymentPM.SetApproved = setApproved;
            return this;
        }
       
        public ARPaymentPMBuilder IsFullAccounting(bool isFullAccounting)
        {
            arPaymentPM.IsFullAccounting = isFullAccounting;
            return this;
        }

        

        public ARPaymentPMBuilder PaymentCurrencyCode(string PaymentCurrencyCode)
        {
            arPaymentPM.PaymentCurrencyCode = PaymentCurrencyCode;
            return this;
        }
        

        public ARPaymentPMBuilder StatusCode(string statusCode)
        {
            arPaymentPM.StatusCode = statusCode;
            return this;
        }
        public ARPaymentPMBuilder BillToId(string billToId)
        {
            arPaymentPM.BillToId = billToId;
            return this;
        }
        public ARPaymentPMBuilder BillToPartnerTypeId(string billToPartnerTypeId)
        {
            arPaymentPM.BillToPartnerTypeId = billToPartnerTypeId;
            return this;
        }

        public ARPaymentPMBuilder WithInvoicesLedgerTransactions(List<LedgerTransactionPM> ledgerTransactionPMs)
        {
            arPaymentPM.InvoicesLedgerTransactions = ledgerTransactionPMs;
            return this;
        }

        public ARPaymentPMBuilder CalculateAmmount()
        {

            foreach (var item in arPaymentPM.InvoicesLedgerTransactions)
            {
                
                
                
            }
            return this;
        }

       

        public ARPaymentPMBuilder WithModel(ARPaymentPM tMEmployeeTime)
        {
            arPaymentPM = tMEmployeeTime;
            return this;
        }

        public ARPaymentPMBuilder WithDefualtValues()
        {
            arPaymentPM = new ARPaymentPM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                
                AmountInPaymentCurrency = 0,
                
                AmountInLocalCurrency = 0,
               

        };
            return this;
        }
        private string MapCurrencyCode(string PaymentCurrencyCode)
        {
            switch (PaymentCurrencyCode)
            {
                case "NIS":
                    return BillingData.CurrencyNISId;
                case "EUR":
                    return BillingData.CurrencyEURId;
                default:
                    return null;
            }
        }
        private string MapBranchCode(string branchCode)
        {
            switch (branchCode)
            {
                case "BZU":
                    return FullAccountingData.BZUBranchID;
                case "RMLAH":
                    return FullAccountingData.RamallahBranchID;
                default:
                    return null;
            }
        }


    }
}
