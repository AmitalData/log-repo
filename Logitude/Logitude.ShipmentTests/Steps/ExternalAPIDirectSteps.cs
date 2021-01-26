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
            ApiResponse<Direct> response = APICaller.CallPost<Direct>(Context.Direct, "Direct", UserTenant.Token);
            Context.Direct = response.Data;
        }

        [Then(@"The direct shipment should be created successfully")]
        public void ThenTheDirectShipmentShouldBeCreatedSuccessfully()
        {
            Context.Direct.Should().NotBeNull();
        }

        [When(@"Update main carriage leg ATA to future date")]
        public void WhenUpdateMainCarriageLegATAToFutureDate()
        {
            Context.Direct.MainCarriageLegs.First().ATA = GetDateBasedOnCurrentDate(1, 0, 0);
            Context.act = () => APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);
        }

        [Then(@"Error message \(cannot set main carriage ATA to future date\) should received")]
        public void ThenErrorMessageCannotSetMainCarriageATAToFutureDateShouldReceived()
        {
            Context.act.Should().ThrowExactly<Exception>()
                .Where(e => e.Message.Contains("Can't set MainCarriageATA to future date"));
        }

        [When(@"Update main carriage leg ATD to future date")]
        public void WhenUpdateMainCarriageLegATDToFutureDate()
        {
            Context.Direct.MainCarriageLegs.First().ATD = GetDateBasedOnCurrentDate(1, 0, 0);
            Context.act = () => APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);

        }

        [Then(@"Error message \(cannot set main carriage ATD to future date\) should received")]
        public void ThenErrorMessageCannotSetMainCarriageATDToFutureDateShouldReceived()
        {
            Context.act.Should().ThrowExactly<Exception>()
                .Where(e => e.Message.Contains("Can't set MainCarriageATD to future date"));
        }

        [When(@"Update main carriage leg ETD,ATD,ETA and ATA to valid date")]
        public void WhenUpdateMainCarriageLegETDATDETAAndATAToValidDate()
        {
            FillVaildDatesInMainCarriageLegs();
            ApiResponse<Direct> response = APICaller.CallPut<Direct>(Context.Direct, "Direct", UserTenant.Token);
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
 

 
            DirectBuilder directBuilder = new DirectBuilder();
            directBuilder.Agent((string)directShipment.Agent.ToString())
                .Direction((string)directShipment.Direction.ToString())
                .TransportMode((string)directShipment.TransportMode.ToString())
                .ShipmentType((string)directShipment.ShipmentType.ToString())
                .Shipper((string)directShipment.Shipper.ToString())
                .ShipperReference1((string)directShipment.ShipperReference1.ToString())
                .ShipperReference2((string)directShipment.ShipperReference2.ToString())
                .GrossWeightUnit((string)directShipment.GrossWeightUnit.ToString())
                .ChargeableWeightUnit((string)directShipment.ChargeableWeightUnit.ToString())
                .VolumeUnit((string)directShipment.VolumeUnit.ToString())
                .Incoterm((string)directShipment.Incoterm.ToString())
                .MainCarriageCarrier((string)directShipment.MainCarriageCarrier.ToString())
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
                MainCarriageLeg newMainCarriageLeg = mainCarriageLegBuilder.LegIndex((int)mainCarriageLeg.LegIndex)
                .Carrier((string)mainCarriageLeg.Carrier.ToString())
                .FromPort((string)mainCarriageLeg.FromPort.ToString())
                .ToPort((string)mainCarriageLeg.ToPort.ToString())
                .Build();
                mainCarriageLegsList.Add(newMainCarriageLeg);
            });

            Context.Direct.MainCarriageLegs = new List<MainCarriageLeg>();
            Context.Direct.MainCarriageLegs.AddRange(mainCarriageLegsList);
        }

        private void FillVaildDatesInMainCarriageLegs()
        {
            Context.Direct.MainCarriageLegs.First().ETD = GetDateBasedOnCurrentDate(0, -1, 1);
            Context.Direct.MainCarriageLegs.First().ATD = GetDateBasedOnCurrentDate(0, -1, 2);
            Context.Direct.MainCarriageLegs.First().ETA = GetDateBasedOnCurrentDate(0, -1, 3);
            Context.Direct.MainCarriageLegs.First().ATA = GetDateBasedOnCurrentDate(0, -1, 4);
        }

        private DateTime GetDateBasedOnCurrentDate(int numberOfYearsToBeAdded, int numberOfMonthesToBeAdded, int numberOfDaysToBeAdded)
        {
            return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(numberOfYearsToBeAdded).AddMonths(numberOfMonthesToBeAdded).AddDays(numberOfDaysToBeAdded);
        }
    }
}