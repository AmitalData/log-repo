using Logitude.CrossDockTests.Services;
using TechTalk.SpecFlow;

namespace Logitude.CrossDockTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {

        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new CrossDockEntryDataPreparation().Prepar();
            new CrossDockReleaseDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Entry")]
        public static void SetUpPrepareDataBeforeFeatureRunEntry()
        {
            new CrossDockEntryDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-release")]
        public static void SetUpPrepareDataBeforeFeatureRunRelease()
        {
            new CrossDockEntryDataPreparation().Prepar();
        }

    }
}
