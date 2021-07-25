using Logitude.CrossDockTests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using Logitude.CrossDockTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CrossDockTests.Services
{
    public class CrossDockEntryServices
    {
        public CrossDockEntryPM CreateInstance(Table crossDockEntryTable)
        {
            dynamic dataTable = crossDockEntryTable.CreateDynamicInstance();

            return new CrossDockEntryBuilder()
                .WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .ChargeableWeightUnitCode((string)dataTable.ChargeableWeightUnitCode)
                .GrossWeightUnitCode((string)dataTable.GrossWeightUnitCode)
                .DimensionsUnitCode((string)dataTable.DimensionsUnitCode)
                .VolumeUnitCode((string)dataTable.VolumeUnitCode)
                .StatusCode((string)dataTable.StatusCode)
                .Build();
        }

        public List<WarehouseEntryPackagePM> BuildPackages(Table warehouseEntryPackageTable)
        {
            List<WarehouseEntryPackagePM> warehouseEntryPackages = new List<WarehouseEntryPackagePM>();
            warehouseEntryPackageTable.CreateDynamicSet().ToList().ForEach(warehouseEntryPackageDynamic =>
            {
                warehouseEntryPackages.Add(GetWarehouseEntryPackage(warehouseEntryPackageDynamic));
            });
            return new List<WarehouseEntryPackagePM>(warehouseEntryPackages);
        }


        private WarehouseEntryPackagePM GetWarehouseEntryPackage(dynamic warehouseEntryPackageDynamic)
        {

            return new WarehouseEntryPackageBuilder()
             .WithDefualtValues()
             .Quantity((int)warehouseEntryPackageDynamic.Quantity)
             .Length((Convert.ToString(warehouseEntryPackageDynamic.Length)).Length == 0 ? null : (double?)warehouseEntryPackageDynamic.Length)
             .Width((Convert.ToString(warehouseEntryPackageDynamic.Width)).Length == 0 ? null : (double?)warehouseEntryPackageDynamic.Width)
             .Height((Convert.ToString(warehouseEntryPackageDynamic.Height)).Length == 0 ? null : (double?)warehouseEntryPackageDynamic.Height)
             .Weight((Convert.ToString(warehouseEntryPackageDynamic.Weight)).Length == 0 ? null : (double?)warehouseEntryPackageDynamic.Weight)
             .Build();
        }
    }
}
