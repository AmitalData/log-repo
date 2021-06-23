using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Test.Base.Models.Api;
using Logitude.Test.Base.Models.Shared;
using Logitude.Test.Base.Models.UserTenantPreparation;
using Logitude.Test.Base.Services;
using System;
using System.Collections.Generic;

namespace Logitude.CrossDockTests.Services
{
    public class CrossDockReleaseDataPreparation
    {
        public void Prepar()
        {
            try
            {
                ApiResponse<CrossDockReleasePM> response = APICaller.CallPost<CrossDockReleasePM>(GetValidCrossDockPM(), Urls.CrossReleaseController, UserTenant.Token);
                CrossDockDataMap(response.Data);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException("Failed Creating release cross dock Before Feature Run");
            }
        }

        private static CrossDockReleasePM GetValidCrossDockPM()
        {

            List<WarehouseReleasePackagePM> warehouseReleasePackagePMs = new List<WarehouseReleasePackagePM>();
            CrossDockData.WarehouseEntryPackages.ForEach(entryPackage =>
            {
                warehouseReleasePackagePMs.Add(
                    new WarehouseReleasePackageBuilder()
                    .WithDefualtValues()
                    .Quantity((int)entryPackage.Quantity)
                    .Weight(entryPackage.Weight)
                    .Width(entryPackage.Width)
                    .Height(entryPackage.Height)
                    .Length(entryPackage.Length)
                    .EntryPackageId(entryPackage.Id)
                    .Build());
            });

            return new CrossDockReleaseBuilder().WithDefualtValues()
             .ChargeableWeightUnitCode("KG")
             .GrossWeightUnitCode("KG")
             .DimensionsUnitCode("Cm")
             .VolumeUnitCode("CBM")
             .StatusCode("CREA")
             .WarehouseReleasePackages(warehouseReleasePackagePMs)
             .Build();
        }

        private static void CrossDockDataMap(CrossDockReleasePM crossDockRelease)
        {
            CrossDockReleaseData.Id = crossDockRelease.Id;
        }

    }
}
