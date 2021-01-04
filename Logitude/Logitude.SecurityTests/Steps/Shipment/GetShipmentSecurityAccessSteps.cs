using FluentAssertions;
using Logitude.SecurityTests.Models.Login;
using Logitude.SecurityTests.Models.Shipment;
using Logitude.Test.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Steps.Shipment
{
    [Binding]
    public class GetShipmentSecurityAccessSteps
    {
        protected readonly UsersData UsersData;
        protected ShipmentSecurityAccessStepsContext Context;

        public GetShipmentSecurityAccessSteps(UsersData usersData, ShipmentSecurityAccessStepsContext context)
        {
            UsersData = usersData;
            Context = context;
            Context.FirstUser = UsersData.Users[0];
            Context.SecondUser = UsersData.Users[1];
        }

        [When(@"First user get the first shipment from shipments list")]
        public void WhenFirstUserGetTheFirstShipmentFromShipmentsList()
        {
            IEnumerable<ShipmentPM> firstUserShipmentsList = GetShipmentsListForFirstUser();
            Context.FirstUserShipment.Id = firstUserShipmentsList?.FirstOrDefault()?.Id;
        }

        [When(@"Second user get the shipment that requested by first user")]
        public void WhenSecondUserGetTheShipmentThatRequestedByFirstUser()
        {
            IEnumerable<ShipmentPM> firstUserShipmentsList = GetShipmentsListForFirstUser();
            string singleShipmentUrl = "Shipment/GetSingle?id=" + firstUserShipmentsList?.FirstOrDefault()?.Id;
            ShipmentPM shipmentPM = APICaller.CallGet<ShipmentPM>(singleShipmentUrl, Context.SecondUser.Token, null);
            Context.SecondUserShipment.Id = shipmentPM?.Id;
        }

        [Then(@"Shipment for first user should be exists")]
        public void ThenShipmentForFirstUserShouldBeExists()
        {
            Context.FirstUserShipment.Id.Should().NotBeNull();
        }

        [Then(@"Shipment for second user should not be exists")]
        public void ThenShipmentForSecondUserShouldNotBeExists()
        {
            Context.SecondUserShipment.Id.Should().BeNull();
        }


        protected IEnumerable<ShipmentPM> GetShipmentsListForFirstUser()
        {
            string shipmentsListUrl = "ShipmentViews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>(shipmentsListUrl, Context.FirstUser.Token, "Result");
            return shipmentPMs;
        }
    }
}