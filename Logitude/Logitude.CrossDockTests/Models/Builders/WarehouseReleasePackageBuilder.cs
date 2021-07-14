using Logitude.Test.Base.Models.Infrastructure;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models.Builders
{
    public class WarehouseReleasePackageBuilder
    {
        private WarehouseReleasePackagePM _warehouseReleasePackage;
        public WarehouseReleasePackageBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _warehouseReleasePackage = new WarehouseReleasePackagePM();
        }

        public WarehouseReleasePackagePM Build()
        {
            WarehouseReleasePackagePM result = _warehouseReleasePackage;
            this.Reset();
            return result;
        }

        public WarehouseReleasePackageBuilder WithModel(WarehouseReleasePackagePM warehouseEntryPackage)
        {
            _warehouseReleasePackage = warehouseEntryPackage;
            return this;
        }

        public WarehouseReleasePackageBuilder Quantity(int quantity)
        {
            _warehouseReleasePackage.Quantity = quantity;
            return this;
        }

        public WarehouseReleasePackageBuilder Weight(double? weight)
        {
            _warehouseReleasePackage.Weight = weight;
            return this;
        }

        public WarehouseReleasePackageBuilder EntryPackageId(string entryPackageId)
        {
            _warehouseReleasePackage.EntryPackageId = entryPackageId;
            return this;
        }

        public WarehouseReleasePackageBuilder Length(double? length)
        {
            _warehouseReleasePackage.Length = length;
            return this;
        }

        public WarehouseReleasePackageBuilder Width(double? width)
        {
            _warehouseReleasePackage.Width = width;
            return this;
        }

        public WarehouseReleasePackageBuilder Height(double? height)
        {
            _warehouseReleasePackage.Height = height;
            return this;
        }
 
        public WarehouseReleasePackageBuilder WithDefualtValues()
        {
            _warehouseReleasePackage = new WarehouseReleasePackagePM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                ChangeSetOp = "Insert",
                WarehouseReleaseId = RandomGeneratorService.RandomNumber(3).ToString()
            };
            return this;
        }
    }
}
