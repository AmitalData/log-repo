using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
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

        #region Step Region

        #region Create shipment for user's tenant steps
        [When(@"create a shipment for user's tenant")]
        public void WhenCreateAShipmentForUserSTenant()
        {
            ApiResponse<ShipmentPM> response = CreateShipmentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the shipment should create successfully")]
        public void ThenTheShipmentShouldCreateSuccessfully()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        #endregion

        #region Create shipment for other tenant steps
        [When(@"create a shipment for other tenant")]
        public void WhenCreateAShipmentForOtherTenant()
        {
            Context.act = () => CreateShipmentForFirstUser(UserOtherTenant.Token);
        }

        [Then(@"the shipment should not create successfully")]
        public void ThenTheShipmentShouldNotCreateSuccessfully()
        {
            Context.act.Should().ThrowExactly<Exception>()
                .Where(m => m.Message.Contains("Sorry! you have no permission to do this operation on Tenant"));
        }
        #endregion

        #region Update shipment for user's tenant steps
        [When(@"update a shipment for user's tenant")]
        public void WhenUpdateAShipmentForUserSTenant()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentForFirstUser(UserTenant.Token);
            Context.FirstUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the shipment should update successfully")]
        public void ThenTheShipmentShouldUpdateSuccessfully()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        #endregion

        #region Update shipment for other tenant steps
        [When(@"update a shipment for other tenant")]
        public void WhenUpdateAShipmentForOtherTenant()
        {
            Context.act = () => UpdateShipmentForFirstUser(UserOtherTenant.Token);
        }

        [Then(@"the shipment should not update successfully")]
        public void ThenTheShipmentShouldNotUpdateSuccessfully()
        {
            Context.act.Should().ThrowExactly<Exception>()
                .Where(m => m.Message.Contains("Sorry! you have no permission to do this operation on Tenant"));
        }
        #endregion

        #endregion

        #region Private Function Region
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

        #endregion

        #region Build Models Region
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

        #endregion
    }
}