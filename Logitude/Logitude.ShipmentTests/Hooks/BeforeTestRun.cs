using Logitude.ShipmentTests.Models;
using Logitude.ShipmentTests.Services;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentTests.Hooks
{
    [Binding]
    public class BeforeTestRun
    {
        [BeforeTestRun]
        public static void SetupShipmentPreparationVariables()
        {
            ShipmentVariables shipmentVariables = DataPreparation.GetShipmentVariables();
            ShipmentDataMap(shipmentVariables);
        }

        private static void ShipmentDataMap(ShipmentVariables shipmentVariables)
        {
            ShipmentData.IncotermLDEId = shipmentVariables.IncotermLDEId;
            ShipmentData.VesselPTId = shipmentVariables.VesselPTId;
            ShipmentData.PackageTypePC1Id = shipmentVariables.PackageTypePC1Id;
            ShipmentData.PackageTypePC2Id = shipmentVariables.PackageTypePC2Id;
            ShipmentData.PackageTypePP1Id = shipmentVariables.PackageTypePP1Id;
            ShipmentData.PackageTypePP2Id = shipmentVariables.PackageTypePP2Id;
            ShipmentData.PaymentTermCashId = shipmentVariables.PaymentTermCashId;
            ShipmentData.VATTypeZeroId = shipmentVariables.VATTypeZeroId;
            ShipmentData.QuoteStageQTDRId = shipmentVariables.QuoteStageQTDRId;
            ShipmentData.MoveTypeMTAId = shipmentVariables.MoveTypeMTAId;
            ShipmentData.MoveTypeMTOId = shipmentVariables.MoveTypeMTOId;
        }
    }
}