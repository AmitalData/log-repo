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
        }

        [Given(@"First user request the shipments list")]
        public void GivenFirstUserRequestTheShipmentsList()
        {
            Context.ShipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>("ShipmentViews/getbyfilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10", UsersData.Users[0].Token, "Result");
        }

        [Given(@"Second user request the shipment that requested by first user")]
        public void GivenSecondUserRequestTheShipmentThatRequestedByFirstUser()
        {
            GivenFirstUserRequestTheShipmentsList();
            WhenGetTheFirstShipmentFromShipmentsList();

            ShipmentPM shipmentPM = APICaller.CallGet<ShipmentPM>("Shipment/GetSingle?id=" + Context.ShipmentPM.Id, UsersData.Users[1].Token, null);
            Context.OtherShipmentPM.Id = shipmentPM?.Id;
        }

        [When(@"Get the first shipment from shipments list")]
        public void WhenGetTheFirstShipmentFromShipmentsList()
        {
            Context.ShipmentPM.Id = Context.ShipmentPMs?.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should be exists")]
        public void ThenShipmentShouldBeExists()
        {
            Context.ShipmentPM.Id.Should().NotBeNull();
        }

        [Then(@"Shipment should not be exists")]
        public void ThenShipmentShouldNotBeExists()
        {
            Context.OtherShipmentPM.Id.Should().BeNull();
        }
    }
}