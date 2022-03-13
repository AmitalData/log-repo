using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using Logitude.Base.Models.Api;
using Logitude.Base.Models.Shared;
using Logitude.Base.Models.UserTenantPreparation;
using Logitude.Base.Services;
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
                throw new InvalidOperationException("Failed Creating release cross dock Before Feature Run " + e.InnerException);
            }
        }

        private CrossDockReleasePM GetValidCrossDockPM()
        {

            return new CrossDockReleaseBuilder()
             .WithDefualtValues()
             .ChargeableWeightUnitCode("KG")
             .GrossWeightUnitCode("KG")
             .DimensionsUnitCode("Cm")
             .VolumeUnitCode("CBM")
             .StatusCode("CREA")
             .WarehouseReleasePackages(new CrossDockReleaseServices().BuildPackages())
             .Build();
        }

        private void CrossDockDataMap(CrossDockReleasePM crossDockRelease)
        {
            CrossDockData.CrossDockReleaseId = crossDockRelease.Id;
        }

    }
}
