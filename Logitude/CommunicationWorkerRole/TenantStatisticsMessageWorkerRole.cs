using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
namespace CommunicationWorkerRole
{
    public class TenantStatisticsMessageWorkerRole : WorkerEntryPoint
    {
        private bool serviceStarted = true;
        private int interval = 1;
        private int systemErrorCount = 0;
        QueueDescription queueDescription;
        QueueClient client;
        TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
        GlobalTenantRepository globalTenantRepository = new GlobalTenantRepository();
        public override void Run()
        {

            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        // TenantRepository tenantRepository = new TenantRepository();

                        var message = client.Receive(new TimeSpan(0, 0, 30));
                        LastActivity = DateTime.UtcNow;
                        if (message != null)
                        {
                            int tenant = 0;

                            try
                            {

                                tenant = (int)message.Properties["Tenant"];
                                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(tenant);
                                TenantStatisticsWorkerRole role = new TenantStatisticsWorkerRole();
                                role.RefreshTenantStatistics(tenantManagement);
                                tenantManagementRepository.SubmitChanges();
                                //do work for specific tenant

                                message.Complete();
                                LogDoneItemInMemory();

                            }
                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, tenant, "", "WorkerRole", "", null);
                                if (message.Properties.Keys.Contains("Tenant"))
                                {
                                    string fileId = message.Properties["Tenant"].ToString();
                                    if (fileId != null)
                                    {
                                        message.Abandon();
                                    }
                                    else
                                    {
                                        message.Complete();
                                    }
                                }
                                else
                                {
                                    message.Complete();
                                }


                            }

                        }
                         
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "tenant statistics worker role start", null,null);
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
            BatchServiceCode = "TenantStatisticsMessage";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            try
            {

                string emailQueueName = ThreadedRoleEntryPoint.GetQueueByEnviroment("TenantStatisticsdataqueue");


                if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailQueueName))
                {
                    queueDescription = new QueueDescription(emailQueueName);
                    queueDescription.MaxSizeInMegabytes = 5120;
                    // queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);

                    StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
                }

                client = StorageAcountDetails.CreateServiceBusQueueClient(emailQueueName);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "tenant statistics worker role start", null,null);
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
