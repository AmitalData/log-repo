using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Test.Base.Extensions;
using Logitude.Test.Base.Models.Login;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class ExternalAPIDirectSteps
    {
        protected ExternalAPIDirectContext Context;
        private Exception exp1; 

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


      //ATA

        [Given(@"User adding main carriage legs to last direct shipment")]
        public void UserAddingMainCarriageLegsToLastDirectShipment(Table mainCarriageLegsTable)
        {
            IEnumerable<MainCarriageLeg> mainCarriageLegs = mainCarriageLegsTable.CreateComplexSet<MainCarriageLeg>();
            GetTheLastDirectShipment();
            Context.Direct.MainCarriageLegs = new List<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs.AddRange(mainCarriageLegs);
        }

        [When(@"User Update Shipment With Invalid Future ATA")]
        public void WhenUpdateShipmentWithInvalidFutureATA()
        {
            Context.Direct.MainCarriageLegs.Last().ATA = new DateTime(2021, 10, 4);

            //shipmentPM = await service.UpdateDirect(shipmentPM, false);

            //Assert.IsTrue(service.HasException == true);
            //Assert.IsTrue(service.ExceptionMessage == "Can't set MainCarriageATA to future date");
            //shipmentPM.MainCarriageLegs.First().ATA = null;
        }

        [Then(@"Update should not be done")]
        public void UpdateShouldNotBeDone()
        {
            ScenarioContext.Current.Pending();
        }


      
   
        private void GetTheLastDirectShipment()
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