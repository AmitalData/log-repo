using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenant;
using Logitude.Base.Services;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class GETShipmentDirectExportAirSteps
    {
        protected readonly ShipmentContext ShipmentContext;
        protected ShipmentPM Response;

        public GETShipmentDirectExportAirSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        [When(@"get shipment with shipmentnumber")]
        public void WhenGetShipmentWithShipmentnumber()
        {
            ShipmentContext.DirectShipment = GetDirectShipment();
            ApiQueryFilters apiQueryFilters = BuildApiQueryFilters(ShipmentContext.DirectShipment.ShipmentNumber);
            ApiResponse<IEnumerable<ShipmentPM>> response = APICaller.CallGetByFilters<IEnumerable<ShipmentPM>>(Urls.ShipmentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            Response = response.Data?.FirstOrDefault();

        }

        [Then(@"shipment should be avaliable")]
        public void ThenShipmentShouldBeAvaliable()
        {
            Response.Id.Should().NotBeNull();
        }


        private ShipmentPM GetDirectShipment()
        {
            ApiQueryFilters apiQueryFilters = new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("ShipmentLevelCode")
                .Filter1Operator("equals")
                .Filter1Value("D")
                .Filter2Name("TransportModeId")
                .Filter2Operator("equals")
                .Filter2Value("A")
                .Build(); 

            ApiResponse<IEnumerable<ShipmentPM>> response = APICaller.CallGetByFilters<IEnumerable<ShipmentPM>>(Urls.ShipmentViewsGetByFilters, UserTenant.Token, apiQueryFilters);
            return response.Data?.FirstOrDefault();
        }

        private ApiQueryFilters BuildApiQueryFilters(string shipmentNumber)
        {
            return new ApiQueryFiltersBuilder().WithDefualtValues()
                .Filter1Name("SearchFields")
                .Filter1Operator("Contains")
                .Filter1Value(shipmentNumber)
                .Build(); 
        }

    }
}
