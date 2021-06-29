using Logitude.CommonDataTests.Services;
using TechTalk.SpecFlow;

namespace Logitude.CommonDataTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare-Vessel")]
        public static void SetUpPrepareDataBeforeFeatureRunEntry()
        {
            new VesselDataPreparation().Prepar();
        }
    }
}
