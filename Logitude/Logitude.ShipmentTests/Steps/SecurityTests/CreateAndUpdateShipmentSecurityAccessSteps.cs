using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
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
            ApiResponse<ShipmentPM> response = CreateShipmentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
            ErrorMsg = response.ErrorMessage;
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
            ApiResponse<ShipmentPM> response = CreateShipmentForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
            ErrorMsg = response.ErrorMessage;
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
            ApiResponse<ShipmentPM> response = UpdateShipmentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
            ErrorMsg = response.ErrorMessage;
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
            ApiResponse<ShipmentPM> response = UpdateShipmentForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
            ErrorMsg = response.ErrorMessage;
        }

        [Then(@"Shipment should not be Updated")]
        public void ThenShipmentShouldNotBeUpdated()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
            ErrorMsg.Should().Contain("Sorry! you have no permission to do this operation on Tenant");
        }

        private ApiResponse<ShipmentPM> UpdateShipmentForFirstUser(string Token)
        {
            ApiResponse<ShipmentPM> response = CreateShipmentForFirstUser(UserTenant.Token);
            return APICaller.CallPut<ShipmentPM>(response.Data, Urls.Shipment(), Token);
        }

        private ApiResponse<ShipmentPM> CreateShipmentForFirstUser(string Token)
        {
            ShipmentPM shipmentModel = GetValidUserShipmentPM();
            return APICaller.CallPost<ShipmentPM>(shipmentModel, Urls.Shipment(), Token);
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
