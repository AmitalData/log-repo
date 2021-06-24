using Logitude.CrossDockTests.Models;
using Logitude.CrossDockTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Logitude.CrossDockTests.Services
{
    public class CrossDockReleaseServices
    {
        public CrossDockReleasePM CreateInstance(Table crossDockReleaseTable)
        {
            dynamic dataTable = crossDockReleaseTable.CreateDynamicInstance();

            return new CrossDockReleaseBuilder()
                .WithDefualtValues()
                .ChargeableWeightUnitCode((string)dataTable.ChargeableWeightUnitCode)
                .GrossWeightUnitCode((string)dataTable.GrossWeightUnitCode)
                .DimensionsUnitCode((string)dataTable.DimensionsUnitCode)
                .VolumeUnitCode((string)dataTable.VolumeUnitCode)
                .StatusCode((string)dataTable.StatusCode)
                .Build();
        }

        public List<WarehouseReleasePackagePM> BuildPackages()
        {
            List<WarehouseReleasePackagePM> warehouseReleasePackagePMs = new List<WarehouseReleasePackagePM>();
            CrossDockData.WarehouseEntryPackages.ForEach(warehouseEntryPackage =>
            {
                warehouseReleasePackagePMs.Add(WarehouseReleasePackagesFromEntryPrepar(warehouseEntryPackage));
            });
            return warehouseReleasePackagePMs;

        }
        private WarehouseReleasePackagePM WarehouseReleasePackagesFromEntryPrepar(WarehouseEntryPackagePM warehouseEntryPackage)
        {

            return new WarehouseReleasePackageBuilder()
               .WithDefualtValues()
               .Quantity(warehouseEntryPackage.Quantity)
               .Weight(warehouseEntryPackage.Weight)
               .Width(warehouseEntryPackage.Width)
               .Height(warehouseEntryPackage.Height)
               .Length(warehouseEntryPackage.Length)
               .EntryPackageId(warehouseEntryPackage.Id)
               .Build();
        }
   


    }
}
