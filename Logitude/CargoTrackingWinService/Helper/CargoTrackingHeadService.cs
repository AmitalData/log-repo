
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
                            if (ApplicationInfo.UpdateCounter == 0)
                            {
                                ApplicationInfo.StartDate = TenantServerConfigration.GetCurrentDateTime(0);
                                ApplicationInfo.Ports = 0;
                                ApplicationInfo.Shipments = 0;
                                ApplicationInfo.TransportModes = 0;
                                ApplicationInfo.Cards = 0;
                                ApplicationInfo.Countries = 0;
                            }
                            ServiceHelper.CheckAndUpdateWaterMark(destinationConnectionString,sourceConnectionString);
                            bool IsFromBuild = AddAllTablesToThread(CargoTrackingTableList.FillCargoTableList());
                            ApplicationInfo.UpdateCounter++;
                            if (ApplicationInfo.UpdateCounter==10)
                            {
                                ApplicationInfo.UpdateCounter = 0;
                                ApplicationInfo.EndDate = TenantServerConfigration.GetCurrentDateTime(0);
                                if (!IsFromBuild)
                                {
                                    CargoTrackingServiceHelper.AddRecordToCargoTrackingIncrementalStats(destinationConnectionString);

                                }
                            }
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

        private RecordUpdated UpdateCargoDataBase(CargoTable table)
        {
            CargoTrackingUpdateDataBaseArgs cargoTrackingDataBaseArgs = new CargoTrackingUpdateDataBaseArgs()
            {
                buildCargoArgs = new CargoArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString },
                NumberOfBulkPerTime = 1000,
                IsUpdateFromBuild = false,
                CargoTrackingArguments = null,
                IsUpdateAfterFinished = null,
            };
            RecordUpdated RecordUpdatedNumber = cargoTrackingMainService.UpdateCargoTrackingDataBase(cargoTrackingDataBaseArgs);
            return RecordUpdatedNumber;
        }


        private bool AddAllTablesToThread(List<CargoTable> CargoTableLists)
        {
            RecordUpdated RecordUpdatedNumber = new RecordUpdated();
            foreach (CargoTable table in CargoTableLists)
            {
                try
                {
                    RecordUpdatedNumber = UpdateCargoDataBase(table);
                }
                catch(Exception e)
                {
                    string ErrorsLog  ="Table Name: " +table.CT_TableName+Environment.NewLine +"Erros: "+ e.Message+ Environment.NewLine+ "Stack Trace: " + e.StackTrace;
                    if (!ApplicationInfo.ErrorLogs.Contains(ErrorsLog))
                    {
                        ApplicationInfo.ErrorLogs += ErrorsLog;
                    }
                    if (ApplicationInfo.ErrorLogs.Length > 4000)
                    {
                        ApplicationInfo.ErrorLogs.Substring(0, 4000);
                    }
                    
                }
                
                UpdaeNumberOfRecordsUpdated(table.CT_TableName, RecordUpdatedNumber.NumberOfRecordUpdated);

            }

            return RecordUpdatedNumber.IsFromBuild;

        }


        private void UpdaeNumberOfRecordsUpdated(string TableName, int RecordUpdatedNumber)
        {
            switch (TableName)
            {
                case "CargoTrackingShipments":
                    {
                        ApplicationInfo.Shipments+= RecordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingCards":
                    {
                        ApplicationInfo.Cards+= RecordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingPorts":
                    {
                        ApplicationInfo.Ports+= RecordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingCountries":
                    {
                        ApplicationInfo.Countries+= RecordUpdatedNumber;
                        break;
                    }
                case "CargoTrackingTransportModes":
                    {
                        ApplicationInfo.TransportModes+= RecordUpdatedNumber;
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
