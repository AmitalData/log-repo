using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Extensions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class ExternalAPIDirectSteps
    {
        protected ExternalAPIDirectContext Context;

        public ExternalAPIDirectSteps(MultiUsers multiUsers, ExternalAPIDirectContext context)
        {
            Context = context;
            Context.User = multiUsers.Users[0];
        }

        [Given(@"Direct shipment with the following properties")]
        public void GivenDirectShipmentWithTheFollowingProperties(Table directShipmentTable)
        {
            Direct directShipment = directShipmentTable.CreateComplexInstance<Direct>();
            if (directShipment != null)
            {
                Context.Direct = directShipment;
            }
        }

        [Given(@"List of ocean or inland packages for direct shipment")]
        public void GivenListOfOceanOrInlandPackagesForDirectShipment(Table oceanOrInlandPackagesTable)
        {
            IEnumerable<OceanOrInlandPackage> oceanOrInlandPackages = oceanOrInlandPackagesTable.CreateComplexSet<OceanOrInlandPackage>();
            Context.Direct.OceanOrInlandPackages = new List<OceanOrInlandPackage>();
            Context.Direct.OceanOrInlandPackages.AddRange(oceanOrInlandPackages);
        }

        [Given(@"List of main carriage legs for direct shipment")]
        public void GivenListOfMainCarriageLegsForDirectShipment(Table mainCarriageLegsTable)
        {
            IEnumerable<MainCarriageLeg> mainCarriageLegs = mainCarriageLegsTable.CreateComplexSet<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs = new List<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs.AddRange(mainCarriageLegs);
        }

        [When(@"User create direct shipment using external API")]
        public void WhenUserCreateDirectShipmentUsingExternalAPI()
        {
            Direct createdDirectShipment = APICaller.CallPost<Direct>(Context.Direct, "Direct", Context.User.Token);
            Context.Direct.Id = createdDirectShipment?.Id;
        }

        [Then(@"The direct shipment should be created successfully")]
        public void ThenTheDirectShipmentShouldBeCreatedSuccessfully()
        {
            Context.Direct.Id.Should().NotBeNull();
        }


        //no need for this step .. just you want to use the code inside it for other steps .. I recommend to use a function as protected
        [Given(@"User get the last direct shipment")]
        public void WhenUserGetTheLastDirectShipment()
        {
            string directShipmentsRequestUrl = "ShipmentViews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=false&PageIndex=0&PageSize=1" +
                "&Filter1Name=ShipmentLevelCode&Filter1Operator=equals&Filter1Value=D" +
                "&Filter2Name=CreatedByUserId&Filter2Operator=equals&Filter2Value=" + Context.User.UserId +
                "&SortBy=CreateDateTime&SortDirection=descending";

            IEnumerable<ShipmentPM> directShipments = APICaller.CallGet<IEnumerable<ShipmentPM>>(directShipmentsRequestUrl, Context.User.Token, "Result");

            string lastDirectShipmentId = directShipments?.FirstOrDefault()?.Id;

            Direct directShipment = APICaller.CallGet<Direct>(("Direct/GetSingleDirect?id=" + lastDirectShipmentId), Context.User.Token, null);

            if (directShipment != null)
            {
                Context.Direct = directShipment;
            }
        }
    }
}