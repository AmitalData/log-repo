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
        public CrossDockReleasePM CreateInstance(Table crossDockTable)
        {
            dynamic dataTable = crossDockTable.CreateDynamicInstance();

            return new CrossDockReleaseBuilder().WithDefualtValues()
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
            return warehouseReleasePackagePMs;

        }

    }
}
