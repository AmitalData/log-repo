
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
    public class CargoTrackingHeadService
    {
        string sourceConnectionString = string.Empty;
        string destinationConnectionString = string.Empty;

        CargoTrackingServiceHelper cargoTrackingServiceHelper;
        CargoTrackingMainService cargoTrackingMainService;

        public CargoTrackingHeadService()
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
                            ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString,sourceConnectionString);
                            bool IsFromBuild = AddAllTablesToThread(CargoTrackingTableList.FillCargoTableList());
                            ApplicationInfo.UpdateCounter++;
                            SetIncrementalRecordData(IsFromBuild);
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

         private void InitializeIncrementalRecordData()
        {
            if (ApplicationInfo.UpdateCounter == 0)
            {
                ApplicationInfo.StartDate = TenantServerConfigration.GetCurrentDateTime(0);
                ApplicationInfo.Ports = 0;
                ApplicationInfo.Shipments = 0;
                ApplicationInfo.TransportModes = 0;
                ApplicationInfo.Cards = 0;
                ApplicationInfo.Countries = 0;
                ApplicationInfo.ShipmentComputedFields = 0;
                ApplicationInfo.ShipmentMasterDatas = 0;
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
            RecordUpdated RecordUpdatedNumber = new RecordUpdated();
            foreach (CargoTrackingTable table in cargoTableLists)
            {
                try
                {
                    RecordUpdatedNumber = UpdateCargoDataBase(table);
                }
                catch(Exception exception)
                {
                    SetIncrementalErrorLog(exception, table);


                }
                
                UpdaeNumberOfRecordsUpdated(table.CargoTracking_TableName, RecordUpdatedNumber.NumberOfRecordUpdated);

            }

            return RecordUpdatedNumber.IsFromBuild;

        }


        private void SetIncrementalErrorLog(Exception exception, CargoTrackingTable table)
        {
            string ErrorsLog = "Table Name: " + table.CargoTracking_TableName + Environment.NewLine + "Erros: " + exception.Message + Environment.NewLine + "Stack Trace: " + exception.StackTrace;
            if (!ApplicationInfo.ErrorLogs.Contains(ErrorsLog))
            {
                ApplicationInfo.ErrorLogs += ErrorsLog;
            }
            if (ApplicationInfo.ErrorLogs.Length > 4000)
            {
                ApplicationInfo.ErrorLogs.Substring(0, 4000);
            }
        }

        private void UpdaeNumberOfRecordsUpdated(string tableName, int recordUpdatedNumber)
        {
            switch (tableName)
            {
                case "CargoTrackingShipments":
                    {
                        ApplicationInfo.Shipments+= recordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingCards":
                    {
                        ApplicationInfo.Cards+= recordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingPorts":
                    {
                        ApplicationInfo.Ports+= recordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingCountries":
                    {
                        ApplicationInfo.Countries+= recordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingTransportModes":
                    {
                        ApplicationInfo.TransportModes+= recordUpdatedNumber;
                        break;
                    }

                case "CargoTrackingShipmentComputeds":
                    {
                        ApplicationInfo.ShipmentComputedFields += recordUpdatedNumber;
                        break;
                    }

                case "CargoTrackingShipmentMasters":
                    {
                        ApplicationInfo.ShipmentMasterDatas += recordUpdatedNumber;
                        break;
                    }

            }
        }

        private void BuildConnectionString()
        {
            string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
            string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
            sourceConnectionString = cargoTrackingServiceHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            destinationConnectionString = cargoTrackingServiceHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
        }
 
    }

 


}
