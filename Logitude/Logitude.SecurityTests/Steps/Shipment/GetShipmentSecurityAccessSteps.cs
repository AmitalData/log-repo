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

        [Given(@"First user request the shipments list")]
        public void GivenFirstUserRequestTheShipmentsList()
        {
            string apiRequestUrl = "ShipmentViews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>(apiRequestUrl, Context.FirstUser.Token, "Result");
            Context.FirstUserShipments = shipmentPMs;
        }

        [When(@"First user get the first shipment from shipments list")]
        public void WhenFirstUserGetTheFirstShipmentFromShipmentsList()
        {
            Context.FirstUserShipment.Id = Context.FirstUserShipments?.FirstOrDefault()?.Id;
        }

        [When(@"Second user request the shipment that requested by first user")]
        public void WhenSecondUserRequestTheShipmentThatRequestedByFirstUser()
        {
            string apiRequestUrl = "Shipment/GetSingle?id=" + Context.FirstUserShipment.Id;
            ShipmentPM shipmentPM = APICaller.CallGet<ShipmentPM>(apiRequestUrl, Context.SecondUser.Token, null);
            Context.SecondUserShipment.Id = shipmentPM?.Id;
        }

        [Then(@"Users should not be on same tenant")]
        public void ThenUsersShouldNotBeOnSameTenant()
        {
            Context.FirstUser.Tenant.Should().NotBe(Context.SecondUser.Tenant);
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
    }
}