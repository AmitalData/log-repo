using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTrackingTests.Services;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CargoTrackingTests.Steps
{
    [Binding]
    public class BuildCargoTrackingDatabaseAfterAdjustingAShipmentSteps
    {
        private readonly ShipmentContext ShipmentContext;
        public BuildCargoTrackingDatabaseAfterAdjustingAShipmentSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }
       
        [When(@"Updating a shipments GrossWeight and building cargo tables")]
        public void WhenUpdatingAShipmentsGrossWeightAndBuildingCargoTables()
        {
            ApiResponse<ShipmentPM> response = UpdateShipmentGrossWeight(ShipmentContext.HouseShipment);
            CargoTrackingBuildService cargoTrackingBuildService = new CargoTrackingBuildService();
            string sourceConnectionString = "Data Source=.;Initial Catalog=Logitude2-5_Main;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            string destinationConnectionString = "Data Source=.;Initial Catalog=Logitude2-5_CargoTracking;Integrated Security=False;Persist Security Info=True;User ID=sa;Password=Saas256;MultipleActiveResultSets=True;Connect Timeout=60";
            cargoTrackingBuildService.BuildCargoTrackingTables(sourceConnectionString,destinationConnectionString);

        }

        

        [Then(@"the GrossWeight of the cargo tracking shipment with the same id will be updated")]
        public void ThenTheGrossWeightOfTheCargoTrackingShipmentWithTheSameIdWillBeUpdated()
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


    }
}
