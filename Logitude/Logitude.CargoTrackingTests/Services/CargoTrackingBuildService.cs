using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Logitude.CargoTracking.BL.CoreBL.Batch;
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
        CargoTrackingXMLParameters cargoTrackingXMLParameters;
        CargoTrackingMainService cargoTrackingMainService;
        public void BuildCargoTrackingTables(string sourceConnectionString,string destinationConnectionString)
        {
            //UpdateTenantAsIncrementalBuilding();
            cargoTrackingXMLParameters = new CargoTrackingXMLParameters()
            {
                FromDate = new DateTime(2021, 01, 01),
                ToDate = DateTime.Now,
                Tenant = 1,
            };
            this.destinationConnectionString = destinationConnectionString;
            this.sourceConnectionString = sourceConnectionString;
            cargoTrackingMainService = new CargoTrackingMainService();
            ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString, sourceConnectionString);
            BuildCargoTrackingDatabaseForCargoTrackingTables(CargoTrackingTableList.GetCargoTrackingTableList());
            //UpdateIsIncrementalRunningFinished();
        }

        private void UpdateIsIncrementalRunningFinished()
        {
            TenantQuery tenantQuery = new TenantQuery(0);
            TenantPM tenantPM = tenantQuery.GetSinglePM(0);
            tenantPM.IsIncrementalBuildRunning = false;
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(0);
            TenantService tenantService = new TenantService(commonDataContext, 0);
            tenantService.Update(tenantPM);
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
        private void BuildCargoTrackingDatabaseForCargoTrackingTables(List<CargoTrackingTable> CargoTableLists)
        {
            foreach (CargoTrackingTable table in CargoTableLists)
            {
                BuildTableForCargoTracking(table);
            }
        }

        private void BuildTableForCargoTracking(CargoTrackingTable table)
        {
            CargoTrackingArguments CargoTrackingArgs = new CargoTrackingArguments
            {
                FromDate = cargoTrackingXMLParameters.FromDate,
                Tenant = cargoTrackingXMLParameters.Tenant,
                ToDate = cargoTrackingXMLParameters.ToDate,
                ThreadNumber = 50,
                FormTableName = null,
            };
            CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs()
            {
                BuildCargoArgs = new CargoTrackingArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString },
                NumberOfBulkPerTime = 1000,
                IsUpdateAfterFinished = null,
                CargoTrackingArguments = CargoTrackingArgs,
                IsUpdateFromBuild = true,
            };
            cargoTrackingMainService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs);
        }
    }
}
