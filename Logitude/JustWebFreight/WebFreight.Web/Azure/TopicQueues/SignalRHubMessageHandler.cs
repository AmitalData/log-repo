using Logitude.Server.Tools;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure.ServiceRuntime;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Azure.TopicQueues
{
    public class SignalRHubMessageHandler
    {

        SubscriptionClient subscriptionClient;
        public SignalRHubMessageHandler()
        {
            //string[] roleId = RoleEnvironment.CurrentRoleInstance.Id.Split('_');
            string subscribtionName = Environment.MachineName; //roleId[roleId.Length - 1];
            subscriptionClient = Microsoft.ServiceBus.Messaging.SubscriptionClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), StorageAcountDetails.SignalRHubTopicName, subscribtionName);
        }

        public void HandleTopicMessages()
        {
            while (true)
            {
                try
                {
                    var brMessage = subscriptionClient.Receive();
                    if (brMessage != null)
                    {

                        
                        var eventName = brMessage.Properties["EventName"].ToString();
                        var channelName = brMessage.Properties["ChannelName"].ToString();
                        var eventParameter = brMessage.Properties["EventParameter"].ToString();

                        if (channelName != "LogBox")
                        {
                            LogitudeHubChannelEvent ev = new LogitudeHubChannelEvent()
                            {
                                EventName = eventName,
                                ChannelName = channelName,
                                EventParameter = eventParameter,

                            };

                            SignalRHubEventPublisher.PublishLogitudeHubEvent(ev);
                        }
                        else
                        {
                            IHubProxy _hub;
                            var connection = new HubConnection(LogitudeSettings.LogitudeURL, new Dictionary<string, string> { { "UserName", eventParameter } });
                            _hub = connection.CreateHubProxy("LogBoxSignatureHub");
                            connection.Start().Wait();

                            _hub.Invoke("SignRequestReceived", eventParameter).Wait();
                        }

                        brMessage.Complete();
                    }
                }
                catch { }
            }
        }
    }
}