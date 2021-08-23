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
            new TariffOceanLCLFreightCostDataPreparation().Prepar();
            new TariffAirSurchargeCostDataPreparation().Prepar();
            new TariffOceanFCLFreightCostDataPreparation().Prepar();
            new TariffOceanLCLSurchargeCostDataPreparation().Prepar();
            new TariffOceanFCLSurchargeCostDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Tariff-Air")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffAirFreightCost()
        {
            new TariffAirFreightCostDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Tariff-OceanLCL")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffOceanLCLFreightCost()
        {
            new TariffOceanLCLFreightCostDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Tariff-OceanLCL-Surcharge")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffOceanLCLSurchargeCost()
        {
            new TariffOceanLCLSurchargeCostDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Tariff-OceanFCL")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffOceanFCLFreightCost()
        {
            new TariffOceanFCLFreightCostDataPreparation().Prepar();
        }

        [BeforeFeature("@Pre-Prepare-Tariff-Air-Surcharge")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffAirSurchargeCost()
        {
            new TariffAirSurchargeCostDataPreparation().Prepar();
        }

        [BeforeFeature("Pre-Prepare-Tariff-OceanFCL-Surcharge")]
        public static void SetUpPrepareDataBeforeFeatureRunTariffOceanFCLSurchargeCost()
        {
            new TariffOceanFCLSurchargeCostDataPreparation().Prepar();
        }
    }
}
