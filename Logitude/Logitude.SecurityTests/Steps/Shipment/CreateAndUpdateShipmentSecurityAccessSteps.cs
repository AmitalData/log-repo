using FluentAssertions;
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
            var model = GetValidShipmentPM();
            model.Tenant = firstUser.Tenant;
            model.CreatedByUserId = firstUser.UserId;
            model.UpdatedByUserId = firstUser.UserId;
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallPost<IEnumerable<ShipmentPM>>(model, "shipment", firstUser.Token);
            Context.ShipmentPM.Id = shipmentPMs.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should be added successfully")]
        public void ThenShipmentShouldBeAddedSuccessfully()
        {
            Context.ShipmentPM.Id.Should().NotBeNull();
        }

        [When(@"Create shipment request sent for other Tenant")]
        public void WhenCreateShipmentRequestSentForOtherTenant()
        {
            var OtherUserToken = Users.ListOfUserData[2].Token;
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallPost<IEnumerable<ShipmentPM>>(GetValidShipmentPM(), "shipment", OtherUserToken);
            Context.OtherShipmentPM.Id = shipmentPMs.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should not be added")]
        public void ThenShipmentShouldNotBeAdded()
        {
            Context.OtherShipmentPM.Id.Should().BeNull();
        }

        [When(@"Update shipment request sent for User's Tenant")]
        public void WhenUpdateShipmentRequestSentForUserSTenant()
        {
            var UserToken = Users.ListOfUserData[1].Token;
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallPut<IEnumerable<ShipmentPM>>(GetValidShipmentPM(), "shipment", UserToken);
            Context.ShipmentPM.Id = shipmentPMs.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should be Updated successfully")]
        public void ThenShipmentShouldBeUpdatedSuccessfully()
        {
            Context.ShipmentPM.Id.Should().NotBeNull();
        }

        [When(@"Update shipment request sent for other Tenant")]
        public void WhenUpdateShipmentRequestSentForOtherTenant()
        {
            var OtherUserToken = Users.ListOfUserData[2].Token;
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallPost<IEnumerable<ShipmentPM>>(GetValidShipmentPM(), "shipment", OtherUserToken);
            Context.OtherShipmentPM.Id = shipmentPMs.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should not be Updated")]
        public void ThenShipmentShouldNotBeUpdated()
        {
            Context.OtherShipmentPM.Id.Should().BeNull();
        }

        private ShipmentPM GetValidShipmentPM()
        {
            return new ShipmentPM { 
                Id = "1",
                Tenant = 951,
                DirectionId = "E",
                TransportModeId = "A",
                shipmentType = "Direct",
                FromPortId = "1-3824",
                ToPortId = "1-3824",
                ShipmentLevelCode = "C",
                NewConcurrencyGUID = Guid.NewGuid().ToString(),
                MainCarriageToPortId = "1-300930",
                MainCarriageFromPortId = "1-303023",
                BranchId = "1-1102",
                DepartmentId = "1-2988",
                BasicFreightId = "P",
                OtherPrepaidCollectId = "P",
                FreightPayerAddressId = "1-610125",
                FreightPayerId = "1-577381",
                FreightPrepaidCollectId = "P",
                CreatedByUserId = "1-108265",
                UpdatedByUserId = "1-108265"
            };
        }

    }
}
