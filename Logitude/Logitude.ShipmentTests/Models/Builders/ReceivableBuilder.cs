using Logitude.ShipmentTests.Models.Accounting;
using Logitude.Test.Base.Models.BillingsPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Simplog.Data.Helpers;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Models.Builders
{
    public class ReceivableBuilder
    {
        private ShipmentReceivablePM _receivablePM;

        public ReceivableBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _receivablePM = new ShipmentReceivablePM();
        }

        public ReceivableBuilder Id(string id)
        {
            _receivablePM.Id = id;
            return this;
        }

        public ReceivableBuilder Tenant(int tenant)
        {
            _receivablePM.Tenant = tenant;
            return this;
        }

        public ReceivableBuilder ShipmentId(string shipmentId)
        {
            _receivablePM.ShipmentId = shipmentId;
            return this;
        }
        public ReceivableBuilder ShipmentNumber(string ShipmentNumber)
        {
            _receivablePM.ShipmentNumber = ShipmentNumber;
            return this;
        }

        public ReceivableBuilder ChargesTypeId(string chargesTypeId)
        {
            _receivablePM.ChargesTypeId = chargesTypeId;
            return this;
        }

        public ReceivableBuilder ChargesTypeIdByCode(string chargesTypeCode)
        {
            _receivablePM.ChargesTypeId = chargesTypeCode == "AFT" ? BillingData.ChargeTypeAFTId : null;
            return this;
        }

        public ReceivableBuilder ChargesTypeCode(string chargesTypeCode)
        {
            _receivablePM.ChargesTypeCode = chargesTypeCode;
            return this;
        }

        public ReceivableBuilder ChargesTypeName(string chargesTypeName)
        {
            _receivablePM.ChargesTypeName = chargesTypeName;
            return this;
        }

        public ReceivableBuilder ShipmentReceivableLineStatusCode(string shipmentReceivableLineStatusCode)
        {
            _receivablePM.ShipmentReceivableLineStatusCode = shipmentReceivableLineStatusCode;
            return this;
        }

        public ReceivableBuilder UnitPrice(double? unitPrice)
        {
            _receivablePM.UnitPrice = unitPrice;
            return this;
        }

        public ReceivableBuilder Quantity(double? Quantity)
        {
            _receivablePM.Quantity = Quantity;
            return this;
        }

        public ReceivableBuilder Rate(double? Rate)
        {
            _receivablePM.Rate = Rate;
            return this;
        }

        public ReceivableBuilder TotalAmount(double? TotalAmount)
        {
            _receivablePM.TotalAmount = TotalAmount;
            return this;
        }

        public ReceivableBuilder TotalAmountLocal(double? TotalAmountLocal)
        {
            _receivablePM.TotalAmountLocal = TotalAmountLocal;
            return this;
        }

        public ReceivableBuilder CurrencyId(string currencyId)
        {
            _receivablePM.CurrencyId = currencyId;
            return this;
        }

        public ReceivableBuilder CurrencyIdByCode(string currencyCode)
        {
            _receivablePM.CurrencyId = currencyCode == "EUR" ? BillingData.CurrencyEURId : null;
            return this;
        }

        public ReceivableBuilder CurrencyCode(string currencyCode)
        {
            _receivablePM.CurrencyCode = currencyCode;
            return this;
        }

        public ReceivableBuilder MeasurementId(string measurementId)
        {
            _receivablePM.MeasurementId = measurementId == "GRWT" ? BillingData.MeasurementGRWTId : null;
            return this;
        }

        public ReceivableBuilder MeasurementIdByCode(string measurementCode)
        {
            _receivablePM.MeasurementId = measurementCode == "GRWT" ? BillingData.MeasurementGRWTId : null;
            return this;
        }

        public ReceivableBuilder MeasurementCode(string measurementCode)
        {
            _receivablePM.MeasurementCode = measurementCode;
            return this;
        }
        public ReceivableBuilder ChangeSetOp(ChangeSetOperation ChangeSetOp)
        {
            _receivablePM.ChangeSetOp = ChangeSetOp;
            return this;
        }

        public ShipmentReceivablePM Build()
        {
            ShipmentReceivablePM result = _receivablePM;

            this.Reset();

            return result;
        }

        public ReceivableBuilder WithModel(ShipmentReceivablePM ReceivablePM)
        {
            _receivablePM = ReceivablePM;
            return this;
        }

        public ReceivableBuilder WithDefualtValues()
        {
            _receivablePM = new ShipmentReceivablePM
            {
                Tenant = UserTenant.Tenant,
                CreateDate = TenantServerConfigration.GetCurrentDateTime(UserTenant.Tenant),
                UpdateDate = TenantServerConfigration.GetCurrentDateTime(UserTenant.Tenant),
                CreatedByUserId = UserTenant.UserId,
                DueTypeCode = "CA",
                IATACodeId="1-2",
                PrepaidCollectId = "P",
                UpdateByUserId = UserTenant.UserId,
                VatAmountLocal=380,
                VatAmountProfit=380,
                VatTypeId="1-3810"
            };
            return this;
        }

        public ReceivableBuilder FromDataTable(Table dataTable)
        {
            _receivablePM = dataTable.CreateInstance<ShipmentReceivablePM>();
            return this;
        }
    }
}
