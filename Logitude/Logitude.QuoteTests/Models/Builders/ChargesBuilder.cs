using Logitude.Base.Models.BillingsPreparation;
using Logitude.Base.Models.UserTenantPreparation;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.QuoteTests.Models.Builders
{
    class ChargesBuilder
    {
        private QuoteChargePM _QuoteCharge;

        public ChargesBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _QuoteCharge = new QuoteChargePM();
        }

        public ChargesBuilder Id(string id)
        {
            _QuoteCharge.Id = id;
            return this;
        }

        public ChargesBuilder Tenant(int tenant)
        {
            _QuoteCharge.Tenant = tenant;
            return this;
        }

        public ChargesBuilder ChargesTypeId(string chargesTypeId)
        {
            _QuoteCharge.ChargesTypeId = chargesTypeId == "AFT" ? BillingData.ChargeTypeAFTId : null;
            return this;
        }

        public ChargesBuilder ChargesTypeCode(string chargesTypeCode)
        {
            _QuoteCharge.ChargesTypeId = chargesTypeCode ;
            return this;
        }

        public ChargesBuilder ChargesTypeName(string ChargesTypeName)
        {
            _QuoteCharge.ChargesTypeName = ChargesTypeName;
            return this;
        }

        public ChargesBuilder QuoteId(string QuoteId)
        {
            _QuoteCharge.QuoteId = QuoteId;
            return this;
        }

        public ChargesBuilder CostCurrencyId(string CostCurrencyId)
        {
            _QuoteCharge.CostCurrencyId = CostCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }

        public ChargesBuilder SaleCurrencyId(string SaleCurrencyId)
        {
            _QuoteCharge.SaleCurrencyId = SaleCurrencyId == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }

        public ChargesBuilder CostExchangeRate(double CostExchangeRate)
        {
            _QuoteCharge.CostExchangeRate = CostExchangeRate;
            return this;
        }
        public ChargesBuilder SaleExchangeRate(double SaleExchangeRate)
        {
            _QuoteCharge.SaleExchangeRate = SaleExchangeRate;
            return this;
        }

        public ChargesBuilder CostMeasurementId(string CostMeasurementId)
        {
            _QuoteCharge.CostMeasurementId = CostMeasurementId == "GRWT" ? BillingData.MeasurementGRWTId : null;
            return this;
        }

        public ChargesBuilder SaleMeasurementId(string SaleMeasurementId)
        {
            _QuoteCharge.SaleMeasurementId = SaleMeasurementId == "GRWT" ? BillingData.MeasurementGRWTId : null;
            return this;
        }

        public QuoteChargePM Build()
        {
            QuoteChargePM result = _QuoteCharge;

            this.Reset();

            return result;
        }

        public ChargesBuilder WithModel(QuoteChargePM quotePM)
        {
            _QuoteCharge = quotePM;
            return this;
        }

        public ChargesBuilder WithDefualtValues()
        {
            _QuoteCharge = new QuoteChargePM
            {
                Tenant = UserTenant.Tenant,
            };
            return this;
        }

        public ChargesBuilder FromDataTable(Table dataTable)
        {
            _QuoteCharge = dataTable.CreateInstance<QuoteChargePM>();
            return this;
        }
    }
}
