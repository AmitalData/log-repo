using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.AccountingTests.Models.Builders
{
    public class APInvoiceLineBuilder
    {
        private APInvoiceLinePM _APInvoiceLinePM;

        public APInvoiceLineBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _APInvoiceLinePM = new APInvoiceLinePM();
        }

        public APInvoiceLineBuilder Tenant(int Tenant)
        {
            _APInvoiceLinePM.Tenant = Tenant;
            return this;
        }
        public APInvoiceLineBuilder ChargesTypeCode(string ChargesTypeCode)
        {
            _APInvoiceLinePM.ChargesTypeCode = ChargesTypeCode;
            return this;
        }
        public APInvoiceLineBuilder ChargesTypeId(string ChargesTypeId)
        {
            _APInvoiceLinePM.ChargesTypeId = ChargesTypeId;
            return this;
        }
        public APInvoiceLineBuilder Description(string Description)
        {
            _APInvoiceLinePM.Description = Description;
            return this;
        }
      
        public APInvoiceLineBuilder InvoiceCurrencyAmount(double InvoiceCurrencyAmount)
        {
            _APInvoiceLinePM.InvoiceCurrencyAmount = InvoiceCurrencyAmount;
            return this;
        }
        public APInvoiceLineBuilder ForiegnCurrencyAmount(double ForiegnCurrencyAmount)
        {
            _APInvoiceLinePM.ForiegnCurrencyAmount = ForiegnCurrencyAmount;
            return this;
        }
 
        public APInvoiceLineBuilder VatTypeId(string VatTypeId)
        {
            _APInvoiceLinePM.VatTypeId = VatTypeId;
            return this;
        }
        public APInvoiceLineBuilder VatTypeName(string VatTypeName)
        {
            _APInvoiceLinePM.VatTypeName = VatTypeName;
            return this;
        }
        public APInvoiceLineBuilder EntityId(string EntityId)
        {
            _APInvoiceLinePM.EntityId = EntityId;
            return this;
        }
        public APInvoiceLineBuilder VatPercentage(double VatPercentage)
        {
            _APInvoiceLinePM.VatPercentage = VatPercentage;
            return this;
        }

        public APInvoiceLinePM Build()
        {
            APInvoiceLinePM result = _APInvoiceLinePM;
            this.Reset();
            return result;
        }

        public APInvoiceLineBuilder WithModel(APInvoiceLinePM APInvoiceLinePM)
        {
            _APInvoiceLinePM = APInvoiceLinePM;
            return this;
        }

        public APInvoiceLineBuilder WithDefualtValues()
        {
            _APInvoiceLinePM = new APInvoiceLinePM
            {

                Tenant = UserTenant.Tenant,
            };
            return this;
        }
    }
}
