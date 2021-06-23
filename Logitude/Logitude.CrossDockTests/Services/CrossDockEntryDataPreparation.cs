using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;

namespace Logitude.CrossDockTests.Services
{
    public class CrossDockEntryDataPreparation
    {

        public void Prepar()
        {
            try
            {
                ApiResponse<CrossDockEntryPM> response = APICaller.CallPost<CrossDockEntryPM>(GetValidCrossDockPM(), Urls.CrossDockController, UserTenant.Token);
                CrossDockDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating cross dock Before Feature Run");
            }
        }

        private static CrossDockEntryPM GetValidCrossDockPM()
        {
            return new CrossDockEntryBuilder().WithDefualtValues()
             .DirectionId("E")
             .TransportModeId("A")
             .ChargeableWeightUnitCode("KG")
             .GrossWeightUnitCode("KG")
             .DimensionsUnitCode("Cm")
             .VolumeUnitCode("CBM")
             .StatusCode("CREA")
             .WarehouseEntryPackage(new WarehouseEntryPackageBuilder()
                    .WithDefualtValues()
                    .Quantity(3)
                    .Length(10)
                    .Width(20)
                    .Build())
             .Build();
        }

        private static void CrossDockDataMap(CrossDockEntryPM crossDockEntry)
        {
            CrossDockData.Id = crossDockEntry.Id;
            CrossDockData.WarehouseEntryPackages = crossDockEntry.WarehouseEntryPackages;
        }


    }
}
