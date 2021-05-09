using Logitude.AccountingTests.Services;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;

namespace Logitude.AccountingTests.Models.APInvoiceBuilders
{
    public class APInvoiceBuilder
    {
        private APInvoicePM _APInvoicePM;

        public APInvoiceBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _APInvoicePM = new APInvoicePM();
        }

        public APInvoiceBuilder Id(string Id)
        {
            _APInvoicePM.Id = Id;
            return this;
        }
        public APInvoiceBuilder ShipmentsNumbers(string ShipmentsNumbers)
        {
            _APInvoicePM.ShipmentsNumbers = ShipmentsNumbers;
            return this;
        }

        public APInvoiceBuilder Tenant(int Tenant)
        {
            _APInvoicePM.Tenant = Tenant;
            return this;
        }
        public APInvoiceBuilder BranchId(string BranchId)
        {
            _APInvoicePM.BranchId = BranchId;
            return this;
        }
        public APInvoiceBuilder VendorId(string VendorId)
        {
            _APInvoicePM.VendorId = VendorId == "TestVendor" ? PartnersData.VendorId : null;
            return this;
        }
        public APInvoiceBuilder VATNumber(string VATNumber)
        {
            _APInvoicePM.VATNumber = VATNumber;
            return this;
        }
        public APInvoiceBuilder InvoiceDate(string InvoiceDate)//
        {
            _APInvoicePM.InvoiceDate = DateHelper.FillTodayDate(InvoiceDate);
            return this;
        }
        public APInvoiceBuilder InvoiceNumber(int InvoiceNumber)
        {
            _APInvoicePM.InvoiceNumber = InvoiceNumber.ToString();
            return this;
        }
        public APInvoiceBuilder PaymentTermId(string PaymentTermId)
        {
            _APInvoicePM.PaymentTermId = PaymentTermId == "Cash" ? BillingData.PaymentTermCashId : null;
            return this;
        }
        public APInvoiceBuilder DueDate(string DueDate)//
        {
            _APInvoicePM.DueDate = DateHelper.FillTodayDate(DueDate);
            return this;
        }
        public APInvoiceBuilder InvoiceCurrencyExchangeRate(double InvoiceCurrencyExchangeRate)
        {
            _APInvoicePM.InvoiceCurrencyExchangeRate = InvoiceCurrencyExchangeRate;
            return this;
        }
        public APInvoiceBuilder ProfitCurrencyExchangeRate(double ProfitCurrencyExchangeRate)
        {
            _APInvoicePM.ProfitCurrencyExchangeRate = ProfitCurrencyExchangeRate;
            return this;
        }
        public APInvoiceBuilder InvoiceCurrencyId(string InvoiceCurrencyId)
        {
            _APInvoicePM.InvoiceCurrencyId = InvoiceCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public APInvoiceBuilder ProfitCurrencyId(string ProfitCurrencyId)
        {
            _APInvoicePM.ProfitCurrencyId = ProfitCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public APInvoiceBuilder LocalCurrencyId(string LocalCurrencyId)
        {
            _APInvoicePM.LocalCurrencyId = LocalCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public APInvoiceBuilder AmountDue(double AmountDue)
        {
            _APInvoicePM.AmountDue = AmountDue;
            return this;
        }
        public APInvoiceBuilder InvoiceExpectedAmount(double InvoiceExpectedAmount)
        {
            _APInvoicePM.InvoiceExpectedAmount = InvoiceExpectedAmount;
            return this;
        }
        public APInvoiceBuilder AmountInLocalCurrency(double AmountInLocalCurrency)
        {
            _APInvoicePM.AmountInLocalCurrency = AmountInLocalCurrency;
            return this;
        }
        public APInvoiceBuilder AmountInInvoiceCurrency(double AmountInInvoiceCurrency)
        {
            _APInvoicePM.AmountInInvoiceCurrency = AmountInInvoiceCurrency;
            return this;
        }
        public APInvoiceBuilder AmountInProfitCurrency(double AmountInProfitCurrency)
        {
            _APInvoicePM.AmountInProfitCurrency = AmountInProfitCurrency;
            return this;
        }
        public APInvoiceBuilder CreateDate(DateTime CreateDate)
        {
            _APInvoicePM.CreateDate = CreateDate;
            return this;
        }
        public APInvoiceBuilder SubTotalInInvoiceCurrency(double SubTotalInInvoiceCurrency)
        {
            _APInvoicePM.SubTotalInInvoiceCurrency = SubTotalInInvoiceCurrency;
            return this;
        }
        public APInvoiceBuilder SubTotalInLocalCurrency(double SubTotalInLocalCurrency)
        {
            _APInvoicePM.SubTotalInLocalCurrency = SubTotalInLocalCurrency;
            return this;
        }
        public APInvoiceBuilder InvoiceLines(APInvoiceLinePM InvoiceLines)
        {
            if(_APInvoicePM.InvoiceLines == null)
            {
                _APInvoicePM.InvoiceLines = new List<APInvoiceLinePM>();
            }
            _APInvoicePM.InvoiceLines.Add(InvoiceLines);
            return this;
        }


        public APInvoicePM Build()
        {
            APInvoicePM result = _APInvoicePM;
            this.Reset();
            return result;
        }

        public APInvoiceBuilder WithModel(APInvoicePM aPInvoicePM)
        {
            _APInvoicePM = aPInvoicePM;
            return this;
        }

        public APInvoiceBuilder WithDefualtValues()
        {
            _APInvoicePM = new APInvoicePM
            {
                Tenant = UserTenant.Tenant,
                BranchId = UserTenant.BranchId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(UserTenant.Tenant),
            };
            return this;
        }

    }
}
