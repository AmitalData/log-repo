using FluentAssertions;
using Logitude.SecurityTests.Models.Login;
using Logitude.SecurityTests.Models.Shipment;
using Logitude.ShipmentTests.Steps.Shipment;
using Logitude.Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Xunit;

namespace Logitude.SecurityTests.Steps.Shipment
{
    [Binding]
    public class UpdateShipment
    {
        protected readonly UserData UserData;
        protected ShipmentPM _masterPM, _housePM;
        protected ShipmentPackagePM _housePackage, _masterPackages;
        protected ShipmentPayablesPM _shipmentPayablesPM;
        protected Exception exceptionForHouse, exceptionForMaster, exceptionForPayables;

        public UpdateShipment(UsersData usersData)
        {
            UserData = usersData.Users[0];
        }

        [Given(@"The master shipment fields")]
        public void GivenTheMasterShipmentFields(Table table)
        {
            _masterPM = table.CreateInstance<ShipmentPM>();
            _masterPM.Tenant = UserData.Tenant;
            _masterPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        [When(@"The post API sent to create master shipment")]
        public void WhenTheShipmentCreateAPISent()
        {
            _masterPM = APICaller.CallPost<ShipmentPM>(_masterPM, "Shipment", UserData.Token);
        }

        [Then(@"A new master created successfully")]
        public void ThenANewMasterCreatedSuccessfully()
        {
            _masterPM.Id.Should().NotBeNull();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////

        [Given(@"The house shipment fields")]
        public void GivenTheHouseShipmentFields(Table table)
        {
            _housePM = table.CreateInstance<ShipmentPM>();
            _housePM.Tenant = UserData.Tenant;
            _housePM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        [Given(@"MasterShipmentDataId is (.*) and MasterShipmentNumber is (.*)")]
        public void GivenHouseMasterIdIs(string masterShipmentDataId, string MasterShipmentNumber)
        {
            _housePM.MasterShipmentDataId = masterShipmentDataId; // "1-1997645"
            _housePM.MasterShipmentNumber = MasterShipmentNumber; // "M1304"
        }

        [When(@"The post API sent to create house shipment")]
        public void WhenThePostAPISentToCreateHouseShipment()
        {
            _housePM = APICaller.CallPost<ShipmentPM>(_housePM, "Shipment", UserData.Token);
        }

        [Then(@"A new house created successfully")]
        public void ThenANewHouseCreatedSuccessfully()
        {
            _housePM.Id.Should().NotBeNull();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////

        [Given(@"The master shipment packages fields")]
        public void GivenTheMasterShipmentPackagesFields(Table table)
        {
            _masterPackages = table.CreateInstance<ShipmentPackagePM>();
            _masterPackages.Tenant = UserData.Tenant;
        }

        [Given(@"MasterShipmentId is (.*)")] 
        public void GivenShipmentMasterIdIs(string masterShipmentId)
        {
            string singleShipmentUrl = "Shipment/GetSingle?id=" + masterShipmentId;
            _masterPM = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserData.Token, null);
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

            exceptionForMaster = Record.Exception(() => APICaller.CallPut<ShipmentPM>(_masterPM, "Shipment", UserData.Token));
        }

        [Then(@"A new master packages added successfully")]
        public void ThenANewMasterPackagesAddedSuccessfully()
        {
            exceptionForMaster.Should().BeNull();
        }

        ///////////////////////////////////////////////////////////////////////////////

        [Given(@"The house shipment packages fields")]
        public void GivenTheHouseShipmentPackagesFields(Table table)
        {
            _housePackage = table.CreateInstance<ShipmentPackagePM>();
            _housePackage.Tenant = UserData.Tenant;
        }

        [Given(@"HouseShipmentId is (.*)")]
        public void GivenShipmentHouseIdIs(string houseShipmentId)
        {
            string singleShipmentUrl = "Shipment/GetSingle?id=" + houseShipmentId;
            _housePM = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserData.Token, null);
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

            exceptionForHouse = Record.Exception(() => APICaller.CallPut<ShipmentPM>(_housePM, "Shipment", UserData.Token));
        }

        [Then(@"A new house packages added successfully")]
        public void ThenANewHousePackagesAddedSuccessfully()
        {
            exceptionForHouse.Should().BeNull();
        }

        ////////////////////////////////////////////////////////////////////////////

        [Given(@"The Payable Charge Type fields")]
        public void GivenThePayableChargeTypeFields(Table table)
        {
            _shipmentPayablesPM = table.CreateInstance<ShipmentPayablesPM>();
            _shipmentPayablesPM.Tenant = UserData.Tenant;
        }

        [When(@"The put API sent to add master Payable")]
        public void WhenThePutAPISentToAddMasterPayable()
        {
            _masterPM.shipmentPayables = new List<ShipmentPayablesPM>();
            _shipmentPayablesPM.ShipmentId = _masterPM.Id;
            _masterPM.shipmentPayables.Add(_shipmentPayablesPM);

            exceptionForPayables = Record.Exception(() => APICaller.CallPut<ShipmentPM>(_masterPM, "Shipment", UserData.Token));
        }

        [Then(@"The payable cherge type added successfully")]
        public void ThenThePayableChergeTypeAddedSuccessfully()
        {
            exceptionForPayables.Should().BeNull();

        }
    }
}