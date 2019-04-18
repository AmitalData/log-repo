using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WarehouseData.Helper;
using WarehouseDataService.Helper;

namespace WarehouseDataService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }


        public void OnDebug()
        {
            OnStart(null);

        }
        protected override void OnStart(string[] args)
        {
            try
            {
                ApplicationInfo.GlobalSourceConnection = ConfigurationSettings.AppSettings["SourceConnection"];
                ApplicationInfo.DestinationConnection = ConfigurationSettings.AppSettings["DestinationConnection"];

                string updateWarehouseSleepTime = ConfigurationSettings.AppSettings["UpdateWarehouseSleepTime"];
                ApplicationInfo.UpdateWarehouseSleepTime = (!string.IsNullOrEmpty(updateWarehouseSleepTime) ? Int32.Parse(updateWarehouseSleepTime) : 1) * 60000;


                string warehouseBuildDays = ConfigurationSettings.AppSettings["WarehouseBuildDays"];
                string warehouseBuildHoures = ConfigurationSettings.AppSettings["WarehouseBuildHoures"];

                string retryBuildWithinHours = ConfigurationSettings.AppSettings["RetryBuildWithinHours"];
                ApplicationInfo.RetryBuildWithinHours = !string.IsNullOrEmpty(retryBuildWithinHours) ? Int32.Parse(retryBuildWithinHours) : 0;


                List<int> buildDays = new List<int>();
                if (!string.IsNullOrEmpty(warehouseBuildDays))
                {
                    foreach (string day in warehouseBuildDays.Split(','))
                    {
                        if (!string.IsNullOrEmpty(day)) buildDays.Add(Int32.Parse(day));
                    }
                }

                ApplicationInfo.BliudingServiceWorking = true;
                WarehouseDataHelper warehouseDataHelper = new WarehouseDataHelper();
                warehouseDataHelper.FillDaysList();


                ApplicationInfo.WarehouseBuildDays = ApplicationInfo.Days.Where(d => buildDays.Contains(d.NumberOfDay)).ToList();
                ApplicationInfo.WarehouseBuildHours = !string.IsNullOrEmpty(warehouseBuildHoures) ? warehouseBuildHoures.ToString() : null;


                WarehouseServiceHelper warehouseServiceHelper = new WarehouseServiceHelper();
                ApplicationInfo.SourceConnection = warehouseServiceHelper.GetMainDBConnectionString(ApplicationInfo.GlobalSourceConnection);

                Thread buildWarehouseDatThread = new Thread(() => warehouseDataHelper.BuildWarehouseData());
                buildWarehouseDatThread.IsBackground = true;
                buildWarehouseDatThread.Start();

                Thread updateWarehouseDataThread = new Thread(() => warehouseDataHelper.UpdateWarehouseData());
                updateWarehouseDataThread.IsBackground = true;
                updateWarehouseDataThread.Start();

            }
            catch (Exception ex)
            {

            }


        }

        protected override void OnStop()
        {

        }

    }
}
