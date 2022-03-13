using FluentAssertions;
using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.ShipmentTests.Steps
{
    [Binding]
    public class POSTShipmentDirectExportAirSteps
    {
        protected readonly ShipmentContext ShipmentContext;
        protected ShipmentPM Response;

        public POSTShipmentDirectExportAirSteps(ShipmentContext shipmentContext)
        {
            ShipmentContext = shipmentContext;
        }

        [Given(@"a direct shipment with the following properties")]
        public void GivenADirectShipmentWithTheFollowingProperties(Table table)
        {
            ShipmentContext.DirectShipment = CreateShipmentInstance(table);
        }

        [When(@"create direct shipment")]
        public void WhenCreateDirectShipment()
        {
            ApiResponse<ShipmentPM> response = APICaller.CallPost<ShipmentPM>(ShipmentContext.DirectShipment, Urls.ShipmentController, UserTenant.Token);
            ShipmentContext.DirectShipment = response?.Data;
        }

        [Then(@"the direct should create successfully")]
        public void ThenTheDirectShouldCreateSuccessfully()
        {
            ShipmentContext.DirectShipment.Id.Should().NotBeNull();
        }

        #region Build Models Region
        private ShipmentPM CreateShipmentInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new ShipmentBuilder().WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .ShipmentLevelCode((string)dataTable.ShipmentLevel)
                .OtherPrepaidCollectId((string)dataTable.OtherPrepaidCollect)
                .FreightPrepaidCollectId((string)dataTable.FreightPrepaidCollect)
                .MainCarriageToPortIdByCode((string)dataTable.MainCarriageToPort)
                .MainCarriageFromPortIdByCode((string)dataTable.MainCarriageFromPort)
                .Build();
        }
        #endregion

    }
}
