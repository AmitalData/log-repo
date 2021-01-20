using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps.SecurityTests
{
    [Binding]
    public class CreateAndUpdateShipmentSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ShipmentPM> Context;
        private string ErrorMsg; 

        public CreateAndUpdateShipmentSecurityAccessSteps(SecurityAccessStepsContext<ShipmentPM> context)
        {
            Context = context;
        }

        [When(@"Create shipment request sent for User's Tenant")]
        public void WhenCreateShipmentRequestSentForUserSTenant()
        {
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = UserTenant.Tenant;
            shipmentModel.CreatedByUserId = UserTenant.UserId;
            shipmentModel.UpdatedByUserId = UserTenant.UserId;

            var response = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", UserTenant.Token);
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
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = UserTenant.Tenant; // Second user try to Post on first user tenant
            shipmentModel.CreatedByUserId = UserOtherTenant.UserId;
            shipmentModel.UpdatedByUserId = UserOtherTenant.UserId;

            var response = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", UserOtherTenant.Token);
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
            ShipmentPM shipmentModel = GetShipmentForFirstUser();
            shipmentModel.NewConcurrencyGUID = Guid.NewGuid().ToString();

            var response = APICaller.CallPut<ShipmentPM>(shipmentModel, "shipment", UserTenant.Token);
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
            ShipmentPM shipmentModel = GetShipmentForFirstUser();
            shipmentModel.NewConcurrencyGUID = Guid.NewGuid().ToString();

            var response = APICaller.CallPut<ShipmentPM>(shipmentModel, "shipment", UserOtherTenant.Token);
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
            ShipmentPM shipmentModel = GetValidShipmentPM();
            shipmentModel.Tenant = UserTenant.Tenant;
            shipmentModel.CreatedByUserId = UserTenant.UserId;
            shipmentModel.UpdatedByUserId = UserTenant.UserId;
            var postResponse = APICaller.CallPost<ShipmentPM>(shipmentModel, "shipment", UserTenant.Token);
            return postResponse.Data;
        }
    }
}
