using FluentAssertions;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTrackingTests.Models;
using Logitude.CargoTrackingTests.Services;

using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Configuration;
using TechTalk.SpecFlow;

namespace Logitude.CargoTrackingTests.Steps
{
    [Binding]
    public class BuildCargoTrackingDatabaseAfterAdjustingOrAddingShipmentsSteps
    {
        private readonly ShipmentContext ShipmentContext;
        public BuildCargoTrackingDatabaseAfterAdjustingOrAddingShipmentsSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }
       
        [When(@"Updating a shipments GrossWeight and building cargo tables")]
        public void WhenUpdatingAShipmentsGrossWeightAndBuildingCargoTables()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentGrossWeight(ShipmentContext.DirectShipment);
            CargoTrackingBuildService cargoTrackingBuildService = new CargoTrackingBuildService();
            string sourceConnectionString = ConfigurationManager.ConnectionStrings["LogitudeConnectionString"].ConnectionString;//"Data Source=.;Initial Catalog=Logitude2-5_Main;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            string destinationConnectionString = ConfigurationManager.ConnectionStrings["CargoTrackingConnectionString"].ConnectionString; //"Data Source=.;Initial Catalog=Logitude2-5_CargoTracking;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            cargoTrackingBuildService.BuildCargoTrackingTables(sourceConnectionString,destinationConnectionString);
        }

      

        [Then(@"the GrossWeight of the cargo tracking shipment with the same id will be updated")]
        public void ThenTheGrossWeightOfTheCargoTrackingShipmentWithTheSameIdWillBeUpdated()
        {
            ApiResponse<CargoTrackingShipmentList> cargoTrackingShipmentResponse = 
                GetUpdatedCargoTrackingShipment(ShipmentContext.DirectShipment.SecurityKey,ShipmentContext.DirectShipment.Tenant);
            cargoTrackingShipmentResponse.Data.GrossWeight.Should().Be(200);
        }

        [When(@"building cargo tables")]
        public void WhenBuildingCargoTables()
        {
            ScenarioContext.Current.Pending();
        }


        [Then(@"a cargo tracking shipment with the same id will be created")]
        public void ThenACargoTrackingShipmentWithTheSameIdWillBeCreated()
        {
            ScenarioContext.Current.Pending();
        }


        private ApiResponse<ShipmentPM> UpdateShipmentGrossWeight(ShipmentPM shipment)
        {
            shipment.GrossWeight = GetNewGrossWeightValue(shipment.GrossWeight);
            return APICaller.CallPut<ShipmentPM>(shipment, Urls.ShipmentController, UserTenant.Token);
        }

        private double? GetNewGrossWeightValue(double? grossWeight)
        {
            return grossWeight + 100;
        }

        private ApiResponse<CargoTrackingShipmentList> GetUpdatedCargoTrackingShipment(string securityKey,int tenant)
        {
            return APICaller.CallGet<CargoTrackingShipmentList>(Urls.CargoTrackingShipmentGetSingleList(securityKey,tenant), UserTenant.Token);
        }


    }
}
