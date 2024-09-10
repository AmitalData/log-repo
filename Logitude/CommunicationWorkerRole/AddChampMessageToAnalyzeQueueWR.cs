using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure;
using Logitude.Server.Tools.QueueService;

namespace CommunicationWorkerRole
{
    public class AddChampMessageToAnalyzeQueueWR : WorkerEntryPoint
    {
        SubscriptionClient subscriptionClient;
        public override void Run()
        {

            while (IsRunning)
            {
                BrokeredMessage message=null;
                if (!General.IsUpdating())
                {
                    try
                    {
                        if (subscriptionClient != null)
                        {

                            message = subscriptionClient.Receive(new TimeSpan(0, 0, 30));
                            LastActivity = DateTime.UtcNow;
                            if (message != null)
                            {
                                string messageData = message.Properties["MessageData"].ToString();
                                SaveMessageToAnalyzeQueue(messageData);
                                message.Complete();
                                LogDoneItemInMemory();
                            }
                        }                       
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ChampMessageInWR : SaveMessageToAnalyzeQueue Method", null);

                        if (message != null)
                        {
                            message.Abandon();
                        }

                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }

        }

        private void SaveMessageToAnalyzeQueue(string messageData)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageData);

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Champ",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = messageData.Length,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();

            DbQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue("ChampAnalyzer", 0);
            queueservice.Send(new Dictionary<string, string>() { { "AnalyzeQueueId", analyzeQueue.Id } }, analyzeQueue.Tenant);
            queueservice.Complete();
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;
            //ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ChampMessageToAnalyzeQueue";
            DoneItemsInRange = new Dictionary<DateTime, int>();
            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //RoleEnvironment.Changing += RoleEnvironmentChanging;

            if (!string.IsNullOrEmpty(LogitudeSettings.ChampEnv))
            {
                //string[] roleId = null;

                string subscribtionName = "ChampSubScription";
                

                subscriptionClient = Microsoft.ServiceBus.Messaging.SubscriptionClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(), "champmessageintopic", subscribtionName);
            }

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