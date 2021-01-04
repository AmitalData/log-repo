using FluentAssertions;
//using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.SecurityTests.Models.Login;
using Logitude.SecurityTests.Models.Shipment;
using Logitude.Test.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.Shipment
{
    [Binding]
    public class CreateAndUpdateShipmentSecurityAccessSteps
    {
        protected readonly UsersData Users;
        protected readonly ShipmentSecurityAccessStepsContext Context;

        public CreateAndUpdateShipmentSecurityAccessSteps(UsersData users, ShipmentSecurityAccessStepsContext context)
        {
            Users = users;
            Context = context;
        }

        [When(@"Create shipment request sent for User's Tenant")]
        public void WhenCreateShipmentRequestSentForUserSTenant()
        {
            var firstUser = Users.ListOfUserData[0];
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = firstUser.Tenant;
            shipmentModel.CreatedByUserId = firstUser.UserId;
            shipmentModel.UpdatedByUserId = firstUser.UserId;

            ShipmentPM shipmentPM = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", firstUser.Token);
            Context.ShipmentPM.Id = shipmentPM?.Id;
        }

        [Then(@"Shipment should be added successfully")]
        public void ThenShipmentShouldBeAddedSuccessfully()
        {
            Context.ShipmentPM.Id.Should().NotBeNull();
        }

        [When(@"Create shipment request sent for other Tenant")]
        public void WhenCreateShipmentRequestSentForOtherTenant()
        {
            var firstUser = Users.ListOfUserData[0];
            var secondUser = Users.ListOfUserData[1];

            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = firstUser.Tenant; // Second user try to Post on first user tenant
            shipmentModel.CreatedByUserId = secondUser.UserId;
            shipmentModel.UpdatedByUserId = secondUser.UserId;

            ShipmentPM shipmentPMs = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", secondUser.Token);
            Context.OtherShipmentPM.Id = shipmentPMs?.Id;
        }

        [Then(@"Shipment should not be added")]
        public void ThenShipmentShouldNotBeAdded()
        {
            Context.OtherShipmentPM.Id.Should().BeNull();
        }

        [When(@"Update shipment request sent for User's Tenant")]
        public void WhenUpdateShipmentRequestSentForUserSTenant()
        {
            var firstUser = Users.ListOfUserData[0];

            ShipmentPM shipmentModel = GetShipmentForFirstUser();
            shipmentModel.NewConcurrencyGUID = Guid.NewGuid().ToString();

            ShipmentPM shipmentPM = APICaller.CallPut<ShipmentPM>(shipmentModel, "shipment", firstUser.Token);
            Context.ShipmentPM.Id = shipmentPM?.Id;
        }

        [Then(@"Shipment should be Updated successfully")]
        public void ThenShipmentShouldBeUpdatedSuccessfully()
        {
            Context.ShipmentPM.Id.Should().NotBeNull();
        }

        [When(@"Update shipment request sent for other Tenant")]
        public void WhenUpdateShipmentRequestSentForOtherTenant()
        {
            var firstUser = Users.ListOfUserData[0];
            var secondUser = Users.ListOfUserData[1];

            ShipmentPM shipmentModel = GetShipmentForFirstUser();
            shipmentModel.NewConcurrencyGUID = Guid.NewGuid().ToString();

            ShipmentPM shipmentPMs = APICaller.CallPut<ShipmentPM>(shipmentModel, "shipment", secondUser.Token);
            Context.OtherShipmentPM.Id = shipmentPMs?.Id;
        }

        [Then(@"Shipment should not be Updated")]
        public void ThenShipmentShouldNotBeUpdated()
        {
            Context.OtherShipmentPM.Id.Should().BeNull();
        }

        private ShipmentPM GetValidShipmentPM()
        {
            return new ShipmentPM { 
                DirectionId = "E",
                TransportModeId = "A",
                ShipmentLevelCode = "C",
                NewConcurrencyGUID = Guid.NewGuid().ToString(),
                MainCarriageToPortId = "1-300930",
                MainCarriageFromPortId = "1-303023",
                BranchId = "1-1102",
                DepartmentId = "1-2988",
                OtherPrepaidCollectId = "P",
                FreightPrepaidCollectId = "P"
            };
        }

        private ShipmentPM GetShipmentForFirstUser()
        {
            var firstUser = Users.ListOfUserData[0];
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = firstUser.Tenant;
            shipmentModel.CreatedByUserId = firstUser.UserId;
            shipmentModel.UpdatedByUserId = firstUser.UserId;

            return APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", firstUser.Token);
        }
    }
}
