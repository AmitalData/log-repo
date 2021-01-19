using Logitude.Test.Base.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Models.Builders
{
    public class PayableBuilder
    {
        private PayablesPM _payablesPM;

        public PayableBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _payablesPM = new PayablesPM();
        }

        public PayableBuilder Id(string id)
        {
            _payablesPM.Id = id;
            return this;
        }

        public PayableBuilder Tenant(int tenant)
        {
            _payablesPM.Tenant = tenant;
            return this;
        }

        public PayableBuilder ShipmentId(string shipmentId)
        {
            _payablesPM.ShipmentId = shipmentId;
            return this;
        }

        public PayableBuilder ChargesTypeId(string chargesTypeId)
        {
            _payablesPM.ChargesTypeId = chargesTypeId;
            return this;
        }

        public PayableBuilder ChargesTypeIdByCode(string chargesTypeCode)
        {
            _payablesPM.ChargesTypeId = chargesTypeCode == "AFT" ? ShipmentData.ChargeTypeAFTId : null;
            return this;
        }

        public PayableBuilder ChargesTypeCode(string chargesTypeCode)
        {
            _payablesPM.ChargesTypeCode = chargesTypeCode;
            return this;
        }

        public PayableBuilder ChargesTypeName(string chargesTypeName)
        {
            _payablesPM.ChargesTypeName = chargesTypeName;
            return this;
        }

        public PayableBuilder ShipmentPayableLineStatusCode(string shipmentPayableLineStatusCode)
        {
            _payablesPM.ShipmentPayableLineStatusCode = shipmentPayableLineStatusCode;
            return this;
        }

        public PayableBuilder UnitPrice(double? unitPrice)
        {
            _payablesPM.UnitPrice = unitPrice;
            return this;
        }

        public PayableBuilder CurrencyId(string currencyId)
        {
            _payablesPM.CurrencyId = currencyId;
            return this;
        }

        public PayableBuilder CurrencyIdByCode(string currencyCode)
        {
            _payablesPM.CurrencyId = currencyCode == "EUR" ? ShipmentData.CurrencyEURId : null;
            return this;
        }

        public PayableBuilder CurrencyCode(string currencyCode)
        {
            _payablesPM.CurrencyCode = currencyCode;
            return this;
        }

        public PayableBuilder MeasurementId(string measurementId)
        {
            _payablesPM.MeasurementId = measurementId;
            return this;
        }

        public PayableBuilder MeasurementIdByCode(string measurementCode)
        {
            _payablesPM.MeasurementId = measurementCode == "GRWT" ? ShipmentData.MeasurmentGRWTId : null;
            return this;
        }

        public PayableBuilder MeasurementCode(string measurementCode)
        {
            _payablesPM.MeasurementCode = measurementCode;
            return this;
        }

        public PayablesPM Build()
        {
            PayablesPM result = _payablesPM;

            this.Reset();

            return result;
        }

        public PayableBuilder WithModel(PayablesPM payablesPM)
        {
            _payablesPM = payablesPM;
            return this;
        }

        public PayableBuilder WithDefualtValues()
        {
            _payablesPM = new PayablesPM
            {
                Tenant = UserTenant.Tenant,
            };
            return this;
        }

        public PayableBuilder FromDataTable(Table dataTable)
        {
            _payablesPM = dataTable.CreateInstance<PayablesPM>();
            return this;
        }
    }
}
