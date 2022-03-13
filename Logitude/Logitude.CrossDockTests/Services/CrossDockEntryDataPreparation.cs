using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
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
                throw new InvalidOperationException("Failed Creating cross dock Before Feature Run :" + e.InnerException);
            }
        }

        private CrossDockEntryPM GetValidCrossDockPM()
        {
            return new CrossDockEntryBuilder()
                 .WithDefualtValues()
                 .DirectionId("E")
                 .TransportModeId("A")
                 .ChargeableWeightUnitCode("KG")
                 .GrossWeightUnitCode("KG")
                 .DimensionsUnitCode("Cm")
                 .VolumeUnitCode("CBM")
                 .StatusCode("CREA")
                 .WarehouseEntryPackage(GetWarehouseEntryPackage())
                 .Build();
        }

        private WarehouseEntryPackagePM GetWarehouseEntryPackage()
        {
            return new WarehouseEntryPackageBuilder()
                         .WithDefualtValues()
                         .Quantity(3)
                         .Length(10)
                         .Width(20)
                         .Build();
        }

        private void CrossDockDataMap(CrossDockEntryPM crossDockEntry)
        {
            CrossDockData.CrossDockEntryId = crossDockEntry.Id;
            CrossDockData.WarehouseEntryPackages = crossDockEntry.WarehouseEntryPackages;
        }


    }
}
