using Logitude.CrossDockTests.ExternalServices;
using Logitude.Test.Base.Models.Infrastructure;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CrossDockTests.Models.Builders
{
    public class WarehouseEntryPackageBuilder
    {
        private WarehouseEntryPackagePM _warehouseEntryPackagePM;
        private readonly RandomGeneratorService randomGeneratorService;
        public WarehouseEntryPackageBuilder()
        {
            randomGeneratorService = new RandomGeneratorService();
            this.Reset();
        }

        private void Reset()
        {
            _warehouseEntryPackagePM = new WarehouseEntryPackagePM();
        }

        public WarehouseEntryPackagePM Build()
        {
            WarehouseEntryPackagePM result = _warehouseEntryPackagePM;
            this.Reset();
            return result;
        }

        public WarehouseEntryPackageBuilder WithModel(WarehouseEntryPackagePM quotePM)
        {
            _warehouseEntryPackagePM = quotePM;
            return this;
        }

        public WarehouseEntryPackageBuilder Quantity(int quantity)
        {
            _warehouseEntryPackagePM.Quantity = quantity;
            return this;
        }

        public WarehouseEntryPackageBuilder Length(double? length)
        {
            _warehouseEntryPackagePM.Length = length;
            return this;
        }

        public WarehouseEntryPackageBuilder Width(double? width)
        {
            _warehouseEntryPackagePM.Width = width;
            return this;
        }

        public WarehouseEntryPackageBuilder Height(double? height)
        {
            _warehouseEntryPackagePM.Height = height;
            return this;
        }
        public WarehouseEntryPackageBuilder Weight(double? weight)
        {
            _warehouseEntryPackagePM.Weight = weight;
            return this;
        }


        public WarehouseEntryPackageBuilder WithDefualtValues()
        {
            _warehouseEntryPackagePM = new WarehouseEntryPackagePM
            {
                Tenant = UserTenant.Tenant,
                CreatedByUserId = UserTenant.UserId,
                UpdatedByUserId = UserTenant.UserId,
                ChangeSetOp = "Insert",
                WarehouseEntryId = randomGeneratorService.RandomNumber(3).ToString()
            };
            return this;
        }

    }
}
