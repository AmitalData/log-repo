using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.FullAccounting.Test.Models.Builders
{
    public class APInvoiceBuilder
    {
        private APInvoicePM apInvoicePM;

        public APInvoiceBuilder()
        {
            this.Reset();
        }

        public APInvoicePM Build()
        {
            APInvoicePM result = apInvoicePM;
            this.Reset();
            return result;
        }

        private void Reset()
        {
            apInvoicePM = new APInvoicePM();
        }

        public APInvoiceBuilder BranchId(string branchId)
        {
            apInvoicePM.BranchId = branchId;
            return this;
        }
        public APInvoiceBuilder BranchIdByCode(string code)
        {
            apInvoicePM.BranchId = MapBranchCode(code);
            return this;
        }
        public APInvoiceBuilder VATNumber(string vatNumber)
        {
            apInvoicePM.VATNumber = vatNumber;
            return this;
        }
        
        public APInvoiceBuilder LocalCurrencyId(string localCurrencyId)
        {
            apInvoicePM.LocalCurrencyId = localCurrencyId;
            return this;
        }
        public APInvoiceBuilder PaymentTermId(string paymentTermId)
        {
            apInvoicePM.PaymentTermId = paymentTermId;
            return this;
        }

        public APInvoiceBuilder InvoiceCurrencyId(string invoiceCurrencyId)
        {
            apInvoicePM.InvoiceCurrencyId = invoiceCurrencyId;
            return this;
        }
        public APInvoiceBuilder InvoiceCurrencyIdByCode(string invoiceCurrencyCode)
        {
            apInvoicePM.InvoiceCurrencyId = MapCurrencyCode(invoiceCurrencyCode);
            return this;
        }
        public APInvoiceBuilder InvoiceCurrencyExchangeRate(double? invoiceCurrencyExchangeRate)
        {
            apInvoicePM.InvoiceCurrencyExchangeRate = invoiceCurrencyExchangeRate;
            return this;
        }
        public APInvoiceBuilder ProfitCurrencyExchangeRate(double? profitCurrencyExchangeRate)
        {
            apInvoicePM.ProfitCurrencyExchangeRate = profitCurrencyExchangeRate;
            return this;
        }
        public APInvoiceBuilder SubTotalInLocalCurrency(double? subTotalInLocalCurrency)
        {
            apInvoicePM.SubTotalInLocalCurrency = subTotalInLocalCurrency;
            return this;
        }
        public APInvoiceBuilder SubTotalInInvoiceCurrency(double? subTotalInInvoiceCurrency)
        {
            apInvoicePM.SubTotalInInvoiceCurrency = subTotalInInvoiceCurrency;
            return this;
        }
        public APInvoiceBuilder AmountInLocalCurrency(double? amountInLocalCurrency)
        {
            apInvoicePM.AmountInLocalCurrency = amountInLocalCurrency;
            return this;
        }
        public APInvoiceBuilder AmountInInvoiceCurrency(double? amountInInvoiceCurrency)
        {
            apInvoicePM.AmountInInvoiceCurrency = amountInInvoiceCurrency;
            return this;
        }
        public APInvoiceBuilder AmountInProfitCurrency(double? amountInProfitCurrency)
        {
            apInvoicePM.AmountInProfitCurrency = amountInProfitCurrency;
            return this;
        }
        public APInvoiceBuilder AmountDue(double? amountDue)
        {
            apInvoicePM.AmountDue = amountDue;
            return this;
        }
        public APInvoiceBuilder AmountDueInLocalCurrency(double? amountDueInLocalCurrency)
        {
            apInvoicePM.AmountDueInLocalCurrency = amountDueInLocalCurrency;
            return this;
        }
        public APInvoiceBuilder AmountDueInProfitCurrency(double? amountDueInProfitCurrency)
        {
            apInvoicePM.AmountDueInProfitCurrency = amountDueInProfitCurrency;
            return this;
        }

        public APInvoiceBuilder SetApproved(bool setApproved)
        {
            apInvoicePM.SetApproved = setApproved;
            return this;
        }
        public APInvoiceBuilder IsGeneralInvoice(bool isGeneralInvoice)
        {
            apInvoicePM.IsGeneralInvoice = isGeneralInvoice;
            return this;
        }
        public APInvoiceBuilder InvoiceNumber(string invoiceNumber)
        {
            apInvoicePM.InvoiceNumber = invoiceNumber;
            return this;
        }
        

        public APInvoiceBuilder DueDate(DateTime dueDate)
        {
            apInvoicePM.DueDate = dueDate;
            return this;
        }
        public APInvoiceBuilder AccountingDate(DateTime accountingDate)
        {
            apInvoicePM.AccountingDate = accountingDate;
            return this;
        }

        public APInvoiceBuilder InvoiceCurrencyCode(string invoiceCurrencyCode)
        {
            apInvoicePM.InvoiceCurrencyCode = invoiceCurrencyCode;
            return this;
        }
        public APInvoiceBuilder ProfitCurrencyIdByCode(string profitCurrencyCode)
        {
            apInvoicePM.ProfitCurrencyId = MapCurrencyCode(profitCurrencyCode);
            return this;
        }

        public APInvoiceBuilder StatusCode(string statusCode)
        {
            apInvoicePM.StatusCode = statusCode;
            return this;
        }
        public APInvoiceBuilder VendorId(string vendorId)
        {
            apInvoicePM.VendorId = vendorId;
            return this;
        }
        public APInvoiceBuilder WithAPInvoiceLines(List<APInvoiceLinePM> apInvoiceLinePM)
        {
            apInvoicePM.InvoiceLines = apInvoiceLinePM;
            return this;
        }

        public APInvoiceBuilder CalculateAmmount()
        {

            foreach (var item in apInvoicePM.InvoiceLines)
            {
                apInvoicePM.SubTotalInInvoiceCurrency += item.InvoiceCurrencyAmount;
                apInvoicePM.SubTotalInLocalCurrency += item.LocalCurrencyAmount;
                apInvoicePM.AmountDue += item.InvoiceCurrencyAmount;
                apInvoicePM.AmountDueInLocalCurrency += item.LocalCurrencyAmount;
                apInvoicePM.InvoiceExpectedAmount += item.LocalCurrencyAmount;
                apInvoicePM.AmountInLocalCurrency += item.LocalCurrencyAmount;
                apInvoicePM.AmountInInvoiceCurrency += item.InvoiceCurrencyAmount;
                apInvoicePM.AmountDueInProfitCurrency += item.ProfitCurrencyAmount;
                apInvoicePM.AmountInProfitCurrency += item.ProfitCurrencyAmount;
                apInvoicePM.AmountInProfitCurrency_Summary += item.ProfitCurrencyAmount;
                apInvoicePM.AmountInInvoiceCurrency_Summary += item.InvoiceCurrencyAmount;
                apInvoicePM.AmountInLocalCurrency_Summary += item.LocalCurrencyAmount;
            }
            return this;
        }

       

        public APInvoiceBuilder WithModel(APInvoicePM tMEmployeeTime)
        {
            apInvoicePM = tMEmployeeTime;
            return this;
        }

        public APInvoiceBuilder WithDefualtValues()
        {
            apInvoicePM = new APInvoicePM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                InvoiceDate = DateTime.Now,
                AmountDue = 0,
                AmountDueInLocalCurrency = 0,
                AmountDueInProfitCurrency = 0,
                AmountInInvoiceCurrency = 0,
                AmountInProfitCurrency = 0,
                AmountInInvoiceCurrency_Summary = 0,
                AmountInLocalCurrency_Summary = 0,
                AmountInProfitCurrency_Summary = 0,
                InvoiceExpectedAmount = 0,
                RefundAmount = 0,
                SubTotalInLocalCurrency = 0,
                AmountInLocalCurrency = 0,
                SubTotalInInvoiceCurrency = 0,
                StatusCode = "WA"

            };
            return this;
        }
        private string MapCurrencyCode(string invoiceCurrencyCode)
        {
            switch (invoiceCurrencyCode)
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
