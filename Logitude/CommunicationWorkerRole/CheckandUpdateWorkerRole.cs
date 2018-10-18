using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.InfrastructureModel;
using WebFreight.Web.Azure;
using WebFreight.Web.Testing;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Transactions;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.MetaDataUpdate;
using Microsoft.WindowsAzure.Storage.Queue;
using Logitude.SystemLogs;
using WebFreight.Web.MetaDataUpdate.UpdateClasses;
using System.Diagnostics;
using Logitude.Server.Tools;

namespace CommunicationWorkerRole
{
    class CheckandUpdateWorkerRole : WorkerEntryPoint
    {
        CloudStorageAccount storageAccount;
        CloudQueueClient queueclient;
        CloudQueue queue;

        public override void Run()
        {
            bool UpdateClient = true;
            //TenantRepository tenantsRepository;
            GlobalTenantRepository GlobaltenantRep;
            while (IsRunning)
            {
                if (UpdateClient)
                {
                    //List<Tenant> tenants=null;
                    List<GlobalTenant> globalTenants;
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {                       
                        globalTenants = GlobalTenantRepository.GetGlobalTenants();
                        //tenantsRepository = new TenantRepository(0);
                        //WebFreight.Web.Testing.General.IsTesting = false;
                        //tenants = tenantsRepository.GetTenants().ToList();
                    }

                    StringBuilder str = new StringBuilder();
                    GlobalTenant tenantZero = globalTenants.Where(d => d.Id == 0).FirstOrDefault();// Tenant tenantZero = tenants.Where(d => d.Id == 0).FirstOrDefault();
                    //EmailsWorkerRole emailrole = new EmailsWorkerRole();
                    if (queue.Exists())
                    {
                        var msg = queue.GetMessage();
                        LastActivity = DateTime.UtcNow;
                        if (msg != null)
                        {
                            string message = msg.AsString;
                            queue.DeleteMessage(msg);

                            if (message != "buildzipfiles")
                            {
                                TenantsUpdateClass.UpdateDataForTenant(0, message);
                                AzureLog.SaveLogsInStorage("(" + message + ")" + " Update Data for tenant : " + 0 + " Completed successfully", "P", DateTime.Now, "", "", 0, "", "WorkerRole", null);
                            }
                            else
                            {
                                TenantsUpdateClass.BuildObjectTablesZipFilesData();
                                AzureLog.SaveLogsInStorage("(" + message + ")" + " Update Data for tenant : " + 0 + " Completed successfully", "P", DateTime.Now, "", "", 0, "", "WorkerRole", null);
   
                            }

                            if (LogitudeSettings.DeploymentStage != "Dev")
                            {
                                string emailbody = "";
                                if (!string.IsNullOrEmpty(message))
                                {
                                     emailbody = message + " was completed successfully.";
                                }
                                else
                                {
                                     emailbody = "Updating tenant zero was completed successfully.";
                                }
                                str.AppendLine(emailbody);
                                 
                                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                                {
                                    From = "admin@fnarsoft.com",
                                    To = "jalal@logitudeworld.com",
                                    CC = "ahmada@logitudeworld.com;ahmadb@logitudeworld.com",
                                    BCC = "",
                                    Subject = LogitudeSettings.DeploymentStage + " - Update completed successfully.",
                                    EmailBody = emailbody,
                                    Tenant = 0,
                                };

                                Communications.AddEmailCommunicationLogQueue(emailParams, 0);
                            }
                            else
                            {
                                Debugger.Log(1, "Update System",
                                    Environment.NewLine +
                                    "*******************************************" +
                                    "Update complete successfully!" +
                                    "*******************************************" +
                                     Environment.NewLine);
                                //Debugger.Break();
                            }
                            LogDoneItemInMemory();
                        }
                    }

                    if (globalTenants != null)
                    {
                        List<GlobalTenant> upgradableTenants = (from a in globalTenants
                                                                where a.Version != tenantZero.Version && a.Id != 0 && a.Version != -1 && a.IsActive == true
                                                                select a).ToList();
                        if (upgradableTenants.Count > 0)
                        {
                            StringBuilder str1 = new StringBuilder();
                            foreach (GlobalTenant tenant in upgradableTenants)
                            {
                                if (tenant.Id != 0)//&&tenant.Id!=1&&tenant.Id!=2&&tenant.Id!=3
                                {
                                    try
                                    {
                                        long StartTime;
                                        long EndTime;

                                        str1.AppendLine(DateTime.Now.ToString());

                                        StartTime = System.DateTime.Now.Ticks;

                                        TenantsUpdateClass.UpdateDataForTenant(tenant.Id, "");

                                        EndTime = System.DateTime.Now.Ticks;

                                         string Duration = Convert.ToString((EndTime - StartTime) / TimeSpan.TicksPerMillisecond);
                                         string emailbody = "Updating tenant " + tenant.Id + " completed successfully. " + Duration;
                                         str1.AppendLine(emailbody);
                                        AzureLog.SaveLogsInStorage("Update Data for tenant:" + tenant.Id + " Completed successfully", "P", DateTime.Now, "", "", 0, "", "WorkerRole", null);
                                    }

                                    catch (Exception e)
                                    {
                                        GlobaltenantRep = new GlobalTenantRepository();
                                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "", "WorkerRole",null);
                                        string emailbody = "Updating tenant " + tenant.Id + " Failed!.";
                                        str1.AppendLine(emailbody);

                                        tenant.Version = -1;
                                        GlobalTenant updatedTenant = GlobaltenantRep.GetGlobalTenantsByTenant(tenant.Id);
                                        updatedTenant.Version = -1;
                                        GlobaltenantRep.Update(updatedTenant);
                                        GlobaltenantRep.SubmitChanges();

                                        if (LogitudeSettings.DeploymentStage == "Dev")
                                        {
                                            //throw e;
                                        }
                                    }
                                }
                            }

                            if (LogitudeSettings.DeploymentStage != "Dev")
                            {
                                string machineInfo = !string.IsNullOrEmpty(Environment.MachineName) ? Environment.MachineName : "";
                                str1.AppendLine(machineInfo);
                                 

                                EmailCommunicationParams emailParams = new EmailCommunicationParams()
                                {
                                    From = "admin@fnarsoft.com",
                                    To = "jalal@logitudeworld.com",
                                    CC = "",
                                    BCC = "",
                                    Subject = "Update All Tenants History",
                                    EmailBody = str1.ToString(),
                                    Tenant = 0,
                                };

                                Communications.AddEmailCommunicationLogQueue(emailParams, 0);
                            }
                        }
                    }
                }

                Thread.Sleep(60000);//600000
            }
        }

        public override bool OnStart()
        {
            storageAccount = StorageAcountDetails.StorageAccount; //CloudStorageAccount.FromConfigurationSetting("DiagnosticsConnectionString");
            queueclient = storageAccount.CreateCloudQueueClient();
            queue = queueclient.GetQueueReference("updatetenantzeroqueue");
            queue.CreateIfNotExists();
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CheckandUpdate";
            DoneItemsInRange = new Dictionary<DateTime, int>();

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
