using FluentAssertions;
using Logitude.Test.Base.Services;
using Logitude.Test.Base.Context;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Constants;

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

        [When(@"First user get the first shipment from shipments list")]
        public void WhenFirstUserGetTheFirstShipmentFromShipmentsList()
        {
            ShipmentPM shipment = GetAShipmentFromFirstUserList();
            Context.FirstUserPMData.Id = shipment?.Id;
        }

        [When(@"Second user get the shipment that requested by first user")]
        public void WhenSecondUserGetTheShipmentThatRequestedByFirstUser()
        {
            APIResponse<ShipmentPM> response = GetAsingleShipmentForFirstUser(UserOtherTenant.Token); 
            Context.SecondUserPMData.Id = response.Data?.Id;
        }

        [Then(@"Shipment for first user should be exists")]
        public void ThenShipmentForFirstUserShouldBeExists()
        {
            Context.FirstUserPMData.Id.Should().NotBeNull();
        }

        [Then(@"Shipment for second user should not be exists")]
        public void ThenShipmentForSecondUserShouldNotBeExists()
        {
            Context.SecondUserPMData.Id.Should().BeNull();
        }

        private APIResponse<ShipmentPM> GetAsingleShipmentForFirstUser(string Token)
        {
            ShipmentPM firstUserShipment = GetAShipmentFromFirstUserList();
            string shipmentGetSingleUrl = URLs.ShipmentGetSingle(firstUserShipment?.Id);
            return APICaller.CallGet<ShipmentPM>(shipmentGetSingleUrl, Token);
        }

        private ShipmentPM GetAShipmentFromFirstUserList()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            APIResponse<IEnumerable<ShipmentPM>> response = APICaller.CallGetByFilters<IEnumerable<ShipmentPM>>(URLs.ShipmentViewsGetByFilters(), UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }
    }
}