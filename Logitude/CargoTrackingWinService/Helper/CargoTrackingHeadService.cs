
using Logitude.CargoTracking.BL.CargoTrackingServices.HelperClasses;
using Logitude.CargoTracking.BL.CargoTrackingServices.Services;
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
                            cargoTrackingMainService.CheckAndUpdateWaterMark(destinationConnectionString,sourceConnectionString);
                            AddAllTablesToThread(cargoTrackingMainService.FillCargoTableList());
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

        private void UpdateCargoDataBase(CargoTable table)
        {
            //cargoTrackingMainService.UpdateLineByLine(new CargoArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString });
            cargoTrackingMainService.UpdateCTDataBase(new CargoArgs() { Table = table, SourceConnectionString = sourceConnectionString, DestinationConnectionString = destinationConnectionString },1000);
        }


        private void AddAllTablesToThread(List<CargoTable> CargoTableLists)
        {
            foreach (CargoTable table in CargoTableLists)
            {

                UpdateCargoDataBase(table);

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
