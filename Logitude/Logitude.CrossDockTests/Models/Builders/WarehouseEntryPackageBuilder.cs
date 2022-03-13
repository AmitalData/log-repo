using Logitude.CrossDockTests.Services;
using Logitude.Base.Models.Infrastructure;
using Logitude.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models.Builders
{
    public class WarehouseEntryPackageBuilder
    {
        private WarehouseEntryPackagePM _warehouseEntryPackage;
        public WarehouseEntryPackageBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _warehouseEntryPackage = new WarehouseEntryPackagePM();
        }

        public WarehouseEntryPackagePM Build()
        {
            WarehouseEntryPackagePM result = _warehouseEntryPackage;
            this.Reset();
            return result;
        }

        public WarehouseEntryPackageBuilder WithModel(WarehouseEntryPackagePM warehouseEntryPackage)
        {
            _warehouseEntryPackage = warehouseEntryPackage;
            return this;
        }

        public WarehouseEntryPackageBuilder Quantity(int quantity)
        {
            _warehouseEntryPackage.Quantity = quantity;
            return this;
        }

        public WarehouseEntryPackageBuilder Length(double? length)
        {
            _warehouseEntryPackage.Length = length;
            return this;
        }

        public WarehouseEntryPackageBuilder Width(double? width)
        {
            _warehouseEntryPackage.Width = width;
            return this;
        }

        public WarehouseEntryPackageBuilder Height(double? height)
        {
            _warehouseEntryPackage.Height = height;
            return this;
        }
        public WarehouseEntryPackageBuilder Weight(double? weight)
        {
            _warehouseEntryPackage.Weight = weight;
            return this;
        }


        public WarehouseEntryPackageBuilder WithDefualtValues()
        {
            _warehouseEntryPackage = new WarehouseEntryPackagePM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                ChangeSetOp = "Insert",
                WarehouseEntryId = RandomGeneratorService.RandomNumber(3).ToString()
            };
            return this;
        }

    }
}
