using Logitude.FullAccounting.Test.Models;
using Logitude.FullAccounting.Test.Models.Codes;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class ARInvoicePMBuilder
    {
        private ARInvoicePM arInvoicePM;

        public ARInvoicePMBuilder()
        {
            this.Reset();
        }

        public ARInvoicePM Build()
        {
            ARInvoicePM result = arInvoicePM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            arInvoicePM = new ARInvoicePM();
        }

        public ARInvoicePMBuilder BranchId(string branchId)
        {
            arInvoicePM.BranchId = branchId;
            return this;
        }
        public ARInvoicePMBuilder BranchIdByCode(string code)
        {
            arInvoicePM.BranchId = MapBranchCode(code);
            return this;
        }
        public ARInvoicePMBuilder ARInvoiceTypeCode(string arInvoiceTypeCode)
        {
            arInvoicePM.ARInvoiceTypeCode = arInvoiceTypeCode;
            return this;
        }

        public ARInvoicePMBuilder LocalCurrencyId(string localCurrencyId)
        {
            arInvoicePM.LocalCurrencyId = localCurrencyId;
            return this;
        }
        public ARInvoicePMBuilder PaymentTermId(string paymentTermId)
        {
            arInvoicePM.PaymentTermId = paymentTermId;
            return this;
        }

        public ARInvoicePMBuilder InvoiceCurrencyId(string invoiceCurrencyId)
        {
            arInvoicePM.InvoiceCurrencyId = invoiceCurrencyId;
            return this;
        }
        public ARInvoicePMBuilder InvoiceCurrencyIdByCode(string invoiceCurrencyCode)
        {
            arInvoicePM.InvoiceCurrencyId = MapCurrencyCode(invoiceCurrencyCode);
            return this;
        }
        public ARInvoicePMBuilder InvoiceCurrencyExchangeRate(double? invoiceCurrencyExchangeRate)
        {
            arInvoicePM.InvoiceCurrencyExchangeRate = invoiceCurrencyExchangeRate;
            return this;
        }
        public ARInvoicePMBuilder ProfitCurrencyExchangeRate(double? profitCurrencyExchangeRate)
        {
            arInvoicePM.ProfitCurrencyExchangeRate = profitCurrencyExchangeRate;
            return this;
        }
        public ARInvoicePMBuilder SubTotalInLocalCurrency(double? subTotalInLocalCurrency)
        {
            arInvoicePM.SubTotalInLocalCurrency = subTotalInLocalCurrency;
            return this;
        }
        public ARInvoicePMBuilder SubTotalInInvoiceCurrency(double? subTotalInInvoiceCurrency)
        {
            arInvoicePM.SubTotalInInvoiceCurrency = subTotalInInvoiceCurrency;
            return this;
        }
        public ARInvoicePMBuilder AmountInLocalCurrency(double? amountInLocalCurrency)
        {
            arInvoicePM.AmountInLocalCurrency = amountInLocalCurrency;
            return this;
        }
        public ARInvoicePMBuilder AmountInInvoiceCurrency(double? amountInInvoiceCurrency)
        {
            arInvoicePM.AmountInInvoiceCurrency = amountInInvoiceCurrency;
            return this;
        }
        public ARInvoicePMBuilder AmountInProfitCurrency(double? amountInProfitCurrency)
        {
            arInvoicePM.AmountInProfitCurrency = amountInProfitCurrency;
            return this;
        }
        public ARInvoicePMBuilder AmountDue(double? amountDue)
        {
            arInvoicePM.AmountDue = amountDue;
            return this;
        }
        public ARInvoicePMBuilder AmountDueInLocalCurrency(double? amountDueInLocalCurrency)
        {
            arInvoicePM.AmountDueInLocalCurrency = amountDueInLocalCurrency;
            return this;
        }
        public ARInvoicePMBuilder AmountDueInProfitCurrency(double? amountDueInProfitCurrency)
        {
            arInvoicePM.AmountDueInProfitCurrency = amountDueInProfitCurrency;
            return this;
        }
       
        public ARInvoicePMBuilder SetApproved(bool setApproved)
        {
            arInvoicePM.SetApproved = setApproved;
            return this;
        }
        public ARInvoicePMBuilder IsGeneralInvoice(bool isGeneralInvoice)
        {
            arInvoicePM.IsGeneralInvoice = isGeneralInvoice;
            return this;
        }
        public ARInvoicePMBuilder IsFullAccounting(bool isFullAccounting)
        {
            arInvoicePM.IsFullAccounting = isFullAccounting;
            return this;
        }

        public ARInvoicePMBuilder DueDate(DateTime dueDate)
        {
            arInvoicePM.DueDate = dueDate;
            return this;
        }

        public ARInvoicePMBuilder InvoiceCurrencyCode(string invoiceCurrencyCode)
        {
            arInvoicePM.InvoiceCurrencyCode = invoiceCurrencyCode;
            return this;
        }
        public ARInvoicePMBuilder ProfitCurrencyIdByCode(string profitCurrencyCode)
        {
            arInvoicePM.ProfitCurrencyId = MapCurrencyCode(profitCurrencyCode);
            return this;
        }

        public ARInvoicePMBuilder StatusCode(string statusCode)
        {
            arInvoicePM.StatusCode = statusCode;
            return this;
        }
        public ARInvoicePMBuilder BillToId(string billToId)
        {
            arInvoicePM.BillToId = billToId;
            return this;
        }
        public ARInvoicePMBuilder BillToPartnerTypeId(string billToPartnerTypeId)
        {
            arInvoicePM.BillToPartnerTypeId = billToPartnerTypeId;
            return this;
        }

        public ARInvoicePMBuilder WithaRInvoiceLines(List<ARInvoiceLinePM> aRInvoiceLinePM)
        {
            arInvoicePM.InvoiceLines = aRInvoiceLinePM;
            return this;
        }

        public ARInvoicePMBuilder CalculateAmmount()
        {

            foreach (var item in arInvoicePM.InvoiceLines)
            {
                arInvoicePM.SubTotalInInvoiceCurrency += item.InvoiceCurrencyAmount;
                arInvoicePM.SubTotalInLocalCurrency += item.LocalCurrencyAmount;
                arInvoicePM.AmountDue += item.InvoiceCurrencyAmount;
                arInvoicePM.AmountDueInLocalCurrency += item.LocalCurrencyAmount;
                arInvoicePM.AmountInLocalCurrency += item.LocalCurrencyAmount;
                arInvoicePM.AmountInInvoiceCurrency += item.InvoiceCurrencyAmount;
                arInvoicePM.AmountDueInProfitCurrency += item.ProfitCurrencyAmount;
            }
            return this;
        }

        public ARInvoicePMBuilder VatNumber(string vatNumber)
        {
            arInvoicePM.VatNumber = vatNumber;
            return this;
        }

        public ARInvoicePMBuilder WithModel(ARInvoicePM tMEmployeeTime)
        {
            arInvoicePM = tMEmployeeTime;
            return this;
        }

        public ARInvoicePMBuilder WithDefualtValues()
        {
            arInvoicePM = new ARInvoicePM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                IssuedByUserId = UserTenant.UserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                InvoiceDate = DateTime.Now,
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInInvoiceCurrency = 0,
                TotaVatableAmountForTaxReport = 0,
                TotalAmountForTaxReport = 0,
                SubTotalInLocalCurrency = 0,
                AmountInLocalCurrency = 0,
                SubTotalInInvoiceCurrency = 0

        };
            return this;
        }
        private string MapCurrencyCode(string invoiceCurrencyCode)
        {
            switch (invoiceCurrencyCode)
            {
                case CurrencyCodes.NIS:
                    return BillingData.CurrencyNISId;
                case CurrencyCodes.EUR:
                    return BillingData.CurrencyEURId;
                default:
                    return null;
            }
        }
        private string MapBranchCode(string branchCode)
        {
            switch (branchCode)
            {
                case BranchCodes.BerzeitU:
                    return FullAccountingData.BZUBranchID;
                case BranchCodes.Ramallah:
                    return FullAccountingData.RamallahBranchID;
                default:
                    return null;
            }
        }


    }
}
