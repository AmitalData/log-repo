using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class UpdateShipmentSteps
    {
        protected User User;
        protected readonly ShipmentContext ShipmentContext;
        protected ShipmentPackagePM _housePackage, _masterPackages;
        protected ShipmentPayablesPM _shipmentPayablesPM;
        protected Exception exceptionForHouse, exceptionForMaster, exceptionForPayables;

        public UpdateShipmentSteps(MultiUsers multiUsers, ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
            User = multiUsers.Users[0];
        }

        [Given(@"The master shipment packages fields")]
        public void GivenTheMasterShipmentPackagesFields(Table table)
        {
            _masterPackages = table.CreateInstance<ShipmentPackagePM>();
            _masterPackages.Tenant = User.Tenant;
        }

        [When(@"The put API sent to add master packages")]
        public void TheputAPIsenttoaddmasterpackages()
        {
            ShipmentContext.MasterShipment.NewConcurrencyGUID = Guid.NewGuid().ToString();
            ShipmentContext.MasterShipment.ShipmentPackages = new List<ShipmentPackagePM>();
            ShipmentContext.MasterShipment.FreightPrepaidCollectId = "P";
            ShipmentContext.MasterShipment.OtherPrepaidCollectId = "C";
            ShipmentContext.MasterShipment.CreatedByUserId = User.UserId;
            ShipmentContext.MasterShipment.UpdatedByUserId = User.UserId;

            _masterPackages.ShipmentNumber = ShipmentContext.MasterShipment.MasterShipmentNumber;
            _masterPackages.ShipmentId = ShipmentContext.MasterShipment.Id;
            ShipmentContext.MasterShipment.PackagesQuantity = _masterPackages.Quantity;
            ShipmentContext.MasterShipment.ShipmentPackages.Add(_masterPackages);

            var response = APICaller.CallPut<ShipmentPM>(ShipmentContext.MasterShipment, "Shipment", User.Token);
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
            _housePackage = table.CreateInstance<ShipmentPackagePM>();
            _housePackage.Tenant = User.Tenant;
        }

        [When(@"The put API sent to add house packages")]
        public void TheputAPIsenttoaddhousepackages()
        {
            ShipmentContext.HouseShipment.NewConcurrencyGUID = Guid.NewGuid().ToString();
            ShipmentContext.HouseShipment.ShipmentPackages = new List<ShipmentPackagePM>();
            ShipmentContext.HouseShipment.FreightPrepaidCollectId = "P";
            ShipmentContext.HouseShipment.OtherPrepaidCollectId = "C";
            ShipmentContext.HouseShipment.CreatedByUserId = User.UserId;
            ShipmentContext.HouseShipment.UpdatedByUserId = User.UserId;

            _housePackage.ShipmentNumber = ShipmentContext.HouseShipment.ShipmentNumber;
            _housePackage.ShipmentId = ShipmentContext.HouseShipment.Id;
            ShipmentContext.HouseShipment.PackagesQuantity = _housePackage.Quantity;
            ShipmentContext.HouseShipment.ShipmentPackages.Add(_housePackage);

            var response = APICaller.CallPut<ShipmentPM>(ShipmentContext.HouseShipment, "Shipment", User.Token);
            ShipmentContext.HouseShipment.Id =response.Data.Id;
        }

        [Then(@"A new house packages added successfully")]
        public void ThenANewHousePackagesAddedSuccessfully()
        {
            ShipmentContext.HouseShipment.Id.Should().NotBeNull();
        }

        [Given(@"The Payable Charge Type fields")]
        public void GivenThePayableChargeTypeFields(Table table)
        {
            _shipmentPayablesPM = table.CreateInstance<ShipmentPayablesPM>();
            _shipmentPayablesPM.Tenant = User.Tenant;
        }

        [When(@"The put API sent to add master Payable")]
        public void WhenThePutAPISentToAddMasterPayable()
        {
            ShipmentContext.MasterShipment.ShipmentPayables = new List<ShipmentPayablesPM>();
            ShipmentContext.MasterShipment.FreightPrepaidCollectId = "P";
            ShipmentContext.MasterShipment.OtherPrepaidCollectId = "C";
            ShipmentContext.MasterShipment.CreatedByUserId = User.UserId;
            ShipmentContext.MasterShipment.UpdatedByUserId = User.UserId;

            _shipmentPayablesPM.ShipmentId = ShipmentContext.MasterShipment.Id;
            ShipmentContext.MasterShipment.ShipmentPayables.Add(_shipmentPayablesPM);

            var response = APICaller.CallPut<ShipmentPM>(ShipmentContext.MasterShipment, "Shipment", User.Token);
            ShipmentContext.MasterShipment.Id =response.Data.Id;
        }

        [Then(@"The payable cherge type added successfully")]
        public void ThenThePayableChergeTypeAddedSuccessfully()
        {
            ShipmentContext.MasterShipment.Id.Should().NotBeNull();
        }
    }
}