using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models.Builders
{
    public class CrossDockBuilder
    {
        private CrossDockPM _crossDockPM;
        private readonly string randomID;
        public CrossDockBuilder()
        {
            randomID = "123";
            this.Reset();
        }

        private void Reset()
        {
            _crossDockPM = new CrossDockPM();
        }

        public CrossDockBuilder DirectionId(string DirectionId)
        {
            _crossDockPM.DirectionId = DirectionId;
            return this;
        }

        public CrossDockBuilder TransportModeId(string TransportModeId)
        {
            _crossDockPM.TransportModeId = TransportModeId;
            return this;
        }

        public CrossDockBuilder ChargeableWeightUnitCode(string ChargeableWeightUnitCode)
        {
            _crossDockPM.ChargeableWeightUnitCode = ChargeableWeightUnitCode;
            return this;
        }
        public CrossDockBuilder GrossWeightUnitCode(string GrossWeightUnitCode)
        {
            _crossDockPM.GrossWeightUnitCode = GrossWeightUnitCode;
            return this;
        }

        public CrossDockBuilder DimensionsUnitCode(string DimensionsUnitCode)
        {
            _crossDockPM.DimensionsUnitCode = DimensionsUnitCode;
            return this;
        }

        public CrossDockBuilder VolumeUnitCode(string VolumeUnitCode)
        {
            _crossDockPM.VolumeUnitCode = VolumeUnitCode;
            return this;
        }

        public CrossDockBuilder StatusCode(string StatusCode)
        {
            _crossDockPM.StatusCode = StatusCode;
            return this;
        }

        public CrossDockPM Build()
        {
            CrossDockPM result = _crossDockPM;
            this.Reset();
            return result;
        }

        public CrossDockBuilder WithModel(CrossDockPM quotePM)
        {
            _crossDockPM = quotePM;
            return this;
        }

        public CrossDockBuilder WithDefualtValues()
        {
            _crossDockPM = new CrossDockPM
            {
                Tenant = UserTenant.Tenant,
                CustomerId = PartnersData.CustomerId,
                ShipperId = PartnersData.ShippingAgentId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                WarehouseId = PartnersData.WarehouseId,
                EntryNumber = randomID
            };
            return this;
        }
    }
}
