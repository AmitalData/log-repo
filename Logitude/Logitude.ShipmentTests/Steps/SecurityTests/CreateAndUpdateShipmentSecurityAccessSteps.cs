using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
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

        public CreateAndUpdateShipmentSecurityAccessSteps(SecurityAccessStepsContext<ShipmentPM> context)
        {
            Context = context;
        }

        [When(@"Create shipment request sent for User's Tenant")]
        public void WhenCreateShipmentRequestSentForUserSTenant()
        {
            ApiResponse<ShipmentPM> response = CreateShipmentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
        }

        [Then(@"Shipment should be added successfully")]
        public void ThenShipmentShouldBeAddedSuccessfully()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Create shipment request sent for other Tenant")]
        public void WhenCreateShipmentRequestSentForOtherTenant()
        {
            Context.act = () => CreateShipmentForFirstUser(UserOtherTenant.Token);
        }

        [Then(@"Shipment should not be added")]
        public void ThenShipmentShouldNotBeAdded()
        {
            Context.act.Should().ThrowExactly<Exception>()
                .Where(m => m.Message.Contains("Sorry! you have no permission to do this operation on Tenant"));
        }

        [When(@"Update shipment request sent for User's Tenant")]
        public void WhenUpdateShipmentRequestSentForUserSTenant()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
        }

        [Then(@"Shipment should be Updated successfully")]
        public void ThenShipmentShouldBeUpdatedSuccessfully()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [When(@"Update shipment request sent for other Tenant")]
        public void WhenUpdateShipmentRequestSentForOtherTenant()
        {
            Context.act =() => UpdateShipmentForFirstUser(UserOtherTenant.Token);
        }

        [Then(@"Shipment should not be Updated")]
        public void ThenShipmentShouldNotBeUpdated()
        {
            Context.act.Should().ThrowExactly<Exception>()
                .Where(m => m.Message.Contains("Sorry! you have no permission to do this operation on Tenant"));
        }

        private ApiResponse<ShipmentPM> UpdateShipmentForFirstUser(string Token)
        {
            ApiResponse<ShipmentPM> response = CreateShipmentForFirstUser(UserTenant.Token);
            return APICaller.CallPut<ShipmentPM>(response.Data, Urls.ShipmentController, Token);
        }

        private ApiResponse<ShipmentPM> CreateShipmentForFirstUser(string Token)
        {
            ShipmentPM shipmentModel = GetValidUserShipmentPM();
            return APICaller.CallPost<ShipmentPM>(shipmentModel, Urls.ShipmentController, Token);
        }

        private ShipmentPM GetValidUserShipmentPM()
        {
            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId("E")
                .TransportModeId("A")
                .ShipmentLevelCode("C")
                .OtherPrepaidCollectId("P")
                .FreightPrepaidCollectId("C")
                .MainCarriageToPortIdByCode("LHR")
                .MainCarriageFromPortIdByCode("MIA")
                .Build();
        }
    }
}
