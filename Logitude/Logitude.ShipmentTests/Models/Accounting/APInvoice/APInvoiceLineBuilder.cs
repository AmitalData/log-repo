using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;

namespace Logitude.ShipmentTests.Models.Accounting.APInvoice
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
            _APInvoiceLinePM.ChargesTypeId = ChargesTypeId == "AFT" ? BillingData.ChargeTypeAFTId : null;
            return this;
        }
        public APInvoiceLineBuilder ChargesTypeName(string ChargesTypeName)
        {
            _APInvoiceLinePM.ChargesTypeName = ChargesTypeName;
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
        public APInvoiceLineBuilder LocalCurrencyAmount(double LocalCurrencyAmount)
        {
            _APInvoiceLinePM.LocalCurrencyAmount = LocalCurrencyAmount;
            return this;
        }
        public APInvoiceLineBuilder ProfitCurrencyAmount(double ProfitCurrencyAmount)
        {
            _APInvoiceLinePM.ProfitCurrencyAmount = ProfitCurrencyAmount;
            return this;
        }

        public APInvoiceLineBuilder VatTypeId(string VatTypeId)
        {
            _APInvoiceLinePM.VatTypeId = VatTypeId == "Zero" ? BillingData.VATTypeZeroId : null;
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
