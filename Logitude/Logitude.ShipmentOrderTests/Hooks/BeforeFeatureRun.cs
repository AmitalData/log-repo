using Logitude.ShipmentOrderTests.Services;
using TechTalk.SpecFlow;

namespace Logitude.ShipmentOrderTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new ShipmentOrderDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-ShipmentOrder")]
        public static void SetUpPrepareDataBeforeFeatureRunShipmentOrder()
        {
            new ShipmentOrderDataPreparation().Prepar();
        }
    }
}
