using FluentAssertions;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;

namespace Logitude.ShipmentTests.Steps.SecurityTests
{
    [Binding]
    public class GetShipmentSecurityAccessSteps
    {
        private SecurityAccessStepsContext<ShipmentPM> Context;

        public GetShipmentSecurityAccessSteps(SecurityAccessStepsContext<ShipmentPM> context)
        {
            Context = context;
        }

        #region Step Region

        #region Get shipment from user's tenant steps
        [When(@"get a shipment from User's shipment list")]
        public void WhenGetAShipmentFromUserSShipmentList()
        {
            ShipmentPM shipment = GetAShipmentFromFirstUserList();
            Context.FirstUserPMData.Id = shipment?.Id;
        }

        [Then(@"the shipment should exist")]
        public void ThenTheShipmentShouldExist()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }
        #endregion

        #region Get shipment from other tenant steps
        [When(@"get a shipment from Other Tenant")]
        public void WhenGetAShipmentFromOtherTenant()
        {
            ApiResponse<ShipmentPM> response = GetASingleShipmentForFirstUser(UserOtherTenant.Token);
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"the shipment should not exist")]
        public void ThenTheShipmentShouldNotExist()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }
        #endregion

        #endregion

        #region Private Function Region
        private ApiResponse<ShipmentPM> GetASingleShipmentForFirstUser(string Token)
        {
            ShipmentPM firstUserShipment = GetAShipmentFromFirstUserList();
            string shipmentGetSingleUrl = Urls.ShipmentGetSingle(firstUserShipment?.Id);
            return APICaller.CallGet<ShipmentPM>(shipmentGetSingleUrl, Token);
        }

        private ShipmentPM GetAShipmentFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues().Build();

            ApiResponse<IEnumerable<ShipmentPM>> response = APICaller.CallGetByFilters<IEnumerable<ShipmentPM>>(Urls.ShipmentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
        #endregion
    }
}