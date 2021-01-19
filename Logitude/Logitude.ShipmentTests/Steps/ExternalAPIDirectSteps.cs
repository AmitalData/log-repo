using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
            APIResponse<Direct> response = APICaller.CallPost<Direct>(Context.Direct, "Direct", UserTenant.Token);
            Context.Direct.Id = response.Data?.Id;
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

            APIResponse<Direct> response = APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);

            Context.ExceptionMessage = response.ErrorMessage;
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

            APIResponse<Direct> response = APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);

            Context.ExceptionMessage = response.ErrorMessage;
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
            APIResponse<Direct> response = APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);
            Context.Direct = response.Data;
        }

        [Then(@"The shipment should updated succesfully")]
        public void ThenTheShipmentIsUpdatedSuccesfully()
        {
            Context.Direct.Should().NotBeNull();
        }


        private void BuildNewDirectShipment(Table directShipmentTable)
        {
            dynamic directShipment = directShipmentTable.CreateDynamicInstance();

            string agent = Convert.ToString(directShipment.Agent);
            string direction = Convert.ToString(directShipment.Direction);
            string transportMode = Convert.ToString(directShipment.TransportMode);
            string shipmentType = Convert.ToString(directShipment.ShipmentType);
            string shipper = Convert.ToString(directShipment.Shipper);
            string shipperReference1 = Convert.ToString(directShipment.ShipperReference1);
            string shipperReference2 = Convert.ToString(directShipment.ShipperReference2);
            string grossWeightUnit = Convert.ToString(directShipment.GrossWeightUnit);
            string chargeableWeightUnit = Convert.ToString(directShipment.ChargeableWeightUnit);
            string volumeUnit = Convert.ToString(directShipment.VolumeUnit);
            string incoterm = Convert.ToString(directShipment.Incoterm);
            string mainCarriageCarrier = Convert.ToString(directShipment.MainCarriageCarrier);
            DateTime mainCarriageATD = Convert.ToDateTime(directShipment.MainCarriageATD);

            DirectBuilder directBuilder = new DirectBuilder();
            directBuilder.Agent(agent)
                .Direction(direction)
                .TransportMode(transportMode)
                .ShipmentType(shipmentType)
                .Shipper(shipper)
                .ShipperReference1(shipperReference1)
                .ShipperReference2(shipperReference2)
                .GrossWeightUnit(grossWeightUnit)
                .ChargeableWeightUnit(chargeableWeightUnit)
                .VolumeUnit(volumeUnit)
                .Incoterm(incoterm)
                .MainCarriageCarrier(mainCarriageCarrier)
                .MainCarriageATD(mainCarriageATD);

            Context.Direct = directBuilder.Build();
        }

        private void AddMainCarriageLegsToDirectShipment(Table mainCarriageLegsTable)
        {
            IEnumerable<dynamic> mainCarriageLegs = mainCarriageLegsTable.CreateDynamicSet();

            List<MainCarriageLeg> mainCarriageLegsList = new List<MainCarriageLeg>();

            mainCarriageLegs.ToList().ForEach(mainCarriageLeg =>
            {
                int legIndex = Convert.ToInt32(mainCarriageLeg.LegIndex);
                string carrier = Convert.ToString(mainCarriageLeg.Carrier);
                string fromPort = Convert.ToString(mainCarriageLeg.FromPort);
                string toPort = Convert.ToString(mainCarriageLeg.ToPort);

                MainCarriageLegBuilder mainCarriageLegBuilder = new MainCarriageLegBuilder();
                MainCarriageLeg newCarriageLeg = mainCarriageLegBuilder.LegIndex(legIndex)
                .Carrier(carrier)
                .FromPort(fromPort)
                .ToPort(toPort)
                .Build();
                mainCarriageLegsList.Add(newCarriageLeg);
            });

            Context.Direct.MainCarriageLegs = new List<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs.AddRange(mainCarriageLegsList);
        }
    }
}