using Logitude.CrossDockTests.ExternalServices;
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
        private readonly RandomGeneratorExternalService randomGeneratorExternalService;
        public CrossDockBuilder()
        {
            randomGeneratorExternalService = new RandomGeneratorExternalService();
            this.Reset();
        }

        private void Reset()
        {
            _crossDockPM = new CrossDockPM();
        }

        public CrossDockBuilder DirectionId(string directionId)
        {
            _crossDockPM.DirectionId = directionId;
            return this;
        }

        public CrossDockBuilder TransportModeId(string transportModeId)
        {
            _crossDockPM.TransportModeId = transportModeId;
            return this;
        }

        public CrossDockBuilder ChargeableWeightUnitCode(string chargeableWeightUnitCode)
        {
            _crossDockPM.ChargeableWeightUnitCode = chargeableWeightUnitCode;
            return this;
        }
        public CrossDockBuilder GrossWeightUnitCode(string grossWeightUnitCode)
        {
            _crossDockPM.GrossWeightUnitCode = grossWeightUnitCode;
            return this;
        }

        public CrossDockBuilder DimensionsUnitCode(string dimensionsUnitCode)
        {
            _crossDockPM.DimensionsUnitCode = dimensionsUnitCode;
            return this;
        }

        public CrossDockBuilder VolumeUnitCode(string volumeUnitCode)
        {
            _crossDockPM.VolumeUnitCode = volumeUnitCode;
            return this;
        }

        public CrossDockBuilder StatusCode(string statusCode)
        {
            _crossDockPM.StatusCode = statusCode;
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
                EntryNumber = randomGeneratorExternalService.RandomNumber(3).ToString()
            };
            return this;
        }
    }
}
