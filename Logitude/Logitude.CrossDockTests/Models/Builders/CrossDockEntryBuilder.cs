using Logitude.CrossDockTests.Services;
using Logitude.Base.Models.Infrastructure;
using Logitude.Base.Models.PartnersPreparation;
using Logitude.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models.Builders
{
    public class CrossDockEntryBuilder
    {
        private CrossDockEntryPM _crossDockEntry;
        public CrossDockEntryBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _crossDockEntry = new CrossDockEntryPM();
        }

        public CrossDockEntryBuilder DirectionId(string directionId)
        {
            _crossDockEntry.DirectionId = directionId;
            return this;
        }

        public CrossDockEntryBuilder TransportModeId(string transportModeId)
        {
            _crossDockEntry.TransportModeId = transportModeId;
            return this;
        }

        public CrossDockEntryBuilder ChargeableWeightUnitCode(string chargeableWeightUnitCode)
        {
            _crossDockEntry.ChargeableWeightUnitCode = chargeableWeightUnitCode;
            return this;
        }
        public CrossDockEntryBuilder GrossWeightUnitCode(string grossWeightUnitCode)
        {
            _crossDockEntry.GrossWeightUnitCode = grossWeightUnitCode;
            return this;
        }

        public CrossDockEntryBuilder DimensionsUnitCode(string dimensionsUnitCode)
        {
            _crossDockEntry.DimensionsUnitCode = dimensionsUnitCode;
            return this;
        }

        public CrossDockEntryBuilder VolumeUnitCode(string volumeUnitCode)
        {
            _crossDockEntry.VolumeUnitCode = volumeUnitCode;
            return this;
        }

        public CrossDockEntryBuilder StatusCode(string statusCode)
        {
            _crossDockEntry.StatusCode = statusCode;
            return this;
        }

        public CrossDockEntryBuilder WarehouseEntryPackages(List<WarehouseEntryPackagePM> warehouseEntryPackages)
        {
            _crossDockEntry.WarehouseEntryPackages = warehouseEntryPackages;
            return this;
        }
        public CrossDockEntryBuilder WarehouseEntryPackage(WarehouseEntryPackagePM warehouseEntryPackage)
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

        public CrossDockEntryBuilder WithModel(CrossDockEntryPM crossDockEntry)
        {
            _crossDockEntry = crossDockEntry;
            return this;
        }

        public CrossDockEntryBuilder WithDefualtValues()
        {
            _crossDockEntry = new CrossDockEntryPM
            {
                Tenant = UserTenant.Tenant,
                CustomerId = PartnersData.CustomerId,
                ShipperId = PartnersData.ShippingAgentId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                WarehouseId = PartnersData.WarehouseId,
                EntryNumber = RandomGeneratorService.RandomNumber(3).ToString()
            };
            return this;
        }
    }
}
