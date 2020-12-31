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
        protected string ShipmentId;

        public SpecFlowFeature1Steps(UserData userData)
        {
            UserData = userData;
        }

        [When(@"Request first shipment from shipments list")]
        public void WhenRequestFirstShipmentFromShipmentsList()
        {
            IEnumerable<ShipmentPM> shipmentPMs = APICaller.CallGet<IEnumerable<ShipmentPM>>("ShipmentViews/getbyfilters?ForceCacheRefresh=false&GetAll=false&GetCount=true&PageIndex=0&PageSize=10", UserData.Token, "Result");
            ShipmentId = shipmentPMs?.FirstOrDefault()?.Id;
        }
        
        [Then(@"The eequested shipment should be exists")]
        public void ThenTheEequestedShipmentShouldBeExists()
        {
            ShipmentId.Should().NotBeNull();
        }
    }
}
