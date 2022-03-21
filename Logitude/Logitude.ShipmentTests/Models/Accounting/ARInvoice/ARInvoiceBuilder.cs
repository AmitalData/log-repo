using LLogitude.ShipmentTests.Services;
using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;

namespace Logitude.ShipmentTests.Models.Accounting.ARInvoice
{
    public class ARInvoiceBuilder
    {
        private ARInvoicePM _ARInvoicePM;

        public ARInvoiceBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _ARInvoicePM = new ARInvoicePM();
        }

        public ARInvoiceBuilder Id(string Id)
        {
            _ARInvoicePM.Id = Id;
            return this;
        }
        public ARInvoiceBuilder ShipmentsNumbers(string ShipmentsNumbers)
        {
            _ARInvoicePM.ShipmentsNumbers = ShipmentsNumbers;
            return this;
        }

        public ARInvoiceBuilder Tenant(int Tenant)
        {
            _ARInvoicePM.Tenant = Tenant;
            return this;
        }
        public ARInvoiceBuilder BranchId(string BranchId)
        {
            _ARInvoicePM.BranchId = BranchId;
            return this;
        }
        public ARInvoiceBuilder BillToId(string BillToId)
        {
            _ARInvoicePM.BillToId = BillToId == "TestCustomer" ? PartnersData.CustomerId : null;
            return this;
        }

        public ARInvoiceBuilder PartnerId(string PartnerId)
        {
            _ARInvoicePM.PartnerId = PartnerId == "TestCustomer" ? PartnersData.CustomerId : null;
            return this;
        }

        public ARInvoiceBuilder VATNumber(string VATNumber)
        {
            _ARInvoicePM.VATNumber = VATNumber;
            return this;
        }
        public ARInvoiceBuilder InvoiceDate(string InvoiceDate)//
        {
            _ARInvoicePM.InvoiceDate = DateHelper.FillTodayDate(InvoiceDate);
            return this;
        }
        public ARInvoiceBuilder InvoiceNumber(int InvoiceNumber)
        {
            _ARInvoicePM.InvoiceNumber = InvoiceNumber.ToString();
            return this;
        }
        public ARInvoiceBuilder PaymentTermId(string PaymentTermId)
        {
            _ARInvoicePM.PaymentTermId = PaymentTermId == "Cash" ? BillingData.PaymentTermCashId : null;
            return this;
        }
        public ARInvoiceBuilder DueDate(string DueDate)//
        {
            _ARInvoicePM.DueDate = DateHelper.FillTodayDate(DueDate);
            return this;
        }
        public ARInvoiceBuilder InvoiceCurrencyExchangeRate(double InvoiceCurrencyExchangeRate)
        {
            _ARInvoicePM.InvoiceCurrencyExchangeRate = InvoiceCurrencyExchangeRate;
            return this;
        }
        public ARInvoiceBuilder ProfitCurrencyExchangeRate(double ProfitCurrencyExchangeRate)
        {
            _ARInvoicePM.ProfitCurrencyExchangeRate = ProfitCurrencyExchangeRate;
            return this;
        }
        public ARInvoiceBuilder InvoiceCurrencyId(string InvoiceCurrencyId)
        {
            _ARInvoicePM.InvoiceCurrencyId = InvoiceCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public ARInvoiceBuilder ProfitCurrencyId(string ProfitCurrencyId)
        {
            _ARInvoicePM.ProfitCurrencyId = ProfitCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public ARInvoiceBuilder LocalCurrencyId(string LocalCurrencyId)
        {
            _ARInvoicePM.LocalCurrencyId = LocalCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }
        public ARInvoiceBuilder AmountDue(double AmountDue)
        {
            _ARInvoicePM.AmountDue = AmountDue;
            return this;
        }
        public ARInvoiceBuilder InvoiceExpectedAmount(double InvoiceExpectedAmount)
        {
            _ARInvoicePM.InvoiceExpectedAmount = InvoiceExpectedAmount;
            return this;
        }
        public ARInvoiceBuilder AmountInLocalCurrency(double AmountInLocalCurrency)
        {
            _ARInvoicePM.AmountInLocalCurrency = AmountInLocalCurrency;
            return this;
        }
        public ARInvoiceBuilder AmountInInvoiceCurrency(double AmountInInvoiceCurrency)
        {
            _ARInvoicePM.AmountInInvoiceCurrency = AmountInInvoiceCurrency;
            return this;
        }
        public ARInvoiceBuilder AmountInProfitCurrency(double AmountInProfitCurrency)
        {
            _ARInvoicePM.AmountInProfitCurrency = AmountInProfitCurrency;
            return this;
        }
        public ARInvoiceBuilder CreateDate(DateTime CreateDate)
        {
            _ARInvoicePM.CreateDate = CreateDate;
            return this;
        }
        public ARInvoiceBuilder SubTotalInInvoiceCurrency(double SubTotalInInvoiceCurrency)
        {
            _ARInvoicePM.SubTotalInInvoiceCurrency = SubTotalInInvoiceCurrency;
            return this;
        }
        public ARInvoiceBuilder SubTotalInLocalCurrency(double SubTotalInLocalCurrency)
        {
            _ARInvoicePM.SubTotalInLocalCurrency = SubTotalInLocalCurrency;
            return this;
        }
        public ARInvoiceBuilder InvoiceLines(ARInvoiceLinePM InvoiceLines)
        {
            if(_ARInvoicePM.InvoiceLines == null)
            {
                _ARInvoicePM.InvoiceLines = new List<ARInvoiceLinePM>();
            }
            _ARInvoicePM.InvoiceLines.Add(InvoiceLines);
            return this;
        }

        public ARInvoiceBuilder InvoiceLinesReplace(ARInvoiceLinePM InvoiceLines)
        {         
            _ARInvoicePM.InvoiceLines[0] = InvoiceLines;
            return this;
        }


        public ARInvoicePM Build()
        {
            ARInvoicePM result = _ARInvoicePM;
            this.Reset();
            return result;
        }

        public ARInvoiceBuilder WithModel(ARInvoicePM ARInvoicePM)
        {
            _ARInvoicePM = ARInvoicePM;
            return this;
        }

        public ARInvoiceBuilder WithDefualtValues()
        {
            _ARInvoicePM = new ARInvoicePM
            {
                Tenant = UserTenant.Tenant,
                BranchId = UserTenant.BranchId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                IssuedByUserId = UserTenant.UserId,
                ARInvoiceTypeCode = "IN",
                CreateDate = DateTime.Now,
                ConcurrencyGUID = Guid.NewGuid().ToString(),
                NewConcurrencyGUID = Guid.NewGuid().ToString()
            };
            return this;
        }

    }
}
