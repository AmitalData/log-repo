using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using TechTalk.SpecFlow;

namespace Logitude.CrossDockTests.Hooks
{
    [Binding]
    public sealed class BeforeFeatureRun
    {
        [BeforeFeature("Pre-Prepare")]
        public static void SetUpPrepareDataBeforeFeatureRun()
        {
            CreateCrossDock();
        }

        private static void CreateCrossDock()
        {
            try
            {
                CrossDockPM crossDockPM = GetValidaCrossDockPM();
                ApiResponse<CrossDockPM> response = APICaller.CallPost<CrossDockPM>(crossDockPM, Urls.CrossDockController, UserTenant.Token);
                CrossDockDataMap(response.Data);
            }
            catch (Exception)
            {
                throw new InvalidOperationException("Failed Creating cross dock Before Feature Run");
            }
        }

        private static void CrossDockDataMap(CrossDockPM quote)
        {
            CrossDockData.Id = quote.Id;
        }

        private static CrossDockPM GetValidaCrossDockPM()
        {
            return new CrossDockBuilder().WithDefualtValues()
             .DirectionId("E")
             .TransportModeId("A")
             .ChargeableWeightUnitCode("KG")
             .GrossWeightUnitCode("KG")
             .DimensionsUnitCode("Cm")
             .VolumeUnitCode("CBM")
             .StatusCode("CREA")
             .Build();
        }
    }
}
