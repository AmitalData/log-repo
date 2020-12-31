
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services.ServicesHelper;
using Simplog.Data.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
 

namespace CargoTrackingWinService.Helper
{
    public class CargoTrackingMainWinService
    {
        string sourceConnectionString = string.Empty;
        string destinationConnectionString = string.Empty;

        CargoTrackingServiceHelper cargoTrackingServiceHelper;
        CargoTrackingMainService cargoTrackingMainService;

        public CargoTrackingMainWinService()
        {
            cargoTrackingMainService = new CargoTrackingMainService();
            cargoTrackingServiceHelper = new CargoTrackingServiceHelper();
            BuildConnectionString();
        }

        public void UpdateCargoTracking()
        {
            while (true)
            {
                try
                {
                    if (!ApplicationInfo.RunCargoTrackingImmediately)
                    {
                        if (!cargoTrackingServiceHelper.CheckIsUpgradingSystem(sourceConnectionString))
                        {
                            InitializeIncrementalRecordData();
                            StartCargoTrackingIncrementalUpdate();
                            Thread.Sleep(ApplicationInfo.UpdateCargoTrackingSleepTime);
                        }
                        else Thread.Sleep(new TimeSpan(0, 5, 0));
                    }
                    else Thread.Sleep(new TimeSpan(0, 5, 0));
                }
                catch (Exception ex)
                {
                    Thread.Sleep(ApplicationInfo.UpdateCargoTrackingSleepTime);
                }
            }
        }

        private void StartCargoTrackingIncrementalUpdate()
        {
            ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString, sourceConnectionString);
            bool IsFromBuild = AddAllTablesToThread(CargoTrackingTableList.GetCargoTrackingTableList());
            ApplicationInfo.UpdateCounter++;
            SetIncrementalRecordData(IsFromBuild);
        }
        private void InitializeIncrementalRecordData()
        {
            if (ApplicationInfo.UpdateCounter == 0)
            {
                ApplicationInfo.StartDate = TenantServerConfigration.GetCurrentDateTime(0);
                ApplicationInfo.CargoTrackingRecordsUpdatedDictionary = new Dictionary<string, int>();
            }
        }

        private void SetIncrementalRecordData(bool isFromBuild)
        {
            if (ApplicationInfo.UpdateCounter == 10)
            {
                ApplicationInfo.UpdateCounter = 0;
                ApplicationInfo.EndDate = TenantServerConfigration.GetCurrentDateTime(0);
                if (!isFromBuild)
                {
                    CargoTrackingServiceHelper.AddRecordToCargoTrackingIncrementalStats(destinationConnectionString);

                }
            }
        }

        private RecordUpdated UpdateCargoDataBase(CargoTrackingTable table)
        {
            CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs()
            {
                BuildCargoArgs = new CargoTrackingArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString },
                NumberOfBulkPerTime = 1000,
                IsUpdateFromBuild = false,
                CargoTrackingArguments = null,
                IsUpdateAfterFinished = null,
            };
            RecordUpdated RecordUpdatedNumber = cargoTrackingMainService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs);
            return RecordUpdatedNumber;
        }


        private bool AddAllTablesToThread(List<CargoTrackingTable> cargoTableLists)
        {
            RecordUpdated recordUpdated = new RecordUpdated();
            foreach (CargoTrackingTable table in cargoTableLists)
            {
                try
                {
                    recordUpdated = UpdateCargoDataBase(table);
                    ApplicationInfo.ErrorLogs += recordUpdated.ErrorLogs;
                }
                catch(Exception exception)
                {
                    SetIncrementalErrorLog(exception, table);
                }
                UpdateNumberOfRecordsUpdated(table.DBTableName, recordUpdated.NumberOfRecordUpdated);
            }

            return recordUpdated.IsFromBuild;

        }


        private void SetIncrementalErrorLog(Exception exception, CargoTrackingTable table)
        {
            string ErrorsLog = "Table Name: " + table.CargoTracking_TableName + Environment.NewLine +
                                "Erros: " + exception.Message + Environment.NewLine + 
                                "Stack Trace: " + exception.StackTrace + Environment.NewLine ;

            if (!ApplicationInfo.ErrorLogs.Contains(ErrorsLog))
                ApplicationInfo.ErrorLogs += ErrorsLog;

            TrimIfOver4000();
        }


        private void TrimIfOver4000()
        {
            if (ApplicationInfo.ErrorLogs.Length > 4000)
            {
                ApplicationInfo.ErrorLogs = ApplicationInfo.ErrorLogs.Substring(0, 4000);
            }
        }

        private void UpdateNumberOfRecordsUpdated(string tableName, int recordUpdatedNumber)
        {
            if (ApplicationInfo.CargoTrackingRecordsUpdatedDictionary.ContainsKey(tableName))
            {
                ApplicationInfo.CargoTrackingRecordsUpdatedDictionary[tableName] = 
                              ApplicationInfo.CargoTrackingRecordsUpdatedDictionary[tableName] + recordUpdatedNumber;
            }
            else
            {
                ApplicationInfo.CargoTrackingRecordsUpdatedDictionary.Add(tableName, recordUpdatedNumber);
            }
  
        }

        private void BuildConnectionString()
        {
            string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
            string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
            ConnectionStringArguments sourceConnectionStringArguments = GetConnectionStringArguments(sourceConnectionArray);
            ConnectionStringArguments destinationConnectionStringArguments = GetConnectionStringArguments(destinationConnectionArray);
            sourceConnectionString = cargoTrackingServiceHelper.BuildConnectionString(sourceConnectionStringArguments);
            destinationConnectionString = cargoTrackingServiceHelper.BuildConnectionString(destinationConnectionStringArguments);
        }

        private ConnectionStringArguments GetConnectionStringArguments(string[] connectionArray)
        {
            ConnectionStringArguments connectionStringArguments = new ConnectionStringArguments()
            {
                Catalog = connectionArray[0],
                UserName = connectionArray[1],
                Password = connectionArray[2],
                Server = connectionArray[3],
            };

            return connectionStringArguments;
        }

    }

}
