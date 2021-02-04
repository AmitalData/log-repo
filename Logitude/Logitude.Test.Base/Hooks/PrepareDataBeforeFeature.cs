using Logitude.Test.Base.Models;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TechTalk.SpecFlow;

namespace Logitude.Test.Base.Hooks
{
    [Binding]
    public class PrepareDataBeforeFeature
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeTestRun()
        {
            SetupLocationPreparationVariables();
            SetupPartnerPreparationVariables();
        }

        private static void SetupLocationPreparationVariables()
        {
            LocationsVariables locationsVariables = DataPreparation.GetLocationsVariables();
            LocationsDataMap(locationsVariables);
        }

        private static void SetupPartnerPreparationVariables()
        {
            ApiResponse<PartnersVariables> partnersVariablesResponse = APICaller.CallGet<PartnersVariables>(Urls.IntegrationTestGetBasePartners, UserTenant.Token);
            PartnersDataMap(partnersVariablesResponse.Data);
        }

        private static void LocationsDataMap(LocationsVariables locationsVariables)
        {
            LocationsData.PortLHRId = locationsVariables.PortLHRId;
            LocationsData.PortLASDomesticId = locationsVariables.PortLASDomesticId;
            LocationsData.PortMIADomesticId = locationsVariables.PortMIADomesticId;
            LocationsData.PortAirJFKId = locationsVariables.PortAirJFKId;
            LocationsData.PortOceanSOUId = locationsVariables.PortOceanSOUId;
            LocationsData.PortInlandNYCId = locationsVariables.PortInlandNYCId;
            LocationsData.PortLONId = locationsVariables.PortLONId;
            LocationsData.PortMANId = locationsVariables.PortMANId;
            LocationsData.CountryUSId = locationsVariables.CountryUSId;
            LocationsData.CountryGBId = locationsVariables.CountryGBId;
            LocationsData.StateAKId = locationsVariables.StateAKId;
        }

        private static void PartnersDataMap(PartnersVariables partnersVariables)
        {
            PartnersData.VendorId = partnersVariables.VendorId;
            PartnersData.AgentId = partnersVariables.AgentId;
            PartnersData.CustomerId = partnersVariables.CustomerId;
            PartnersData.CustomAgentId = partnersVariables.CustomAgentId;
            PartnersData.ShippingAgentId = partnersVariables.ShippingAgentId;
            PartnersData.PotentialCustomerId = partnersVariables.PotentialCustomerId;
            PartnersData.TruckerId = partnersVariables.TruckerId;
            PartnersData.ShipperExport1 = partnersVariables.ShipperExport1;
            PartnersData.AirlineAAId = partnersVariables.AirlineAAId;
            PartnersData.AirlineBAId = partnersVariables.AirlineBAId;
            PartnersData.ShippingLineMSCUId = partnersVariables.ShippingLineMSCUId;
            PartnersData.ShippingLineMAEUId = partnersVariables.ShippingLineMAEUId;
            PartnersData.WarehouseId = partnersVariables.WarehouseId;
        }
    }
}