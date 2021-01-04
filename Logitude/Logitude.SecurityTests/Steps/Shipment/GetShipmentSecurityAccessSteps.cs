using FluentAssertions;
//using Logitude.BL.ShipmentsModel.EntityPMs;
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
        protected readonly UserData User;
        protected readonly ShipmentSecurityAccessStepsContext Context;

        public GetShipmentSecurityAccessSteps(UserData user, ShipmentSecurityAccessStepsContext context)
        {
            User = user;
            Context = context;
        }

        [When(@"Get the first shipment from shipments list")]
        public void WhenGetTheFirstShipmentFromShipmentsList()
        {
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>("ShipmentViews/getbyfilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10", User.Token, "Result");
            Context.ShipmentPM.Id = shipmentPMs.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should be exists")]
        public void ThenShipmentShouldBeExists()
        {
            Context.ShipmentPM.Id.Should().NotBeNull();
        }


        [When(@"Get shipment from other tenant")]
        public void WhenGetShipmentFromOtherTenant()
        {
            ShipmentPM otherShipmentPM = APICaller.CallGet<ShipmentPM>("Shipment/GetSingle?id=" + Context.ShipmentPM.Id, User.Token, null);
            Context.OtherShipmentPM.Id = otherShipmentPM?.Id;
        }

        [Then(@"Shipment from other tenant should not be exists")]
        public void ThenShipmentFromOtherTenantShouldNotBeExists()
        {
            Context.OtherShipmentPM.Id.Should().BeNull();
        }
    }
}