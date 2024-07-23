using CargoTrackingWinService.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CargoTrackingWinService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }
        public void OnDebug(string SourceConnection, string DestinationConnection , int Sleep)
        {
            ApplicationInfo.SourceConnection = SourceConnection;
            ApplicationInfo.DestinationConnection = DestinationConnection;
            ApplicationInfo.UpdateCargoTrackingSleepTime = Sleep;
            OnStart(null);

        }

        protected override void OnStart(string[] args)
        {
            try
            {

                CargoTrackingServiceHelper cargoTrackingServiceHelper = new CargoTrackingServiceHelper();

                //string sourceConnection = cargoTrackingServiceHelper.BuildConnectionString(ConfigurationManager.AppSettings["SourceConnection"]);
                //ApplicationInfo.SourceConnection = cargoTrackingServiceHelper.GetMainDBConnectionString(sourceConnection);
                ApplicationInfo.UpdateCounter = 0;
                if (ApplicationInfo.Mode!= "Debug")
                {
                    ApplicationInfo.SourceConnection = ConfigurationManager.AppSettings["SourceConnection"];
                    ApplicationInfo.DestinationConnection = ConfigurationManager.AppSettings["DestinationConnection"];
                    string updateWarehouseSleepTime = ConfigurationManager.AppSettings["UpdateCargoTrackingSleepTime"];
                    ApplicationInfo.UpdateCargoTrackingSleepTime = (!string.IsNullOrEmpty(updateWarehouseSleepTime) ? Int32.Parse(updateWarehouseSleepTime) : 1) * 60000;
                }
               
                
                string retryBuildWithinHours = ConfigurationManager.AppSettings["RetryBuildWithinHours"];
                ApplicationInfo.RunCargoTrackingImmediately = GetIsBuildCargoTrackingFromConfigurationSettings();


                CargoTrackingMainWinService cargoTrackingHeadService = new CargoTrackingMainWinService();
                Thread updateWarehouseDataThread = new Thread(() => cargoTrackingHeadService.UpdateCargoTracking());
                updateWarehouseDataThread.IsBackground = true;
                updateWarehouseDataThread.Start();

            }
            catch (Exception ex)
            {

            }

        }

        private bool GetIsBuildCargoTrackingFromConfigurationSettings()
        {
            bool result = false;
            var isBuildDWHNow = ConfigurationManager.AppSettings["RunCargoTrackingImmediately"] != null ? ConfigurationManager.AppSettings["RunCargoTrackingImmediately"].ToString() : null;
            if (!string.IsNullOrEmpty(isBuildDWHNow))
            {
                result = Boolean.Parse(isBuildDWHNow);
            }

            return result;
        }
        protected override void OnStop()
        {
        }
    }
}
