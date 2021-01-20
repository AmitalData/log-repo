using FluentAssertions;
using Logitude.Test.Base.Models.Login;
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
        protected SecurityAccessStepsContext<ShipmentPM> Context;

        public GetShipmentSecurityAccessSteps(MultiUsers multiUsers, SecurityAccessStepsContext<ShipmentPM> context)
        {
            Context = context;
            Context.FirstUser = multiUsers.Users[0];
            Context.SecondUser = multiUsers.Users[1];
        }

        [When(@"First user get the first shipment from shipments list")]
        public void WhenFirstUserGetTheFirstShipmentFromShipmentsList()
        {
            IEnumerable<ShipmentPM> firstUserShipmentsList = GetShipmentsListForFirstUser();
            Context.FirstUserPMData.Id = firstUserShipmentsList?.FirstOrDefault()?.Id;
        }

        [When(@"Second user get the shipment that requested by first user")]
        public void WhenSecondUserGetTheShipmentThatRequestedByFirstUser()
        {
            IEnumerable<ShipmentPM> firstUserShipmentsList = GetShipmentsListForFirstUser();
            string shipmentGetSingleUrl = URLs.ShipmentGetSingle(firstUserShipmentsList?.FirstOrDefault()?.Id);
            var response = APICaller.CallGet<ShipmentPM>(shipmentGetSingleUrl, Context.SecondUser.Token);
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


        protected IEnumerable<ShipmentPM> GetShipmentsListForFirstUser()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFilters
            {
                PageIndex = 0,
                PageSize = 1
            };

            APIResponse<IEnumerable<ShipmentPM>> response = APICaller.CallGetByFilters<IEnumerable<ShipmentPM>>(URLs.ShipmentViewsGetByFilters(), Context.FirstUser.Token, apiQueryFilters);
            return response.Data;
        }
    }
}