using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CargoTrackingTests.Services
{
    public class CargoTrackingBuildService
    {
        private string destinationConnectionString;
        private string sourceConnectionString;
        CargoTrackingArgs CargoTrackingArguments;
        public void BuildCargoTrackingTables()
        {
            UpdateTenantAsIncrementalBuilding();
            ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString, sourceConnectionString);
            //AddAllTablesToThread(CargoTrackingTableList.GetCargoTrackingTableList());
        }

        private void UpdateTenantAsIncrementalBuilding()
        {
            TenantQuery tenantQuery = new TenantQuery(0);
            TenantPM tenantPM = tenantQuery.GetSinglePM(0);
            tenantPM.IsIncrementalBuildRunning = true;
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
            TenantService tenantService = new TenantService(commonDataContext, 0);
            tenantService.Update(tenantPM);
        }
        private void AddAllTablesToThread(List<CargoTrackingTable> CargoTableLists)
        {
            foreach (CargoTrackingTable table in CargoTableLists)
            {

                UpdateCargoDataBase(table);

            }
        }

        private void UpdateCargoDataBase(CargoTrackingTable table)
        {
            //CargoTrackingArguments CargoTrackingArgs = new CargoTrackingArguments
            //{
            //    FromDate = CargoTrackingArguments.FromDate,
            //    Tenant = CargoTrackingArguments.Tenant,
            //    ToDate = CargoTrackingArguments.ToDate,
            //    ThreadNumber = 50,
            //    FormTableName = null,
            //};
            //CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs()
            //{
            //    BuildCargoArgs = new CargoTrackingServices.HelperClasses.CargoTrackingArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString },
            //    NumberOfBulkPerTime = 1000,
            //    IsUpdateAfterFinished = null,
            //    CargoTrackingArguments = CargoTrackingArgs,
            //    IsUpdateFromBuild = true,
            //};
            //cargoTrackingMainService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs);
        }
    }
}
