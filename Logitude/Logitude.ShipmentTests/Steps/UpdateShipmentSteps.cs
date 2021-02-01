using FluentAssertions;
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
        private PackagePM HousePackage, MasterPackages;
        private PayablesPM PayablesPM;

        public UpdateShipmentSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        #region Step Region

        #region Add a master package steps
        [Given(@"a master package with the following properties")]
        public void GivenAMasterPackageWithTheFollowingProperties(Table table)
        {
            MasterPackages = CreatePackageInstance(table);
        }

        [When(@"add a master package")]
        public void WhenAddAMasterPackage()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentByAddingPackages(ShipmentContext.MasterShipment, MasterPackages);
            ShipmentContext.MasterShipment.Id = response.Data?.Id;
        }

        [Then(@"the master should add package successfully")]
        public void ThenTheMasterShouldAddPackageSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }
        #endregion

        #region Add a house package steps
        [Given(@"a house package with the following properties")]
        public void GivenAHousePackageWithTheFollowingProperties(Table table)
        {
            HousePackage = CreatePackageInstance(table);
        }

        [When(@"add a house package")]
        public void WhenAddAHousePackage()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentByAddingPackages(ShipmentContext.HouseShipment, HousePackage);
            ShipmentContext.HouseShipment.Id = response.Data?.Id;
        }

        [Then(@"the house should add package successfully")]
        public void ThenTheHouseShouldAddPackageSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
        }
        #endregion

        #region Add a master payable steps
        [Given(@"a master payable with the following properties")]
        public void GivenAMasterPayableWithTheFollowingProperties(Table table)
        {
            PayablesPM = CreatePayablesInstance(table);
        }

        [When(@"add a master payable")]
        public void WhenAddAMasterPayable()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentByAddingPayables(ShipmentContext.MasterShipment, PayablesPM);
            ShipmentContext.MasterShipment.Id = response.Data?.Id;
        }

        [Then(@"the master should add payable successfully")]
        public void ThenTheMasterShouldAddPayableSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }
        #endregion

        #endregion

        #region Private Function Region
        private ApiResponse<ShipmentPM> UpdateShipmentByAddingPackages(ShipmentPM shipment, PackagePM package)
        {
            shipment = AddPackagesToShipment(shipment, package);
            return APICaller.CallPut<ShipmentPM>(shipment, Urls.ShipmentController, UserTenant.Token);
        }

        private ApiResponse<ShipmentPM> UpdateShipmentByAddingPayables(ShipmentPM shipment, PayablesPM payable)
        {
            shipment = AddPayablesToShipment(shipment, payable);
            return APICaller.CallPut<ShipmentPM>(shipment, Urls.ShipmentController, UserTenant.Token);
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
        #endregion

        #region Build Models Region
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
        #endregion
    }
}