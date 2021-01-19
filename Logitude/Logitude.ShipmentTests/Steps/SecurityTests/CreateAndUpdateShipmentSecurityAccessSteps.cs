using FluentAssertions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System;
using TechTalk.SpecFlow;
using Logitude.ShipmentTests.Models;

namespace Logitude.ShipmentTests.Steps.SecurityTests
{
    [Binding]
    public class CreateAndUpdateShipmentSecurityAccessSteps
    {
        protected SecurityAccessStepsContext<ShipmentPM> Context;
        private string ErrorMsg; 

        public CreateAndUpdateShipmentSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<ShipmentPM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"Create shipment request sent for User's Tenant")]
        public void WhenCreateShipmentRequestSentForUserSTenant()
        {
            var firstUser = Context.FirstUser;
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = firstUser.Tenant;
            shipmentModel.CreatedByUserId = firstUser.UserId;
            shipmentModel.UpdatedByUserId = firstUser.UserId;

            var response = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", firstUser.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
            ErrorMsg = response?.ErrorMessage;
        }

        [Then(@"Shipment should be added successfully")]
        public void ThenShipmentShouldBeAddedSuccessfully()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
            ErrorMsg.Should().BeNull();
        }

        [When(@"Create shipment request sent for other Tenant")]
        public void WhenCreateShipmentRequestSentForOtherTenant()
        {
            var firstUser = Context.FirstUser;
            var secondUser = Context.SecondUser;

            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = firstUser.Tenant; // Second user try to Post on first user tenant
            shipmentModel.CreatedByUserId = secondUser.UserId;
            shipmentModel.UpdatedByUserId = secondUser.UserId;

            var response = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", secondUser.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
            ErrorMsg = response?.ErrorMessage;
        }

        [Then(@"Shipment should not be added")]
        public void ThenShipmentShouldNotBeAdded()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
            ErrorMsg.Should().Contain("Sorry! you have no permission to do this operation on Tenant");
        }

        [When(@"Update shipment request sent for User's Tenant")]
        public void WhenUpdateShipmentRequestSentForUserSTenant()
        {
            var firstUser = Context.FirstUser;

            ShipmentPM shipmentModel = GetShipmentForFirstUser();
            shipmentModel.NewConcurrencyGUID = Guid.NewGuid().ToString();

            var response = APICaller.CallPut<ShipmentPM>(shipmentModel, "shipment", firstUser.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
            ErrorMsg = response?.ErrorMessage;
        }

        [Then(@"Shipment should be Updated successfully")]
        public void ThenShipmentShouldBeUpdatedSuccessfully()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
            ErrorMsg.Should().BeNull();
        }

        [When(@"Update shipment request sent for other Tenant")]
        public void WhenUpdateShipmentRequestSentForOtherTenant()
        {
            var firstUser = Context.FirstUser;
            var secondUser = Context.SecondUser;

            ShipmentPM shipmentModel = GetShipmentForFirstUser();
            shipmentModel.NewConcurrencyGUID = Guid.NewGuid().ToString();

            var response = APICaller.CallPut<ShipmentPM>(shipmentModel, "shipment", secondUser.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
            ErrorMsg = response?.ErrorMessage;
        }

        [Then(@"Shipment should not be Updated")]
        public void ThenShipmentShouldNotBeUpdated()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
            ErrorMsg.Should().Contain("Sorry! you have no permission to do this operation on Tenant");
        }

        private ShipmentPM GetValidShipmentPM()
        {
            return new ShipmentPM
            {
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
            var firstUser = Context.FirstUser;
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = firstUser.Tenant;
            shipmentModel.CreatedByUserId = firstUser.UserId;
            shipmentModel.UpdatedByUserId = firstUser.UserId;
            var postResponse = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", firstUser.Token);
            return postResponse.Data;
        }
    }
}
