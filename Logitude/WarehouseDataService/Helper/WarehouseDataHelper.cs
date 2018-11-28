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
    public class WarehouseDataHelper
    {
        #region BuildWarehouseData

        public void BuildWarehouseData()
        {
            try
            {
                while (true)
                {

                    DateTime? warehouseDate = null;
                    if (!string.IsNullOrEmpty(ApplicationInfo.WarehouseBuildHours) && ApplicationInfo.WarehouseBuildDays.Count > 0) warehouseDate = GetWarehouseRunDate(DateTime.Now);

                    #region DWNextRunTime
                    WarehouseServiceHelper warehouseServiceHelper = new WarehouseServiceHelper();
               
                    string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
                    string sourceConnectionString = warehouseServiceHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                    bool IsBuildNow = false;

                    DateTime? DWNextRunTime = warehouseServiceHelper.GetDWNextRunTime(sourceConnectionString);

                    if (DWNextRunTime == null || ( DWNextRunTime > warehouseDate))
                    {
                        DWNextRunTime = warehouseDate;
                        warehouseServiceHelper.UpdateDWNextRunTime(sourceConnectionString, DWNextRunTime);
                    }


                    if (DWNextRunTime != null)
                    {
                       if(warehouseDate!= DWNextRunTime)
                        {
                            if (DWNextRunTime.Value.AddHours(ApplicationInfo.RetryBuildWithinHours) > DateTime.Now) IsBuildNow = true;
                        }
                    }
                    #endregion

                    if (warehouseDate != null || IsBuildNow)
                    {
                        if (!IsBuildNow)
                        {
                            TimeSpan span = (DateTime)warehouseDate - (DateTime.Now);
                            int sleepTime = (int)span.TotalMilliseconds;
                            Thread.Sleep(sleepTime);
                        }
                       
                        StartBuildWarehouseData();

                        DWNextRunTime = GetWarehouseRunDate(DateTime.Now);
                        warehouseServiceHelper.UpdateDWNextRunTime(sourceConnectionString, DWNextRunTime);
                    }
                    else Thread.Sleep((5 * 60000));


                }
            }
            catch (Exception ex)
            {

            }

        }
        private void StartBuildWarehouseData()
        {
            WarehouseHelper warehouseHelper = new WarehouseHelper("Service", ApplicationInfo.Mode);
            WarehouseServiceHelper warehouseServiceHelper = new WarehouseServiceHelper();
            string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
            string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
            string sourceConnectionString = warehouseHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            string destinationConnectionString = warehouseHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);


            bool isBuildStart = false;
            while (!isBuildStart)
            {
                if (!warehouseServiceHelper.GetWarehouseFieldFromSettings("IsIncrementalDWRunning", sourceConnectionString))
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
            try
            {
                WarehouseHelper warehouseHelper = new WarehouseHelper("Service", ApplicationInfo.Mode);
                WarehouseServiceHelper warehouseServiceHelper = new WarehouseServiceHelper();
                string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
                string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
                string sourceConnectionString = warehouseHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                string destinationConnectionString = warehouseHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);

                while (true)
                {
                    if (!warehouseServiceHelper.GetWarehouseFieldFromSettings("IsFullBuildDWRunning", sourceConnectionString))
                    {
                        warehouseServiceHelper.UpdateWarehouseFieldSettings("IsIncrementalDWRunning", true, sourceConnectionString);
                        warehouseHelper.UpdateWarehouseData(sourceConnectionString, destinationConnectionString);
                        warehouseHelper.BuildOrUpdatePrivateDBData(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Update");
                        warehouseServiceHelper.UpdateWarehouseFieldSettings("IsIncrementalDWRunning", false, sourceConnectionString);

                        Thread.Sleep(ApplicationInfo.UpdateWarehouseSleepTime);
                    }
                    else Thread.Sleep((10 * 60000));

                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion

        #region Warehouse Run Date
        private DateTime? GetWarehouseRunDate(DateTime todayDate)
        {


            DateTime? warehouseDate = null;

            DayOfWeekClass dayToday = ApplicationInfo.Days.Where(d => d.DayOfWeek == todayDate.DayOfWeek).FirstOrDefault();
            DayOfWeekClass warehouseday = ApplicationInfo.WarehouseBuildDays.Where(d => d.NumberOfDay == dayToday.NumberOfDay).FirstOrDefault();

            if (warehouseday != null)
            {
                warehouseDate = DateTime.Parse((todayDate.Date.ToShortDateString() + " " + ApplicationInfo.WarehouseBuildHours.ToString()));
                if (warehouseDate < todayDate)
                {
                    warehouseDate = null;
                    warehouseday = null;
                }
            }

            if (warehouseday == null)
            {
                warehouseday = ApplicationInfo.WarehouseBuildDays.Where(d => d.NumberOfDay > dayToday.NumberOfDay).OrderBy(a => a.NumberOfDay).FirstOrDefault();
                if (warehouseday != null)
                {
                    int dayBetwwenDate = warehouseday.NumberOfDay - dayToday.NumberOfDay;
                    warehouseDate = DateTime.Parse((todayDate.AddDays(dayBetwwenDate).Date.ToShortDateString() + " " + ApplicationInfo.WarehouseBuildHours.ToString()));
                }
            }

            if (warehouseday == null)
            {
                warehouseday = ApplicationInfo.WarehouseBuildDays.Where(d => d.NumberOfDay < dayToday.NumberOfDay).OrderBy(a => a.NumberOfDay).FirstOrDefault();
                if (warehouseday != null)
                {
                    int dayBetwwenDate = (7 - dayToday.NumberOfDay) + warehouseday.NumberOfDay;
                    warehouseDate = DateTime.Parse((todayDate.AddDays(dayBetwwenDate).Date.ToShortDateString() + " " + ApplicationInfo.WarehouseBuildHours.ToString()));
                }
            }

            return warehouseDate;
        }
        #endregion

        #region  FillDays

        public void FillDaysList()
        {
            ApplicationInfo.Days = new List<DayOfWeekClass>();
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Sunday, 0));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Monday, 1));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Thursday, 2));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Wednesday, 3));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Tuesday, 4));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Friday, 5));
            ApplicationInfo.Days.Add(new DayOfWeekClass(DayOfWeek.Saturday,6));
        }
        #endregion

    }


}
