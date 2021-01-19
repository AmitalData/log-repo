using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class ExternalAPIDirectSteps
    {
        private readonly ExternalAPIDirectContext Context;

        public ExternalAPIDirectSteps(ExternalAPIDirectContext context)
        {
            Context = context;
        }

        [Given(@"Direct shipment with the following properties")]
        public void GivenDirectShipmentWithTheFollowingProperties(Table directShipmentTable)
        {
            BuildNewDirectShipment(directShipmentTable);
        }

        [Given(@"List of main carriage legs")]
        public void GivenListOfMainCarriageLegs(Table mainCarriageLegsTable)
        {
            AddMainCarriageLegsToDirectShipment(mainCarriageLegsTable);
        }
        
        [When(@"Create direct shipment using external API")]
        public void WhenCreateDirectShipmentUsingExternalAPI()
        {
            Direct createdDirectShipment = APICaller.CallPost<Direct>(Context.Direct, "Direct", UserTenant.Token);
            Context.Direct.Id = createdDirectShipment?.Id;
        }

        [Then(@"The direct shipment should be created successfully")]
        public void ThenTheDirectShipmentShouldBeCreatedSuccessfully()
        {
            Context.Direct.Id.Should().NotBeNull();
        }

        [When(@"Update main carriage leg ATA to future date")]
        public void WhenUpdateMainCarriageLegATAToFutureDate()
        {
            DateTime futureDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(1);
            Context.Direct.MainCarriageLegs.First().ATA = futureDate;

            dynamic response = APICaller.CallPut<dynamic>(Context.Direct, "Direct", UserTenant.Token, HttpStatusCode.BadRequest);

            Context.ExceptionMessage = (response["ErrorMessage"] as string).Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
        }

        [Then(@"Error message \(cannot set main carriage ATA to future date\) should received")]
        public void ThenErrorMessageCannotSetMainCarriageATAToFutureDateShouldReceived()
        {
            Context.ExceptionMessage.Should().Be("Can't set MainCarriageATA to future date");
        }

        [When(@"Update main carriage leg ATD to future date")]
        public void WhenUpdateMainCarriageLegATDToFutureDate()
        {
            DateTime futureDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(1);
            Context.Direct.MainCarriageLegs.First().ATD = futureDate;

            dynamic response = APICaller.CallPut<dynamic>(Context.Direct, "Direct", UserTenant.Token, HttpStatusCode.BadRequest);

            Context.ExceptionMessage = (response["ErrorMessage"] as string).Replace("\r", string.Empty).Replace("\n", string.Empty).Trim();
        }

        [Then(@"Error message \(cannot set main carriage ATD to future date\) should received")]
        public void ThenErrorMessageCannotSetMainCarriageATDToFutureDateShouldReceived()
        {
            Context.ExceptionMessage.Should().Be("Can't set MainCarriageATD to future date");
        }

        [When(@"Update main carriage leg ETD,ATD,ETA and ATA to valid date")]
        public void WhenUpdateMainCarriageLegETDATDETAAndATAToValidDate()
        {
            Context.Direct.MainCarriageLegs.First().ETD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(1);
            Context.Direct.MainCarriageLegs.First().ATD = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(2);
            Context.Direct.MainCarriageLegs.First().ETA = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(3);
            Context.Direct.MainCarriageLegs.First().ATA = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddMonths(-1).AddDays(4);
            //Context.Direct.NewConcurrencyGUID = Guid.NewGuid().ToString();
            Context.Direct = APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);
        }

        [Then(@"The shipment should updated succesfully")]
        public void ThenTheShipmentIsUpdatedSuccesfully()
        {
            Context.Direct.Should().NotBeNull();
        }



        private void BuildNewDirectShipment(Table directShipmentTable)
        {
            dynamic directShipment = directShipmentTable.CreateDynamicInstance();

            DirectBuilder directBuilder = new DirectBuilder();
            directBuilder.Agent((string)directShipment.Agent)
                .Direction((string)directShipment.Direction)
                .TransportMode((string)directShipment.TransportMode)
                .ShipmentType((string)directShipment.ShipmentType)
                .Shipper((string)directShipment.Shipper)
                .ShipperReference1((string)directShipment.ShipperReference1)
                .ShipperReference2((string)directShipment.ShipperReference2)
                .GrossWeightUnit((string)directShipment.GrossWeightUnit)
                .ChargeableWeightUnit((string)directShipment.ChargeableWeightUnit)
                .VolumeUnit((string)directShipment.VolumeUnit)
                .Incoterm((string)directShipment.Incoterm)
                .MainCarriageCarrier((string)directShipment.MainCarriageCarrier)
                .MainCarriageATD((DateTime)directShipment.MainCarriageATD);

            Context.Direct = directBuilder.Build();
        }

        private void AddMainCarriageLegsToDirectShipment(Table mainCarriageLegsTable)
        {
            IEnumerable<dynamic> mainCarriageLegs = mainCarriageLegsTable.CreateDynamicSet();

            List<MainCarriageLeg> mainCarriageLegsList = new List<MainCarriageLeg>();

            mainCarriageLegs.ToList().ForEach(mainCarriageLeg =>
            {
                MainCarriageLegBuilder mainCarriageLegBuilder = new MainCarriageLegBuilder();
                MainCarriageLeg newCarriageLeg = mainCarriageLegBuilder.LegIndex((int)mainCarriageLeg.LegIndex)
                .Carrier((string)mainCarriageLeg.Carrier)
                .FromPort((string)mainCarriageLeg.FromPort)
                .ToPort((string)mainCarriageLeg.ToPort)
                .Build();
                mainCarriageLegsList.Add(newCarriageLeg);
            });

            Context.Direct.MainCarriageLegs = new List<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs.AddRange(mainCarriageLegsList);
        }
    }
}