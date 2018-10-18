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
                    DateTime todayDate = DateTime.Now;
                    DateTime? warehouseDate = null;
                    if (!string.IsNullOrEmpty(ApplicationInfo.WarehouseBuildHours) && ApplicationInfo.WarehouseBuildDays.Count > 0) warehouseDate = GetWarehouseRunDate(todayDate);
   
                    if (warehouseDate != null)
                    {
                        TimeSpan span = (DateTime)warehouseDate - todayDate;
                        int sleepTime = (int)span.TotalMilliseconds;
                        Thread.Sleep(sleepTime);
                        StartBuildWarehouseData();
                    }
                    else Thread.Sleep((20 * 60000));


                }
            }
            catch (Exception ex)
            {

            }

        }
        private void StartBuildWarehouseData()
        {
            WarehouseHelper warehouseHelper = new WarehouseHelper();
            string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
            string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
            string sourceConnectionString = warehouseHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
            string destinationConnectionString = warehouseHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);


            bool isBuildStart = false;
            while (!isBuildStart)
            {
                if (!warehouseHelper.GetWarehouseFieldFromSettings("IsIncrementalDWRunning", sourceConnectionString))
                {
                    isBuildStart = true;
                    warehouseHelper.UpdateWarehouseFieldSettings("IsFullBuildDWRunning", true, sourceConnectionString);
                    warehouseHelper.BuildDataBase(sourceConnectionString, destinationConnectionString);
                    warehouseHelper.BuildOrUpdatePrivateDBData(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Build");
                    warehouseHelper.UpdateWarehouseFieldSettings("IsFullBuildDWRunning", false, sourceConnectionString);
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
                WarehouseHelper warehouseHelper = new WarehouseHelper();
                string[] sourceConnectionArray = ApplicationInfo.SourceConnection.Split(',');
                string[] destinationConnectionArray = ApplicationInfo.DestinationConnection.Split(',');
                string sourceConnectionString = warehouseHelper.BuildConnectionString(sourceConnectionArray[0], sourceConnectionArray[1], sourceConnectionArray[2], sourceConnectionArray[3]);
                string destinationConnectionString = warehouseHelper.BuildConnectionString(destinationConnectionArray[0], destinationConnectionArray[1], destinationConnectionArray[2], destinationConnectionArray[3]);

                while (true)
                {
                    if (!warehouseHelper.GetWarehouseFieldFromSettings("IsFullBuildDWRunning", sourceConnectionString))
                    {
                        warehouseHelper.UpdateWarehouseFieldSettings("IsIncrementalDWRunning", true, sourceConnectionString);
                        warehouseHelper.UpdateWarehouseData(sourceConnectionString, destinationConnectionString);
                        warehouseHelper.BuildOrUpdatePrivateDBData(ApplicationInfo.SourceConnection, ApplicationInfo.DestinationConnection, "Update");
                        warehouseHelper.UpdateWarehouseFieldSettings("IsIncrementalDWRunning", false, sourceConnectionString);

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
