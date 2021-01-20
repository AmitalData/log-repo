using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Context;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps.SecurityTests
{
    [Binding]
    public class GetShipmentSecurityAccessSteps
    {
        protected SecurityAccessStepsContext<ShipmentPM> Context;

        public GetShipmentSecurityAccessSteps(SecurityAccessStepsContext<ShipmentPM> context)
        {
            Context = context;
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
            string singleShipmentUrl = "Shipment/GetSingle?id=" + firstUserShipmentsList?.FirstOrDefault()?.Id;
            var response = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, UserOtherTenant.Token);
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
            string shipmentsListUrl = "ShipmentViews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            //this method is waiting the CallGetByFilter to be implemented by Abd.M
            // IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>(shipmentsListUrl, Context.FirstUser.Token, "Result");
            //return shipmentPMs;
            return null;
        }
    }
}