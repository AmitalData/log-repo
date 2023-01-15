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
        MainDataWarehouseService mainDataWarehouseService;
        MainDataWarehouseService privateMainDataWarehouseService;

        public WarehouseService()
        {

            mainDataWarehouseService = new MainDataWarehouseService("Service", ApplicationInfo.Mode);
            privateMainDataWarehouseService = new MainDataWarehouseService("Service", ApplicationInfo.Mode);

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
                    DWBuildTime dwBuildTime = !ApplicationInfo.RunDataWarehouseImmediately ? GetDWBuildInfoTime() : new DWBuildTime();
                    if (ApplicationInfo.RunDataWarehouseImmediately)
                    {
                        dwBuildTime.IsBuildNow = true;
                        warehouseServiceHelper.UpdateDWNextRunTime(sourceConnectionString, DateTime.Now);
                    }
                    
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
                        ApplicationInfo.RunDataWarehouseImmediately = false;
                    }
                    else Thread.Sleep(new TimeSpan(0, 5, 0));
                }

                catch (Exception ex)
                {
                    Thread.Sleep(new TimeSpan(0, 5, 0));
                }
            }


        }

        private void StartBuildWarehouseData()
        {
            bool isBuildStart = false;
            while (!isBuildStart)
            {
                if (!warehouseServiceHelper.CheckIsUpgradingSystem(sourceConnectionString))
                {
                    bool isIncrementalDWRunning = warehouseServiceHelper.GetFieldValueFromDBByTableNameAndFieldName("IsIncrementalDWRunning", "DWHBuildStatus", sourceConnectionString);
                    if (!isIncrementalDWRunning)
                    {
                        isBuildStart = true;
                        warehouseServiceHelper.UpdateDWHBuildStatus("IsFullBuildDWRunning", true, sourceConnectionString);
                        RuningBuildDataWarehouseByTasks();
                        warehouseServiceHelper.UpdateDWHBuildStatus("IsFullBuildDWRunning", false, sourceConnectionString);
                        warehouseServiceHelper.UpdateLastIncrementalDWUpdateDate(sourceConnectionString);
                    }
                    else Thread.Sleep(2000);
                }
                else Thread.Sleep(new TimeSpan(0, 5, 0));
            }
        }

        private void RuningBuildDataWarehouseByTasks()
        {
            Task dataWarehouseBuildTask = new Task(() => mainDataWarehouseService.BuildDataWarehouse(sourceConnectionString, destinationConnectionString));
            Task privateDataWarehouseBuildTask = new Task(() => BuildPrivateDataWarehouse());
            dataWarehouseBuildTask.Start();
            privateDataWarehouseBuildTask.Start();
            Task.WhenAll(dataWarehouseBuildTask, privateDataWarehouseBuildTask).Wait();

        }

        private void BuildPrivateDataWarehouse()
        {
            Thread.Sleep(new TimeSpan(0, 130, 0));
            privateMainDataWarehouseService.BuildOrUpdatePrivateDataWarehouse(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Build");
        }


        #endregion

        #region UpdateWarehouseData
        public void UpdateWarehouseData()
        {

            while (true)
            {
                try
                {
                    if (!ApplicationInfo.RunDataWarehouseImmediately)
                    {
                        if (!warehouseServiceHelper.CheckIsUpgradingSystem(sourceConnectionString))
                        {
                            bool isFullBuildDWRunning = warehouseServiceHelper.GetFieldValueFromDBByTableNameAndFieldName("IsFullBuildDWRunning", "DWHBuildStatus", sourceConnectionString);
                            if (!isFullBuildDWRunning)
                            {

                                warehouseServiceHelper.UpdateDWHBuildStatus("IsIncrementalDWRunning", true, sourceConnectionString);
                                RuningUpdateDataWarehouseByTasks();
                                warehouseServiceHelper.UpdateDWHBuildStatus("IsIncrementalDWRunning", false, sourceConnectionString);
                                warehouseServiceHelper.UpdateLastIncrementalDWUpdateDate(sourceConnectionString);
                                Thread.Sleep(ApplicationInfo.UpdateWarehouseSleepTime);
                            }
                            else Thread.Sleep(new TimeSpan(0, 5, 0));
                        }
                        else Thread.Sleep(new TimeSpan(0, 5, 0));
                    }
                    else Thread.Sleep(new TimeSpan(0, 5, 0));
                }

                catch (Exception ex)
                {
                    Thread.Sleep(ApplicationInfo.UpdateWarehouseSleepTime);
                }
            }
        }


        private void RuningUpdateDataWarehouseByTasks()
        {
            Task dataWarehouseUpdateTask = new Task(() => mainDataWarehouseService.UpdateDataWarehouse(sourceConnectionString, destinationConnectionString));
            Task privateDataWarehouseUpdateTask = new Task(() => privateMainDataWarehouseService.BuildOrUpdatePrivateDataWarehouse(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Update"));
            dataWarehouseUpdateTask.Start();
            privateDataWarehouseUpdateTask.Start();
            Task.WhenAll(dataWarehouseUpdateTask, privateDataWarehouseUpdateTask).Wait();
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
