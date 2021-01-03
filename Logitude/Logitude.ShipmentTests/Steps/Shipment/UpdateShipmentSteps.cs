using FluentAssertions;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.SecurityTests.Models.Login;
using Logitude.SecurityTests.Models.Shipment;
using Logitude.Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.SecurityTests.Steps.Shipment
{
    [Binding]
    public class UpdateShipment
    {
        protected readonly UserData UserData;
        protected ShipmentPM _shipmentPM;

        public UpdateShipment(UsersData usersData, ShipmentPM shipmentPM)
        {
            UserData = usersData.Users[0];
            _shipmentPM = shipmentPM;
        }

        [Given(@"The master shipment fields")]
        public void GivenTheMasterShipmentFields(Table table)
        {
            _shipmentPM = table.CreateInstance<ShipmentPM>();
            _shipmentPM.Tenant = UserData.Tenant;
            _shipmentPM.NewConcurrencyGUID = Guid.NewGuid().ToString();
        }

        [When(@"The shipment create API sent")]
        public void WhenTheShipmentCreateAPISent()
        {
            _shipmentPM = APICaller.CallPost<ShipmentPM>(_shipmentPM, "Shipment", UserData.Token);
        }

        [Then(@"A new master created successfully")]
        public void ThenANewMasterCreatedSuccessfully()
        {
            _shipmentPM.Id.Should().NotBeNull();
        }

        [Given(@"The house shipment fields")]
        public void GivenTheHouseShipmentFields(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"A new house created successfully")]
        public void ThenANewHouseCreatedSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"The master shipment packages fields")]
        public void GivenTheMasterShipmentPackagesFields(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"The shipment update API sent")]
        public void WhenTheShipmentUpdateAPISent()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"A new master packages added successfully")]
        public void ThenANewMasterPackagesAddedSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"The house shipment packages fields")]
        public void GivenTheHouseShipmentPackagesFields(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"A new house packages added successfully")]
        public void ThenANewHousePackagesAddedSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"The Payable Charge Type fields")]
        public void GivenThePayableChargeTypeFields(Table table)
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"The payable cherge type added successfully")]
        public void ThenThePayableChergeTypeAddedSuccessfully()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"houseMasterId is (.*)")]
        public void GivenHouseMasterIdIs(string houseMasterId)
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"shipmentMasterId is (.*)")]
        public void GivenShipmentMasterIdIs(string shipmentMasterId)
        {
            ScenarioContext.Current.Pending();
        }

    }
}