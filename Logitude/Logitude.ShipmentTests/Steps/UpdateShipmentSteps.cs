using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class UpdateShipmentSteps
    {
        protected User User;
        protected ShipmentPM _masterPM, _housePM;
        protected ShipmentPackagePM _housePackage, _masterPackages;
        protected ShipmentPayablesPM _shipmentPayablesPM;
        protected Exception exceptionForHouse, exceptionForMaster, exceptionForPayables;

        public UpdateShipmentSteps(MultiUsers multiUsers)
        {
            User = multiUsers.Users[0];
        }

        [Given(@"The master shipment packages fields")]
        public void GivenTheMasterShipmentPackagesFields(Table table)
        {
            _masterPackages = table.CreateInstance<ShipmentPackagePM>();
            _masterPackages.Tenant = User.Tenant;
        }

        [Given(@"MasterShipmentId is (.*)")] 
        public void GivenShipmentMasterIdIs(string masterShipmentId)
        {
            string singleShipmentUrl = "Shipment/GetSingle?id=" + masterShipmentId;
            _masterPM = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, User.Token, null);
        }

        [When(@"The put API sent to add master packages")]
        public void TheputAPIsenttoaddmasterpackages()
        {
            _masterPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
            _masterPackages.ShipmentNumber = _masterPM.MasterShipmentNumber;
            _masterPackages.ShipmentId = _masterPM.Id; 
            _masterPM.ShipmentPackages = new List<ShipmentPackagePM>();
            _masterPM.PackagesQuantity = _masterPackages.Quantity;
            _masterPM.ShipmentPackages.Add(_masterPackages);

            exceptionForMaster = Record.Exception(() => APICaller.CallPut<ShipmentPM>(_masterPM, "Shipment", User.Token));
        }

        [Then(@"A new master packages added successfully")]
        public void ThenANewMasterPackagesAddedSuccessfully()
        {
            exceptionForMaster.Should().BeNull();
        }

        [Given(@"The house shipment packages fields")]
        public void GivenTheHouseShipmentPackagesFields(Table table)
        {
            _housePackage = table.CreateInstance<ShipmentPackagePM>();
            _housePackage.Tenant = User.Tenant;
        }

        [Given(@"HouseShipmentId is (.*)")]
        public void GivenShipmentHouseIdIs(string houseShipmentId)
        {
            string singleShipmentUrl = "Shipment/GetSingle?id=" + houseShipmentId;
            _housePM = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, User.Token, null);
        }

        [When(@"The put API sent to add house packages")]
        public void TheputAPIsenttoaddhousepackages()
        {
            _housePM.NewConcurrencyGUID = Guid.NewGuid().ToString();

            _housePM.ShipmentPackages = new List<ShipmentPackagePM>();
            _housePM.PackagesQuantity = _housePackage.Quantity;
            _housePackage.ShipmentNumber = _housePM.ShipmentNumber;
            _housePackage.ShipmentId = _housePM.Id;
            _housePM.ShipmentPackages.Add(_housePackage);

            exceptionForHouse = Record.Exception(() => APICaller.CallPut<ShipmentPM>(_housePM, "Shipment", User.Token));
        }

        [Then(@"A new house packages added successfully")]
        public void ThenANewHousePackagesAddedSuccessfully()
        {
            exceptionForHouse.Should().BeNull();
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
            _masterPM.ShipmentPayables = new List<ShipmentPayablesPM>();
            _shipmentPayablesPM.ShipmentId = _masterPM.Id;
            _masterPM.ShipmentPayables.Add(_shipmentPayablesPM);

            exceptionForPayables = Record.Exception(() => APICaller.CallPut<ShipmentPM>(_masterPM, "Shipment", User.Token));
        }

        [Then(@"The payable cherge type added successfully")]
        public void ThenThePayableChergeTypeAddedSuccessfully()
        {
            exceptionForPayables.Should().BeNull();

        }
    }
}