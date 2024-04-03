using Logitude.Base.Models.Billings;
using Logitude.Base.Models.UserTenant;

namespace Logitude.ShipmentTests.Models.Accounting.ARInvoice
{
    public class ARInvoiceLineBuilder
    {
        private ARInvoiceLinePM _ARInvoiceLinePM;

        public ARInvoiceLineBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _ARInvoiceLinePM = new ARInvoiceLinePM();
        }

        public ARInvoiceLineBuilder Tenant(int Tenant)
        {
            _ARInvoiceLinePM.Tenant = Tenant;
            return this;
        }
        public ARInvoiceLineBuilder ChargesTypeCode(string ChargesTypeCode)
        {
            _ARInvoiceLinePM.ChargesTypeCode = ChargesTypeCode;
            return this;
        }
        public ARInvoiceLineBuilder ChargesTypeId(string ChargesTypeId)
        {
            _ARInvoiceLinePM.ChargesTypeId = ChargesTypeId == "AFT" ? BillingData.ChargeTypeAFTId : null;
            return this;
        }
        public ARInvoiceLineBuilder ChargesTypeName(string ChargesTypeName)
        {
            _ARInvoiceLinePM.ChargesTypeName = ChargesTypeName;
            return this;
        }
        public ARInvoiceLineBuilder Description(string Description)
        {
            _ARInvoiceLinePM.Description = Description;
            return this;
        }

        public ARInvoiceLineBuilder InvoiceCurrencyAmount(double InvoiceCurrencyAmount)
        {
            _ARInvoiceLinePM.InvoiceCurrencyAmount = InvoiceCurrencyAmount;
            return this;
        }
        public ARInvoiceLineBuilder ForiegnCurrencyAmount(double ForiegnCurrencyAmount)
        {
            _ARInvoiceLinePM.ForiegnCurrencyAmount = ForiegnCurrencyAmount;
            return this;
        }
        public ARInvoiceLineBuilder LocalCurrencyAmount(double LocalCurrencyAmount)
        {
            _ARInvoiceLinePM.LocalCurrencyAmount = LocalCurrencyAmount;
            return this;
        }
        public ARInvoiceLineBuilder ProfitCurrencyAmount(double ProfitCurrencyAmount)
        {
            _ARInvoiceLinePM.ProfitCurrencyAmount = ProfitCurrencyAmount;
            return this;
        }

        public ARInvoiceLineBuilder ForiegnCurrencyId(string ForiegnCurrencyId)
        {
            _ARInvoiceLinePM.ForiegnCurrencyId = ForiegnCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }

        public ARInvoiceLineBuilder ForiegnCurrencyCode(string ForiegnCurrencyCode)
        {
            _ARInvoiceLinePM.ForiegnCurrencyCode = ForiegnCurrencyCode;
            return this;
        }

        public ARInvoiceLineBuilder VatTypeId(string VatTypeId)
        {
            _ARInvoiceLinePM.VatTypeId = VatTypeId == "Zero" ? BillingData.VATTypeZeroId : null;
            return this;
        }
        public ARInvoiceLineBuilder VatTypeName(string VatTypeName)
        {
            _ARInvoiceLinePM.VatTypeName = VatTypeName;
            return this;
        }
        public ARInvoiceLineBuilder EntityId(string EntityId)
        {
            _ARInvoiceLinePM.EntityId = EntityId;
            return this;
        }
        public ARInvoiceLineBuilder VatPercentage(double VatPercentage)
        {
            _ARInvoiceLinePM.VatPercentage = VatPercentage;
            return this;
        }
        public ARInvoiceLineBuilder UnitPrice(double UnitPrice)
        {
            _ARInvoiceLinePM.UnitPrice = UnitPrice;
            return this;
        }
        public ARInvoiceLineBuilder Quantity(double Quantity)
        {
            _ARInvoiceLinePM.Quantity = Quantity;
            return this;
        }
        public ARInvoiceLineBuilder ForiegnExchangeRate(double ForiegnExchangeRate)
        {
            _ARInvoiceLinePM.ForiegnExchangeRate = ForiegnExchangeRate;
            return this;
        }
        public ARInvoiceLineBuilder ChangeSetOp(ChangeSetOperation ChangeSetOp)
        {
            _ARInvoiceLinePM.ChangeSetOp = ChangeSetOp;
            return this;
        }

        public ARInvoiceLinePM Build()
        {
            ARInvoiceLinePM result = _ARInvoiceLinePM;
            this.Reset();
            return result;
        }

        public ARInvoiceLineBuilder WithModel(ARInvoiceLinePM ARInvoiceLinePM)
        {
            _ARInvoiceLinePM = ARInvoiceLinePM;
            return this;
        }

        public ARInvoiceLineBuilder WithDefualtValues()
        {
            _ARInvoiceLinePM = new ARInvoiceLinePM
            {

                Tenant = UserTenant.Tenant,
            };
            return this;
        }
    }
}
