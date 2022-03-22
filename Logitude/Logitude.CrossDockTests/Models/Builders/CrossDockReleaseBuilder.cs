using Logitude.Base.Models.Infrastructure;
using Logitude.Base.Models.Partners;
using Logitude.Base.Models.UserTenant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models.Builders
{
    public class CrossDockReleaseBuilder
    {
        private CrossDockReleasePM _crossDockRelease;
        public CrossDockReleaseBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _crossDockRelease = new CrossDockReleasePM();
        }

        public CrossDockReleaseBuilder ChargeableWeightUnitCode(string chargeableWeightUnitCode)
        {
            _crossDockRelease.ChargeableWeightUnitCode = chargeableWeightUnitCode;
            return this;
        }
        public CrossDockReleaseBuilder GrossWeightUnitCode(string grossWeightUnitCode)
        {
            _crossDockRelease.GrossWeightUnitCode = grossWeightUnitCode;
            return this;
        }

        public CrossDockReleaseBuilder DimensionsUnitCode(string dimensionsUnitCode)
        {
            _crossDockRelease.DimensionsUnitCode = dimensionsUnitCode;
            return this;
        }

        public CrossDockReleaseBuilder VolumeUnitCode(string volumeUnitCode)
        {
            _crossDockRelease.VolumeUnitCode = volumeUnitCode;
            return this;
        }

        public CrossDockReleaseBuilder StatusCode(string statusCode)
        {
            _crossDockRelease.StatusCode = statusCode;
            return this;
        }

        public CrossDockReleaseBuilder CustomerRef1(string customerRef1)
        {
            _crossDockRelease.CustomerRef1 = customerRef1;
            return this;
        }


        public CrossDockReleaseBuilder HouseNumber(string houseNumber)
        {
            _crossDockRelease.HouseNumber = houseNumber;
            return this;
        }


        public CrossDockReleaseBuilder WarehouseReleasePackages(List<WarehouseReleasePackagePM> warehouseReleasePackages)
        {
            _crossDockRelease.WarehouseReleasePackages = warehouseReleasePackages;
            return this;
        }
        public CrossDockReleaseBuilder WarehouseReleasePackage(WarehouseReleasePackagePM warehouseReleasePackage)
        {
            if (_crossDockRelease.WarehouseReleasePackages == null)
            {
                _crossDockRelease.WarehouseReleasePackages = new List<WarehouseReleasePackagePM>();
            }
            _crossDockRelease.WarehouseReleasePackages.Add(warehouseReleasePackage);
            return this;
        }

        public CrossDockReleasePM Build()
        {
            CrossDockReleasePM result = _crossDockRelease;
            this.Reset();
            return result;
        }

        public CrossDockReleaseBuilder WithModel(CrossDockReleasePM crossDockEntry)
        {
            _crossDockRelease = crossDockEntry;
            return this;
        }

        public CrossDockReleaseBuilder WithDefualtValues()
        {
            _crossDockRelease = new CrossDockReleasePM
            {
                Tenant = UserTenant.Tenant,
                CustomerId = PartnersData.CustomerId,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                WarehouseId = PartnersData.WarehouseId,
                ReleaseNumber = RandomGeneratorService.RandomNumber(3).ToString()
            };
            return this;
        }
    }
}
