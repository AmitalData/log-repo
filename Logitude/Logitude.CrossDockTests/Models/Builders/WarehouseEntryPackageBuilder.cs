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
        private readonly string randomID;
        public WarehouseEntryPackageBuilder()
        {
            randomID = "123";
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

        public WarehouseEntryPackageBuilder Quantity(int Quantity)
        {
            _warehouseEntryPackagePM.Quantity = Quantity;
            return this;
        }

        public WarehouseEntryPackageBuilder Length(int? Length)
        {
            _warehouseEntryPackagePM.Length = Length;
            return this;
        }

        public WarehouseEntryPackageBuilder Width(int? Width)
        {
            _warehouseEntryPackagePM.Width = Width;
            return this;
        }

        public WarehouseEntryPackageBuilder Height(int? Height)
        {
            _warehouseEntryPackagePM.Height = Height;
            return this;
        }
        public WarehouseEntryPackageBuilder Weight(int? Weight)
        {
            _warehouseEntryPackagePM.Weight = Weight;
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
                WarehouseEntryId = randomID
            };
            return this;
        }

    }
}
