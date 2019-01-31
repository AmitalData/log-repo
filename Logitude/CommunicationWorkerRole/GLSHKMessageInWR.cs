using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommunicationWorkerRole
{
    public class GLSHKMessageInWR : WorkerEntryPoint
    {
         string gateWay;
         public GLSHKMessageInWR(string gateWay)
         {
             this.gateWay = gateWay;
         }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        if (string.IsNullOrEmpty(LogitudeSettings.GLSHKURL) || string.IsNullOrEmpty(LogitudeSettings.GLSHKEnv))
                        {
                            Thread.Sleep(300000);
                        }

                        else
                        {
                            string messageData = "";
                            string errorMessage = "";

                            int timeOut = 20000;
                            string url = LogitudeSettings.GLSHKURL;
                            
                            if (gateWay == "amital")
                            {
                                url = "http://192.116.221.66:8732";
                                timeOut = 90000;
                            }
                          
                            url = url + "/Design_Time_Addresses/ChampGateway/QueueGateway";

                            ChampProxy.QueueGatewayClient queueGateway = new ChampProxy.QueueGatewayClient();
                            queueGateway.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);
                            this.UpdateMonitorService();
                            bool succeeded = queueGateway.Get(out messageData, out errorMessage, LogitudeSettings.GLSHKEnv, timeOut);
                            LastActivity = DateTime.UtcNow;
                            if (succeeded)
                            {                               
                                this.SaveMessageToAnalyzeQueue(messageData);
                                LogDoneItemInMemory();
                            }
                            queueGateway.Close();

                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "GLSHKMessageInWR : SaveMessageToAnalyzeQueue Method", null);
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
            ServicePointManager.DefaultConnectionLimit = 12;

            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "GLSHKMessageIn";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            RoleEnvironment.Changing += RoleEnvironmentChanging;
            TopicDescription champMessageInTopic;

            try
            {
                if (!StorageAcountDetails.NameSpaceManager.TopicExists("champmessageintopic"))
                {
                    champMessageInTopic = StorageAcountDetails.NameSpaceManager.CreateTopic("champmessageintopic");
                }

                else
                {
                    champMessageInTopic = StorageAcountDetails.NameSpaceManager.GetTopic("champmessageintopic");
                }

                SubscriptionDescription myAgentSubscription;
                string[] roleId = RoleEnvironment.CurrentRoleInstance.Id.Split('_');
                string subscribtionName = roleId[roleId.Length - 1];

                if (!StorageAcountDetails.NameSpaceManager.SubscriptionExists(champMessageInTopic.Path, subscribtionName))
                {
                    myAgentSubscription = StorageAcountDetails.NameSpaceManager.CreateSubscription(champMessageInTopic.Path, subscribtionName);
                }
            }

            catch
            {

            }

            return base.OnStart();
        }

        private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
        {
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                e.Cancel = true;
            }
        }

        private void UpdateMonitorService()
        {
            string myCode = "GLSHK";

            if (gateWay != null)
            {
                gateWay = gateWay.ToLower();

                if (gateWay == "amazon")
                {
                    myCode = "GLSHK_AMAZON";
                }

                else if (gateWay == "amital")
                {
                    myCode = "GLSHK_AMITAL";
                }
            }

            IGlobalContext globalContext = GlobalContext.GetContext();
            MonitorServiceLastUpdateRepository myRepository = new MonitorServiceLastUpdateRepository(globalContext);
            MonitorServiceLastUpdate lastUpdate = myRepository.GetSingleMonitorServiceLastUpdate(myCode);

            if (lastUpdate == null)
            {
                lastUpdate = new MonitorServiceLastUpdate()
                {
                    Code = myCode,
                    Name = "GLSHK Received message",
                    LastUpdate = TenantServerConfigration.GetCurrentDateTime(0),
                };

                lastUpdate.SearchFields = lastUpdate.Code + "," + lastUpdate.Name;
                myRepository.Add(lastUpdate);
            }

            else
            {
                lastUpdate.LastUpdate = TenantServerConfigration.GetCurrentDateTime(0);
                myRepository.Update(lastUpdate);
            }

            myRepository.SubmitChanges();
        }

        private void SaveMessageToAnalyzeQueue(string xmlString)
        {
            if (!string.IsNullOrEmpty(xmlString))
            {
                if (xmlString.Contains("<"))
                {
                    int index = xmlString.IndexOf('<');
                    if (index > 0)
                    {
                        xmlString = xmlString.Substring(index);
                    }
                }

                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                byte[] messageBytes = Encoding.UTF8.GetBytes(xmlString);

                AnalyzeQueue analyzeQueue = new AnalyzeQueue()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    From = "GLSHK",
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = messageBytes,
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = false,
                    FileSize = xmlString.Length,
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();
            }
        }
    }
}
