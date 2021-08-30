using Logitude.Test.Base.Models.LocationsPreparation;
using Logitude.Test.Base.Models.PackageTypesPreparation;
using Logitude.Test.Base.Models.PartnersPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.OceanTest.Models.Builders
{
    public class PackageBuilder
    {
        private PackagePM _packagePM;

        public PackageBuilder()
        {
            this.Reset();
        }

        private void Reset()
        {
            _packagePM = new PackagePM();
        }

        public PackageBuilder Id(string id)
        {
            _packagePM.Id = id;
            return this;
        }

        public PackageBuilder Tenant(int tenant)
        {
            _packagePM.Tenant = tenant;
            return this;
        }

        public PackageBuilder ShipmentNumber(string shipmentNumber)
        {
            _packagePM.ShipmentNumber = shipmentNumber;
            return this;
        }

        public PackagePM Build()
        {
            PackagePM result = _packagePM;

            this.Reset();

            return result;
        }

        public PackageBuilder WithModel(PackagePM PackagePM)
        {
            _packagePM = PackagePM;
            return this;
        }

        public PackageBuilder WithDefualtValues()
        {
            _packagePM = new PackagePM
            {
                Tenant = UserTenant.Tenant
            };
            return this;
        }

        public PackageBuilder FromDataTable(Table dataTable)
        {
            _packagePM = dataTable.CreateInstance<PackagePM>();
            return this;
        }

        public PackageBuilder ChangeSetOp(string changeSetOp)
        {
            _packagePM.ChangeSetOp = changeSetOp;
            return this;
        }

        public PackageBuilder PackageTypeByCode(string code)
        {
            _packagePM.PackageTypeId = PackageTypeCodeMapping(code);
            return this;
        }

        public PackageBuilder ContainerNumber(string containerNumber)
        {
            _packagePM.ContainerNumber = containerNumber;
            return this;
        }

        public PackageBuilder FlashPointTemperatureUnitCode(string flashPointTemperatureUnitCode)
        {
            _packagePM.FlashPointTemperatureUnitCode = flashPointTemperatureUnitCode;
            return this;
        }

        public PackageBuilder PackageTypeId(string PackageTypeId)
        {
            _packagePM.PackageTypeId = PackageTypeId;
            return this;
        }

        public PackageBuilder TemperatureUnitCode(string TemperatureUnitCode)
        {
            _packagePM.TemperatureUnitCode = TemperatureUnitCode;
            return this;
        }

        public PackageBuilder Quantity(int? Quantity)
        {
            _packagePM.Quantity = Quantity;
            return this;
        }

        private string PackageTypeCodeMapping(string portCode)
        {
            switch (portCode)
            {
                case "PC2":
                    return PackageTypesData.PackageTypeOceanPC2Id;
                    case "PC1":
                    return PackageTypesData.PackageTypeOceanPC1Id;
                    case "PP1":
                    return PackageTypesData.PackageTypeAirPP1Id;
                    case "PP2":
                    return PackageTypesData.PackageTypeAirPP2Id;
                default:
                    return null;
            }
        }


    }
}
