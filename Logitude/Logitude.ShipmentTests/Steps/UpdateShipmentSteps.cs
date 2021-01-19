using FluentAssertions;
using Logitude.ShipmentTests.Constants;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class UpdateShipmentSteps
    {
        private readonly ShipmentContext ShipmentContext;
        private PackagePM _housePackage, _masterPackages;
        private PayablesPM _PayablesPM;

        public UpdateShipmentSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        [Given(@"The master shipment packages fields")]
        public void GivenTheMasterShipmentPackagesFields(Table table)
        {
            _masterPackages = CreatePackageInstance(table);
        }

        [When(@"The put API sent to add master packages")]
        public void TheputAPIsenttoaddmasterpackages()
        {
            ShipmentContext.MasterShipment = AddPackagesToShipment(ShipmentContext.MasterShipment, _masterPackages);

            var response = APICaller.CallPut<ShipmentPM>(ShipmentContext.MasterShipment, URLs.Shipment, User.Token);
            ShipmentContext.MasterShipment.Id = response.Data.Id;
        }

        [Then(@"A new master packages added successfully")]
        public void ThenANewMasterPackagesAddedSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }

        [Given(@"The house shipment packages fields")]
        public void GivenTheHouseShipmentPackagesFields(Table table)
        {
            _housePackage = CreatePackageInstance(table);
        }

        [When(@"The put API sent to add house packages")]
        public void TheputAPIsenttoaddhousepackages()
        {
            ShipmentContext.MasterShipment = AddPackagesToShipment(ShipmentContext.HouseShipment, _housePackage);
            var response = APICaller.CallPut<ShipmentPM>(ShipmentContext.HouseShipment, URLs.Shipment, User.Token);
            ShipmentContext.HouseShipment.Id = response.Data.Id;
        }

        [Then(@"A new house packages added successfully")]
        public void ThenANewHousePackagesAddedSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
        }

        [Given(@"The Payable Charge Type fields")]
        public void GivenThePayableChargeTypeFields(Table table)
        {
            _PayablesPM = CreatePayablesInstance(table);
        }

        [When(@"The put API sent to add master Payable")]
        public void WhenThePutAPISentToAddMasterPayable()
        {
            ShipmentContext.MasterShipment = AddPayablesToShipment(ShipmentContext.MasterShipment, _PayablesPM);
            var response = APICaller.CallPut<ShipmentPM>(ShipmentContext.MasterShipment, URLs.Shipment, User.Token);
            ShipmentContext.MasterShipment.Id = response.Data?.Id;
        }

        [Then(@"The payable cherge type added successfully")]
        public void ThenThePayableChergeTypeAddedSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }

        private PackagePM CreatePackageInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new PackageBuilder().WithDefualtValues()
                .Quantity((int)dataTable.Quantity)
                .Length((double)dataTable.Length)
                .Width((double)dataTable.Width)
                .Height((double)dataTable.Height)
                .Weight((double)dataTable.Weight)
                .Build();
        }

        private PayablesPM CreatePayablesInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new PayableBuilder().WithDefualtValues()
                .ChargesTypeName((string)dataTable.ChargesTypeName)
                .ChargesTypeCode((string)dataTable.ChargesType)
                .ChargesTypeIdByCode((string)dataTable.ChargesType)
                .MeasurementCode((string)dataTable.Measurement)
                .MeasurementIdByCode((string)dataTable.Measurement)
                .UnitPrice((double)dataTable.UnitPrice)
                .CurrencyCode((string)dataTable.Currency)
                .CurrencyIdByCode((string)dataTable.Currency)
                .ShipmentPayableLineStatusCode((string)dataTable.ShipmentPayableLineStatus)
                .Build();
        }

        private ShipmentPM AddPackagesToShipment(ShipmentPM shipment, PackagePM package)
        {
            package = new PackageBuilder().WithModel(package)
                .ShipmentNumber(shipment.MasterShipmentNumber)
                .ShipmentId(shipment.Id)
                .Build();

            return new ShipmentBuilder().WithModel(shipment)
                .PackagesQuantity(package.Quantity)
                .ShipmentPackages(package)
                .Build();
        }

        private ShipmentPM AddPayablesToShipment(ShipmentPM shipment, PayablesPM payable)
        {
            payable = new PayableBuilder().WithModel(payable)
                .ShipmentId(shipment.Id)
                .Build();

            return new ShipmentBuilder().WithModel(shipment)
                .ShipmentPayables(payable)
                .Build();
        }
    }
}