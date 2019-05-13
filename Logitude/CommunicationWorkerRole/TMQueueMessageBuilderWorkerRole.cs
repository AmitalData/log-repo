using Logitude.Server.Tools.QueueService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;

using WebFreight.Web.Helpers;

namespace CommunicationWorkerRole
{
    public class TMQueueMessageBuilderWorkerRole : WorkerEntryPoint
    {
        private string queueName = "timemanagementqueue";
        private int Tenant;
        private string WorkItemNumber;
        private string CompletedWork;
        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        this.Tenant = 0;
                        queueservice = new DbQueueService();
                        queueservice.InitializeQueue(queueName, 0);
                        //queueservice = QueueServiceManager.GetQueueService(queueName, 0);
                        var response = queueservice.Receive();
                        LastActivity = DateTime.UtcNow;
                        if (response.MessageId != null)
                        {
                            try
                            {
                                int.TryParse(response.MessageValues["Tenant"].ToString(), out Tenant);
                                WorkItemNumber = response.MessageValues["WorkItemNumber"].ToString();
                                CompletedWork = response.MessageValues["CompletedWork"].ToString();
                                TMMessageBuilderAnalyzer analyzer = new TMMessageBuilderAnalyzer(WorkItemNumber, CompletedWork, queueservice, Tenant);
                                LogDoneItemInMemory();
                            }

                            catch (Exception ex)
                            {
                                ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, "", "TMQueueMessageBuilderWorkerRole", "", null);
                                queueservice.CompleteAsFailed();
                                Thread.Sleep(10000);
                            }
                        }

                        //continue
                        Thread.Sleep(10000);
                    }

                    catch (Exception ex)
                    {
                        ConnectClient();
                        ExceptionHandler.HandleException(ex, DateTime.Now, Tenant, null, "TMQueueMessageBuilderWorkerRole Run method", null, null);
                        queueservice.CompleteAsFailed();
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
            ConnectClient();
            ServicePointManager.DefaultConnectionLimit = 12;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "TMQueueMessageBuilderWorkerRole";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            RoleEnvironment.Changing += RoleEnvironmentChanging;
            return base.OnStart();
        }

        IQueueService queueservice;
        public void ConnectClient()
        {
            try
            {
                queueservice = QueueServiceManager.GetQueueService(queueName, 0);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
            }
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            { 
                e.Cancel = true;
            }
        }
    }
}
