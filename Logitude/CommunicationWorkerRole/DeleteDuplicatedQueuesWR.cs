using System.Threading;
using System.IO;
using System.Net.Mail;
using System.Net;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;

using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

using Logitude.SystemLogs;
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.Helpers;
using System.Data.SqlClient;
using System.Data;

namespace CommunicationWorkerRole
{
    public class DeleteDuplicatedQueuesWR : WorkerEntryPoint
    {


        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        string strConnString = TenantServerConfigration.GetDbConnection(0);
                        using (SqlConnection cn = new SqlConnection(strConnString))
                        {
                            SqlCommand cmd = new SqlCommand("[dbo].[RemoveDuplicatedQueueMessages]", cn);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cn.Open();
                            var output = cmd.ExecuteNonQuery();
                            cn.Close();
                        }

                        Thread.Sleep(15000);
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DeleteDuplicatedQueuesWR worker role start", null, null);
                        Thread.Sleep(10000);
                    }

                }
                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

      

        public override bool OnStart()
        {
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "DeleteDuplicatedQueuesWR";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {

            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "DeleteDuplicatedQueuesWR role start", null, null);
            }

            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            RoleEnvironment.Changing += RoleEnvironmentChanging;

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {

            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {

                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
