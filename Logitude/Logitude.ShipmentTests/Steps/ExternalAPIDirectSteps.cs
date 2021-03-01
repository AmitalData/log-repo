using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
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

        #region Step Region

        #region Create direct shipment with main carriage leg steps

        [Given(@"a direct shipment with the following fields")]
        public void GivenADirectShipmentWithTheFollowingFields(Table directShipmentTable)
        {
            Context.Direct = CreateDirectInstance(directShipmentTable);
        }

        [Given(@"a main carriage leg")]
        public void GivenAMainCarriageLeg(Table mainCarriageLegsTable)
        {
            AddMainCarriageLegsToDirectShipment(Context.Direct, mainCarriageLegsTable);
        }

        [When(@"create shipment")]
        public void WhenCreateShipment()
        {
            ApiResponse<Direct> response = APICaller.CallPost<Direct>(Context.Direct, Urls.DirectController, UserTenant.Token);
            Context.Direct = response.Data;
        }

        [Then(@"shipment should create successfully")]
        public void ThenShipmentShouldCreateSuccessfully()
        {
            Context.Direct.Should().NotBeNull();
        }
        #endregion

        #region Update ATA to future date steps
        [When(@"update ATA to future date")]
        public void WhenUpdateATAToFutureDate()
        {
            Context.Direct.MainCarriageLegs.First().ATA = GetDateBasedOnCurrentDate(1, 0, 0);
            Context.act = () => APICaller.CallPut<Direct>(Context.Direct, Urls.DirectController, UserTenant.Token);
        }

        [Then(@"should receive error message say cannot set main carriage ATA to future date")]
        public void ThenShouldReceiveErrorMessageSayCannotSetMainCarriageATAToFutureDate()
        {
            Context.act.Should().ThrowExactly<AggregateException>().And.InnerExceptions[0].Message.Contains("Can't set MainCarriageATA to future date");
        }
        #endregion

        #region Update ATD to future date steps
        [When(@"update ATD to future date")]
        public void WhenUpdateATDToFutureDate()
        {
            Context.Direct.MainCarriageLegs.First().ATD = GetDateBasedOnCurrentDate(1, 0, 0);
            Context.act = () => APICaller.CallPut<Direct>(Context.Direct, Urls.DirectController, UserTenant.Token);
        }

        [Then(@"should receive error message say cannot set main carriage ATD to future date")]
        public void ThenShouldReceiveErrorMessageSayCannotSetMainCarriageATDToFutureDate()
        {
            Context.act.Should().ThrowExactly<AggregateException>().And.InnerExceptions[0].Message.Contains("Can't set MainCarriageATD to future date");
        }
        #endregion

        #region Update ETD, ATD, ETA, and ATA to vaild dates steps
        [When(@"update ETD, ATD, ETA, and ATA to vaild dates")]
        public void WhenUpdateETDATDETAAndATAToVaildDates()
        {
            FillVaildDatesInMainCarriageLegs();
            ApiResponse<Direct> response = APICaller.CallPut<Direct>(Context.Direct, Urls.DirectController, UserTenant.Token);
            Context.Direct = response.Data;
        }

        [Then(@"shipment should update successfully")]
        public void ThenShipmentShouldUpdateSuccessfully()
        {
            Context.Direct.Should().NotBeNull();
        }
        #endregion

        #endregion

        #region Private Function Region
        private void AddMainCarriageLegsToDirectShipment(Direct direct,Table mainCarriageLegsTable)
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

            direct.MainCarriageLegs = new List<MainCarriageLeg>();
            direct.MainCarriageLegs.AddRange(mainCarriageLegsList);
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
        #endregion

        #region Build Models Region
        private Direct CreateDirectInstance(Table directShipmentTable)
        {
            dynamic directShipment = directShipmentTable.CreateDynamicInstance();

            return new DirectBuilder().WithDefualtValues()
                .Direction((string)directShipment.Direction.ToString())
                .TransportMode((string)directShipment.TransportMode.ToString())
                .ShipmentType((string)directShipment.ShipmentType.ToString())
                .ShipperReference1((string)directShipment.ShipperReference1.ToString())
                .ShipperReference2((string)directShipment.ShipperReference2.ToString())
                .GrossWeightUnit((string)directShipment.GrossWeightUnit.ToString())
                .ChargeableWeightUnit((string)directShipment.ChargeableWeightUnit.ToString())
                .VolumeUnit((string)directShipment.VolumeUnit.ToString())
                .Incoterm((string)directShipment.Incoterm.ToString())
                .MainCarriageCarrier((string)directShipment.MainCarriageCarrier.ToString())
                .MainCarriageATD(DateTime.Now.Date)
                .Build();
        }
        #endregion
    }
}