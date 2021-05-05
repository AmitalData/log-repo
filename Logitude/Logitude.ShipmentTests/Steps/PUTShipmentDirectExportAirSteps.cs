using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class PUTShipmentDirectExportAirSteps
    {
        protected readonly ShipmentContext ShipmentContext;
        private PackagePM directPackages;
        private PayablesPM PayablesPM;
        private ReceivablePM receivablePM;
        private ShipmentPM Partners;

        public PUTShipmentDirectExportAirSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        [Given(@"a direct package with the following properties")]
        public void GivenADirectPackageWithTheFollowingProperties(Table table)
        {
            directPackages = CreatePackageInstance(table);
        }

        [Given(@"a direct payable with the following properties")]
        public void GivenADirectPayableWithTheFollowingProperties(Table table)
        {
            PayablesPM = CreatePayablesInstance(table);
        }

        [Given(@"a direct receivable with the following properties")]
        public void GivenADirectReceivableWithTheFollowingProperties(Table table)
        {
            receivablePM = CreateReceivableInstance(table);
        }

        [Given(@"a partners with the following properties")]
        public void GivenAPartnersWithTheFollowingProperties(Table table)
        {
            Partners = AddAgentPartner(table);
        }

        [When(@"update a direct shipment")]
        public void WhenUpdateADirectShipment()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentByAddingPackages(ShipmentContext.DirectShipment, directPackages, PayablesPM, receivablePM);
            ShipmentContext.DirectShipment.Id = response.Data?.Id;
        }

        [Then(@"the direct should update successfully")]
        public void ThenTheDirectShouldUpdateSuccessfully()
        {
            ShipmentContext.DirectShipment.Id.Should().NotBeNull();
        }

        private ApiResponse<ShipmentPM> UpdateShipmentByAddingPackages(ShipmentPM shipment, PackagePM package, PayablesPM payable, ReceivablePM receivable)
        {
            shipment = AddPackagesToShipment(shipment, package);
            shipment = AddPayablesToShipment(shipment, payable);
            shipment = AddReceivableToShipment(shipment, receivable);
            shipment = AddPartnerToShipment(shipment, Partners);
            return APICaller.CallPut<ShipmentPM>(shipment, Urls.ShipmentController, UserTenant.Token);
        }

        private ShipmentPM AddPackagesToShipment(ShipmentPM shipment, PackagePM package)
        {
            package = new PackageBuilder().WithModel(package)
                .ShipmentNumber(shipment.ShipmentNumber)
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
        private ShipmentPM AddReceivableToShipment(ShipmentPM shipment, ReceivablePM receivable)
        {
            receivable = new ReceivableBuilder().WithModel(receivable)
                .ShipmentId(shipment.Id)
                .Build();

            return new ShipmentBuilder().WithModel(shipment)
                .ShipmentReceivable(receivable)
                .Build();
        }

        private ShipmentPM AddPartnerToShipment(ShipmentPM shipment, ShipmentPM partners)
        {
            return new ShipmentBuilder().WithModel(shipment)
                .AgentAddressCountryCode((string)partners.AgentAddressCountryCode)
                .AgentName((string)partners.AgentName)
                .AgentId((string)partners.AgentId)
                .Build();
        }

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

        private ReceivablePM CreateReceivableInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ReceivableBuilder().WithDefualtValues()
                .ChargesTypeName((string)dataTable.ChargesTypeName)
                .ChargesTypeCode((string)dataTable.ChargesType)
                .ChargesTypeIdByCode((string)dataTable.ChargesType)
                .MeasurementCode((string)dataTable.Measurement)
                .MeasurementIdByCode((string)dataTable.Measurement)
                .CurrencyCode((string)dataTable.Currency)
                .CurrencyIdByCode((string)dataTable.Currency)
                .ShipmentReceivableLineStatusCode((string)dataTable.ShipmentReceivableLineStatusCode)
                .Build();
        }

        private ShipmentPM AddAgentPartner(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ShipmentBuilder().WithDefualtValues()
                .AgentAddressCountryCode((string)dataTable.Country)
                .AgentName((string)dataTable.EnglishName)
                .AgentId((string)dataTable.PartnerType)
                .Build();
        }
        #endregion

    }
}
