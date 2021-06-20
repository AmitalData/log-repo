using Logitude.CrossDockTests.Models;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using FluentAssertions;
using Logitude.CrossDockTests.Models.Builders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.CrossDockTests.ExternalServices
{
    public class CrossDockExternalServices
    {
        public CrossDockPM CreateEntriesCrossInstance(Table DataTable)
        {
            dynamic dataTable = DataTable.CreateDynamicInstance();

            return new CrossDockBuilder().WithDefualtValues()
                .DirectionId((string)dataTable.Direction)
                .TransportModeId((string)dataTable.TransportMode)
                .ChargeableWeightUnitCode((string)dataTable.ChargeableWeightUnitCode)
                .GrossWeightUnitCode((string)dataTable.GrossWeightUnitCode)
                .DimensionsUnitCode((string)dataTable.DimensionsUnitCode)
                .VolumeUnitCode((string)dataTable.VolumeUnitCode)
                .StatusCode((string)dataTable.StatusCode)
                .Build();
        }

        internal CrossDockPM AddPackagesDetails(CrossDockPM crossDockPM, Table packagesDetailsTable)
        {
            IEnumerable<dynamic> packagesDetails = packagesDetailsTable.CreateDynamicSet();
            List<WarehouseEntryPackagePM> mainCarriageLegsList = new List<WarehouseEntryPackagePM>();
            packagesDetails.ToList().ForEach(packagesDetail =>
            {
                mainCarriageLegsList.Add(
                    new WarehouseEntryPackageBuilder()
                    .WithDefualtValues()
                    .Quantity((int)packagesDetail.Quantity)
                    .Length((Convert.ToString(packagesDetail.Length)).Length == 0 ? null : (int?)packagesDetail.Length)
                    .Width((Convert.ToString(packagesDetail.Width)).Length == 0 ? null : (int?)packagesDetail.Width)
                    .Height((Convert.ToString(packagesDetail.Height)).Length == 0 ? null : (int?)packagesDetail.Height)
                    .Weight((Convert.ToString(packagesDetail.Weight)).Length == 0 ? null : (int?)packagesDetail.Weight)
                    .Build());
            });
            crossDockPM.WarehouseEntryPackages = new List<WarehouseEntryPackagePM>(mainCarriageLegsList);
            return crossDockPM;
        }


    }
}
