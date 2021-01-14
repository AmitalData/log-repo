using Logitude.SpecFlow.Models.ChargeType;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SpecFlow.Builders.ChargeType
{
    public class ChargeTypeBuilder
    {
        private ChargeTypePM _chargeType;

        public ChargeTypeBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _chargeType = new ChargeTypePM();
        }

        public ChargeTypeBuilder Id(string id)
        {
            _chargeType.Id = id;
            return this;
        }

        public ChargeTypeBuilder Code(string code)
        {
            _chargeType.Code = code;
            return this;
        }

        public ChargeTypeBuilder EnglishName(string englishName)
        {
            _chargeType.EnglishName = englishName;
            return this;
        }

        public ChargeTypeBuilder ChargesGroupCode(string chargesGroupCode)
        {
            _chargeType.ChargesGroupCode = chargesGroupCode;
            return this;
        }

        public ChargeTypeBuilder MeasurementId(string measurementId)
        {
            _chargeType.MeasurementId = measurementId;
            return this;
        }

        public ChargeTypeBuilder ChargesGroupId(string chargesGroupId)
        {
            _chargeType.ChargesGroupId = chargesGroupId;
            return this;
        }

        public ChargeTypeBuilder Tenant(int tenant)
        {
            _chargeType.Tenant = tenant;
            return this;
        }

        public ChargeTypePM Build()
        {
            ChargeTypePM result = this._chargeType;

            this.Reset();

            return result;
        }

        public ChargeTypeBuilder WithDefualtValues()
        {
            _chargeType = new ChargeTypePM
            {
                EnglishName = "Test Charge Type",
                Tenant = 0
            };
        return this;
        }

        public ChargeTypeBuilder FromDataTable(Table chargeTypeData)
        {
            _chargeType = chargeTypeData.CreateInstance<ChargeTypePM>();
            return this;
        }
    }
}
