using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Extensions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TechTalk.SpecFlow;
using Xunit;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class ExternalAPIDirectSteps
    {
        protected ExternalAPIDirectContext Context;
        string ExceptionMessage;

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

        [Given(@"User adding main carriage legs to last direct shipmentd")]
        public void GivenUserAddingMainCarriageLegsToLastDirectShipmentd(Table table)
        {
            IEnumerable<MainCarriageLeg> mainCarriageLegs = table.CreateComplexSet<MainCarriageLeg>();
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

        [Given(@"The main carriage legs are added to last direct shipment")]
        public void GivenTheMainCarriageLegsAreAddedToLastDirectShipment(Table mainCarriageLegsTable)
        {
            IEnumerable<MainCarriageLeg> mainCarriageLegs = mainCarriageLegsTable.CreateComplexSet<MainCarriageLeg>();
            GetTheLastDirectShipment();
            Context.Direct.MainCarriageLegs = new List<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs.AddRange(mainCarriageLegs);
        }

        [When(@"User Update Shipment With Invalid Future ATA")]
        public void WhenUpdateShipmentWithInvalidFutureATA()
        {
            DateTime FutureDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(1);

            Context.Direct.MainCarriageLegs.Last().ATA = FutureDate;
            dynamic response = APICaller.CallPut<dynamic>(Context.Direct, "Direct", Context.User.Token, HttpStatusCode.BadRequest);

            ExceptionMessage = (response["ErrorMessage"] as string).Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
        }

        [Then(@"Update should not be done")]
        public void UpdateShouldNotBeDone()
        {
            ExceptionMessage.Should().Be("Can't set MainCarriageATA to future date");
        }

        [When(@"The User Updates Shipment With Invalid Future ATD")]
        public void WhenTheUserUpdatesShipmentWithInvalidFutureATD()
        {
            DateTime FutureDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(1);

            Context.Direct.MainCarriageLegs.Last().ATD = FutureDate;
            dynamic response = APICaller.CallPut<dynamic>(Context.Direct, "Direct", Context.User.Token, HttpStatusCode.BadRequest);

            ExceptionMessage = (response["ErrorMessage"] as string).Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
        }

        [Then(@"The excption massage that's related to this case is shown")]
        public void ThenTheExcptionMassageThatSRelatedToThisCaseIsShown()
        {
            ExceptionMessage.Should().Be("Can't set MainCarriageATD to future date");
            
        }

        [When(@"The User Updates Shipment With valid Future ETD,ATD,ETA and ATA")]
        public void WhenTheUserUpdatesShipmentWithValidFutureETDATDETAAndATA()
        {
            Context.Direct.MainCarriageLegs.First().ETD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(1);
            Context.Direct.MainCarriageLegs.First().ATD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(2);
            Context.Direct.MainCarriageLegs.First().ETA = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(3);
            Context.Direct.MainCarriageLegs.First().ATA = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(4);

            Context.Direct = APICaller.CallPut<Direct>(Context.Direct, "Direct", Context.User.Token);
        }

        [Then(@"The shipment is updated succesfully")]
        public void ThenTheShipmentIsUpdatedSuccesfully()
        {
            Context.Direct.Should().NotBeNull();
        }

        private void GetTheLastDirectShipment()
        {
            string directShipmentsRequestUrl = "ShipmentViews/GetByFilters?ForceCacheRefresh=false&GetAll=false&GetCount=false&PageIndex=0&PageSize=10" +
                "&Filter1Name=ShipmentLevelCode&Filter1Operator=equals&Filter1Value=D" +
                "&Filter3Name=TransportModeId&Filter3Operator=equals&Filter3Value=O" +
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