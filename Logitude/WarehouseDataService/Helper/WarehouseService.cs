using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WarehouseData.Helper;

namespace WarehouseDataService.Helper
{
    public class WarehouseService
    {
        string sourceConnectionString = string.Empty;
        string destinationConnectionString = string.Empty;

        WarehouseServiceHelper warehouseServiceHelper;
        WarehouseHelper warehouseHelper;

        public WarehouseService()
        {

            warehouseHelper = new WarehouseHelper("Service", ApplicationInfo.Mode);
            warehouseServiceHelper = new WarehouseServiceHelper();
            BuildConnectionString();
        }


        #region BuildWarehouseData

        public void BuildWarehouseData()
        {

            while (true)
            {
                try
                {
                    DWBuildTime dwBuildTime = GetDWBuildInfoTime();
                    if (dwBuildTime.IsUpdateDWNextRunTime) warehouseServiceHelper.UpdateDWNextRunTime(sourceConnectionString, dwBuildTime.DWNextRunTime);

                    if (dwBuildTime.DWRunTime != null || dwBuildTime.IsBuildNow)
                    {
                        if (!dwBuildTime.IsBuildNow)
                        {
                            TimeSpan span = (DateTime)dwBuildTime.DWRunTime - (DateTime.Now);
                            int sleepTime = (int)span.TotalMilliseconds;
                            Thread.Sleep(sleepTime);
                        }

                        StartBuildWarehouseData();
                        dwBuildTime.DWNextRunTime = warehouseServiceHelper.CalculateDWNextRunTime(DateTime.Now);
                        warehouseServiceHelper.UpdateDWNextRunTime(sourceConnectionString, dwBuildTime.DWNextRunTime);
                    }
                    else Thread.Sleep((10 * 60000));
                }

                catch (Exception ex)
                {
                    Thread.Sleep((10 * 60000));
                }
            }


        }



        private void StartBuildWarehouseData()
        {
            bool isBuildStart = false;
            while (!isBuildStart)
            {
                bool isIncrementalDWRunning = warehouseServiceHelper.GetWarehouseFieldFromSettings("IsIncrementalDWRunning", sourceConnectionString);
                if (!isIncrementalDWRunning)
                {
                    isBuildStart = true;
                    warehouseServiceHelper.UpdateWarehouseFieldSettings("IsFullBuildDWRunning", true, sourceConnectionString);
                    warehouseHelper.BuildDataBase(sourceConnectionString, destinationConnectionString);
                    warehouseHelper.BuildOrUpdatePrivateDBData(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Build");
                    warehouseServiceHelper.UpdateWarehouseFieldSettings("IsFullBuildDWRunning", false, sourceConnectionString);
                }
                else Thread.Sleep(2000);
            }
        }







        #endregion

        #region UpdateWarehouseData
        public void UpdateWarehouseData()
        {

            while (true)
            {
                try
                {
                    bool isFullBuildDWRunning = warehouseServiceHelper.GetWarehouseFieldFromSettings("IsFullBuildDWRunning", sourceConnectionString);
                    if (!isFullBuildDWRunning)
                    {
                        warehouseServiceHelper.UpdateWarehouseFieldSettings("IsIncrementalDWRunning", true, sourceConnectionString);
                        warehouseHelper.UpdateWarehouseData(sourceConnectionString, destinationConnectionString);
                        warehouseHelper.BuildOrUpdatePrivateDBData(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Update");
                        warehouseServiceHelper.UpdateWarehouseFieldSettings("IsIncrementalDWRunning", false, sourceConnectionString);
                        Thread.Sleep(ApplicationInfo.UpdateWarehouseSleepTime);
                    }
                    else Thread.Sleep((10 * 60000));
                }

                catch (Exception ex)
                {
                    Thread.Sleep(ApplicationInfo.UpdateWarehouseSleepTime);
                }
            }
        }



        #endregion


        private void BuildConnectionString()
        {
            string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
            string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
            sourceConnectionString = warehouseServiceHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            destinationConnectionString = warehouseServiceHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);
        }



        private DWBuildTime GetDWBuildInfoTime()
        {
            DWBuildTime dwBuildTime = new DWBuildTime();
            if (!string.IsNullOrEmpty(ApplicationInfo.WarehouseBuildHours) && ApplicationInfo.WarehouseBuildDays.Count > 0) dwBuildTime.DWRunTime = warehouseServiceHelper.CalculateDWNextRunTime(DateTime.Now);

            #region DWNextRunTime
            dwBuildTime.DWNextRunTime = warehouseServiceHelper.GetDWNextRunTime(sourceConnectionString);

            if (dwBuildTime.DWNextRunTime == null || (dwBuildTime.DWNextRunTime > dwBuildTime.DWRunTime))
            {
                dwBuildTime.DWNextRunTime = dwBuildTime.DWRunTime;
                dwBuildTime.IsUpdateDWNextRunTime = true;
            }

            if (dwBuildTime.DWNextRunTime != null && dwBuildTime.DWRunTime != dwBuildTime.DWNextRunTime)
            {
                if (dwBuildTime.DWNextRunTime.Value.AddHours(ApplicationInfo.RetryBuildWithinHours) > DateTime.Now)
                {
                    dwBuildTime.IsBuildNow = true;
                }
                else
                {
                    dwBuildTime.DWNextRunTime = dwBuildTime.DWRunTime;
                    dwBuildTime.IsUpdateDWNextRunTime = true;
                }
               
            }
            #endregion

            return dwBuildTime;
        }
    }

    public class DWBuildTime
    {
        public DateTime? DWNextRunTime { get; set; }
        public DateTime? DWRunTime { get; set; }
        public bool IsBuildNow { get; set; }
        public bool IsUpdateDWNextRunTime { get; set; }

    }


}
