using Logitude.Test.Base.Models.UserTenantPreparation;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            _APInvoicePM.VendorId = VendorId;
            return this;
        }
        public APInvoiceBuilder VATNumber(string VATNumber)
        {
            _APInvoicePM.VATNumber = VATNumber;
            return this;
        }
        public APInvoiceBuilder InvoiceDate(DateTime InvoiceDate)
        {
            _APInvoicePM.InvoiceDate = InvoiceDate;
            return this;
        }
        public APInvoiceBuilder InvoiceNumber(string InvoiceNumber)
        {
            _APInvoicePM.InvoiceNumber = InvoiceNumber;
            return this;
        }
        public APInvoiceBuilder PaymentTermId(string PaymentTermId)
        {
            _APInvoicePM.PaymentTermId = PaymentTermId;
            return this;
        }
        public APInvoiceBuilder DueDate(DateTime DueDate)
        {
            _APInvoicePM.DueDate = DueDate;
            return this;
        }
        public APInvoiceBuilder InvoiceCurrencyExchangeRate(double InvoiceCurrencyExchangeRate)
        {
            _APInvoicePM.InvoiceCurrencyExchangeRate = InvoiceCurrencyExchangeRate;
            return this;
        }
        public APInvoiceBuilder InvoiceCurrencyId(string InvoiceCurrencyId)
        {
            _APInvoicePM.InvoiceCurrencyId = InvoiceCurrencyId;
            return this;
        }
        public APInvoiceBuilder ProfitCurrencyId(string ProfitCurrencyId)
        {
            _APInvoicePM.ProfitCurrencyId = ProfitCurrencyId;
            return this;
        }
        public APInvoiceBuilder LocalCurrencyId(string LocalCurrencyId)
        {
            _APInvoicePM.LocalCurrencyId = LocalCurrencyId;
            return this;
        }
        public APInvoiceBuilder AmountDue(double AmountDue)
        {
            _APInvoicePM.AmountDue = AmountDue;
            return this;
        }
        public APInvoiceBuilder CreateDate(DateTime CreateDate)
        {
            _APInvoicePM.CreateDate = CreateDate;
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
