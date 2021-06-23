using Logitude.CrossDockTests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using FluentAssertions;
using Logitude.CrossDockTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CrossDockTests.Services
{
    public class CrossDockEntryServices
    {
        public CrossDockEntryPM CreateInstance(Table crossDockTable)
        {
            dynamic dataTable = crossDockTable.CreateDynamicInstance();

            return new CrossDockEntryBuilder().WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .ChargeableWeightUnitCode((string)dataTable.ChargeableWeightUnitCode)
                .GrossWeightUnitCode((string)dataTable.GrossWeightUnitCode)
                .DimensionsUnitCode((string)dataTable.DimensionsUnitCode)
                .VolumeUnitCode((string)dataTable.VolumeUnitCode)
                .StatusCode((string)dataTable.StatusCode)
                .Build();
        }

        public List<WarehouseEntryPackagePM> BuildPackages(Table packagesDetailsTable)
        {
            List<WarehouseEntryPackagePM> warehouseEntryPackages = new List<WarehouseEntryPackagePM>();
            packagesDetailsTable.CreateDynamicSet().ToList().ForEach(packagesDetail =>
            {
                warehouseEntryPackages.Add(
                    new WarehouseEntryPackageBuilder()
                    .WithDefualtValues()
                    .Quantity((int)packagesDetail.Quantity)
                    .Length((Convert.ToString(packagesDetail.Length)).Length == 0 ? null : (double?)packagesDetail.Length)
                    .Width((Convert.ToString(packagesDetail.Width)).Length == 0 ? null : (double?)packagesDetail.Width)
                    .Height((Convert.ToString(packagesDetail.Height)).Length == 0 ? null : (double?)packagesDetail.Height)
                    .Weight((Convert.ToString(packagesDetail.Weight)).Length == 0 ? null : (double?)packagesDetail.Weight)
                    .Build());
            });
            return new List<WarehouseEntryPackagePM>(warehouseEntryPackages);
        }

    

    }
}
