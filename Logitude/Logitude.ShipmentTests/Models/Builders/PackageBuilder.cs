using Logitude.Test.Base.Models.PackageTypesPreparation;
using Logitude.Test.Base.Models.UserTenantPreparation;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Models.Builders
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

        public PackageBuilder Height(double? height)
        {
            _packagePM.Height = height;
            return this;
        }

        public PackageBuilder Length(double? length)
        {
            _packagePM.Length = length;
            return this;
        }


        public PackageBuilder Weight(double? weight)
        {
            _packagePM.Weight = weight;
            return this;
        }

        public PackageBuilder Width(double? width)
        {
            _packagePM.Width = width;
            return this;
        }

        public PackageBuilder Quantity(int? quantity)
        {
            _packagePM.Quantity = quantity;
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
        public PackageBuilder TemperatureUnitCode(string TemperatureUnitCode)
        {
            _packagePM.TemperatureUnitCode = TemperatureUnitCode;
            return this;
        }
        public PackageBuilder PackageTypeByCode(string code)
        {
            _packagePM.PackageTypeId = PackageTypeCodeMapping(code);
            return this;
        }
        public PackageBuilder ShipmentId(string shipmentId)
        {
            _packagePM.ShipmentId = shipmentId;
            return this;
        }

        public PackageBuilder ShipmentNumber(string shipmentNumber)
        {
            _packagePM.ShipmentNumber = shipmentNumber;
            return this;
        }

        public PackageBuilder ChangeSetOp(int changeSetOp)
        {
            _packagePM.ChangeSetOp = changeSetOp;
            return this;
        }

        public PackagePM Build()
        {
            PackagePM result = _packagePM;

            this.Reset();

            return result;
        }

        public PackageBuilder WithModel(PackagePM packagePM)
        {
            _packagePM = packagePM;
            return this;
        }

        public PackageBuilder WithDefualtValues()
        {
            _packagePM = new PackagePM
            {
                Tenant = UserTenant.Tenant,
            };
            return this;
        }

        public PackageBuilder FromDataTable(Table dataTable)
        {
            _packagePM = dataTable.CreateInstance<PackagePM>();
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
