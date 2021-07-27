using Logitude.Tariff.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace Logitude.Tariff.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {

        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            new TariffAirFreightCostDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Tariff-Air")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffAirFreightCost()
        {
            new TariffAirFreightCostDataPreparation().Prepar();
        }

    }
}
