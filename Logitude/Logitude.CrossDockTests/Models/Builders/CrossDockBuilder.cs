using Logitude.CrossDockTests.ExternalServices;
using Logitude.Test.Base.Models.Infrastructure;
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
        private CrossDockEntryPM _crossDockEntry;
        private readonly RandomGeneratorService randomGeneratorService;
        public CrossDockBuilder()
        {
            randomGeneratorService = new RandomGeneratorService();
            this.Reset();
        }

        private void Reset()
        {
            _crossDockEntry = new CrossDockEntryPM();
        }

        public CrossDockBuilder DirectionId(string directionId)
        {
            _crossDockEntry.DirectionId = directionId;
            return this;
        }

        public CrossDockBuilder TransportModeId(string transportModeId)
        {
            _crossDockEntry.TransportModeId = transportModeId;
            return this;
        }

        public CrossDockBuilder ChargeableWeightUnitCode(string chargeableWeightUnitCode)
        {
            _crossDockEntry.ChargeableWeightUnitCode = chargeableWeightUnitCode;
            return this;
        }
        public CrossDockBuilder GrossWeightUnitCode(string grossWeightUnitCode)
        {
            _crossDockEntry.GrossWeightUnitCode = grossWeightUnitCode;
            return this;
        }

        public CrossDockBuilder DimensionsUnitCode(string dimensionsUnitCode)
        {
            _crossDockEntry.DimensionsUnitCode = dimensionsUnitCode;
            return this;
        }

        public CrossDockBuilder VolumeUnitCode(string volumeUnitCode)
        {
            _crossDockEntry.VolumeUnitCode = volumeUnitCode;
            return this;
        }

        public CrossDockBuilder StatusCode(string statusCode)
        {
            _crossDockEntry.StatusCode = statusCode;
            return this;
        }

        public CrossDockBuilder WarehouseEntryPackages(List<WarehouseEntryPackagePM> warehouseEntryPackages)
        {
            _crossDockEntry.WarehouseEntryPackages = warehouseEntryPackages;
            return this;
        }
        public CrossDockBuilder WarehouseEntryPackage(WarehouseEntryPackagePM warehouseEntryPackage)
        {
            if (_crossDockEntry.WarehouseEntryPackages == null)
            {
                _crossDockEntry.WarehouseEntryPackages = new List<WarehouseEntryPackagePM>();
            }
            _crossDockEntry.WarehouseEntryPackages.Add(warehouseEntryPackage);
            return this;
        }

        public CrossDockEntryPM Build()
        {
            CrossDockEntryPM result = _crossDockEntry;
            this.Reset();
            return result;
        }

        public CrossDockBuilder WithModel(CrossDockEntryPM crossDockEntry)
        {
            _crossDockEntry = crossDockEntry;
            return this;
        }

        public CrossDockBuilder WithDefualtValues()
        {
            _crossDockEntry = new CrossDockEntryPM
            {
                Tenant = UserTenant.Tenant,
                CustomerId = PartnersData.CustomerId,
                ShipperId = PartnersData.ShippingAgentId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                WarehouseId = PartnersData.WarehouseId,
                EntryNumber = randomGeneratorService.RandomNumber(3).ToString()
            };
            return this;
        }
    }
}
