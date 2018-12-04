using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.IO;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using WebFreight.Web.CommonDataModel;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using System.Transactions;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Azure;
using Microsoft.WindowsAzure.Storage;
using System.Threading;
using System.Xml;
using WebFreight.Web.InfrastructureModel;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.Repositories;
using WebFreight.Web.InfrastructureModel.DomainServices;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.SystemLogs;
using Logitude.Server.Tools.Counters;
using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure;
using Simplog.Global.Data.GlobalModel;

namespace CommunicationWorkerRole
{
    public class ChampMessageInWR : WorkerEntryPoint
    {
        string gateWay;
        public ChampMessageInWR(string gateWay)
        {
            this.gateWay = gateWay;

            if (LogitudeSettings.ChampEnv == "TEST")
            {
                gateWay = "amazon";
            }
        }

        public override void Run()
        {
            while (IsRunning)
            {
                if (!General.IsUpdating())
                {
                    try
                    {
                        string messageData = "";
                        string errorMessage = "";

                        int timeOut = 20000;
                        string url = LogitudeSettings.ChampURL;

                        if (gateWay == "amital")
                        {
                            url = "http://192.116.221.66:8732";
                            timeOut = 90000;
                        }

                        if (!string.IsNullOrEmpty(url))
                        {
                            url = url + "/Design_Time_Addresses/ChampGateway/QueueGateway";

                            ChampProxy.QueueGatewayClient queueGateway = new ChampProxy.QueueGatewayClient();
                            queueGateway.Endpoint.Address = new System.ServiceModel.EndpointAddress(url);
                            queueGateway.InnerChannel.OperationTimeout = new TimeSpan(0, 2, 0);
                            bool succeeded = queueGateway.Get(out messageData, out errorMessage, LogitudeSettings.ChampEnv, timeOut);
                            LastActivity = DateTime.UtcNow;
                            this.UpdateMonitorService();

                            if (succeeded)
                            {
                                BrokeredMessage message = new BrokeredMessage();
                                message.Properties["MessageData"] = messageData;
                                TopicClient client = Microsoft.ServiceBus.Messaging.TopicClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), "champmessageintopic");

                                client.Send(message);
                                LogDoneItemInMemory();
                            }
                                                        
                            queueGateway.Close();
                        }
                    }

                    catch (Exception e)
                    {
                        ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "WorkerRole", "ChampMessageInWR : SaveMessageToAnalyzeQueue Method", null);
                        Thread.Sleep(10000);
                    }
                }

                else
                {
                    Thread.Sleep(60000);
                }
            }
        }

        private void UpdateMonitorService()
        {
            string myCode = "CHAMP";

            if (gateWay != null)
            {
                gateWay = gateWay.ToLower();

                if (gateWay == "amazon")
                {
                    myCode = "CHAMP_AMAZON";
                }

                else if (gateWay == "amital")
                {
                    myCode = "CHAMP_AMITAL";
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
                    Name = "CHAMP Received message",
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

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;


            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "ChampMessageIn";
            DoneItemsInRange = new Dictionary<DateTime, int>();

            //DiagnosticMonitor.Start("DiagnosticsConnectionString");

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            //RoleEnvironment.Changing += RoleEnvironmentChanging;
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

                string subscribtionName = "ChampSubScription";
                SubscriptionDescription myAgentSubscription;
                //string[] roleId = RoleEnvironment.CurrentRoleInstance.Id.Split('_');

                //if (LogitudeSettings.DeploymentStage == "Dev")
                //{
                //    subscribtionName = Environment.MachineName + "_" + roleId[roleId.Length - 1];
                //}

                //else
                //{
                //    subscribtionName = roleId[roleId.Length - 1];
                //}

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
            // If a configuration setting is changing
            if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
            {
                // Set e.Cancel to true to restart this role instance
                e.Cancel = true;
            }
        }
    }
}
