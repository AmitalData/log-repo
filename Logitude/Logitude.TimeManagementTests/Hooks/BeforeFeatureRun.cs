using Logitude.TimeManagementTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.TimeManagementTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetupPrepareDataBeforeFeatureRun()
        {
            new DataEntryDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-DataEntry")]
        public static void SetupPrepareDataBeforeFeatureRunDataEntry()
        {
            new DataEntryDataPreparation().Prepar();
        }
    }
}
