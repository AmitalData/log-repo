using FluentAssertions;
using Logitude.SecurityTests.Models.Login;
using Logitude.SecurityTests.Models.Shipment;
using Logitude.Test.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.SecurityTests.Features.Shipment
{
    [Binding]
    public class SpecFlowFeature1Steps
    {
        protected readonly UserData UserData;
        protected IEnumerable<ShipmentPM> ShipmentPMs;
        protected string ShipmentId;

        public SpecFlowFeature1Steps(UserData userData)
        {
            UserData = userData;
        }

        [Given(@"User request the shipments list")]
        public void GivenUserRequestTheShipmentsList()
        {
            string apiRequestUrl = "ShipmentViews/getbyfilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10";
            ShipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>(apiRequestUrl, UserData.Token, "Result");
        }

        [When(@"User get the first shipment from shipments list")]
        public void WhenUserGetTheFirstShipmentFromShipmentsList()
        {
            ShipmentId = ShipmentPMs?.FirstOrDefault()?.Id;
        }

        [Then(@"Shipment should be exists")]
        public void ThenShipmentShouldBeExists()
        {
            ShipmentId.Should().NotBeNull();
        }
    }
}