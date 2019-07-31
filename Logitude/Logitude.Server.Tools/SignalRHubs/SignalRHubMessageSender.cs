//using Microsoft.AspNet.SignalR.Client;
//using Microsoft.ServiceBus.Messaging;
//using Simplog.Server.Infrastructure;
//using Simplog.Server.Infrastructure.Azure;
//using Simplog.Server.Infrastructure.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Transactions;

//namespace Logitude.Server.Tools.SignalRHubs
//{
//    public class SignalRHubMessageSender
//    {
//        public static void SendSignalRMessage(string eventName, string channelName, string eventParameter)
//        {
//            if (LogitudeSettings.DeploymentStage == "Dev" || LogitudeSettings.DeploymentStage == "Test2" || LogitudeSettings.IsCostomsDeploy)
//            {
//                SendSignalMessage(eventName, channelName, eventParameter);
//            }
//            else
//            {
//                SendTopicMessage(eventName, eventParameter, channelName);
//            }

//        }

        

//        public static void SendUserChannelMessage(string eventName, string eventParameter, string userId, int tenant)
//        {
//            string channelName = "UserChannel" + userId + tenant;
//            if (LogitudeSettings.DeploymentStage == "Dev" || LogitudeSettings.DeploymentStage == "Test2" || LogitudeSettings.IsCostomsDeploy)
//            {
//                SendSignalMessage(eventName, channelName, eventParameter);
//            }
//            else
//            {
//                SendTopicMessage(eventName, eventParameter, channelName);
//            }

//        }


//        public static void SendTenantChannelMessage(string eventName, string eventParameter, int tenant)
//        {

//            string channelName = "Tenant" + tenant + "Channel";
//            if (LogitudeSettings.DeploymentStage == "Dev" || LogitudeSettings.DeploymentStage == "Test2" || LogitudeSettings.IsCostomsDeploy)
//            {
//                SendSignalMessage(eventName, channelName, eventParameter);
//            }
//            else
//            {
//                SendTopicMessage(eventName, eventParameter, channelName);
//            }

//        }

//        private static void SendSignalMessage(string eventName, string channelName, string eventParameter)
//        {
//            if (channelName != "LogBox")
//            {
//                LogitudeHubChannelEvent ev = new LogitudeHubChannelEvent()
//                {
//                    EventName = eventName,
//                    ChannelName = channelName,
//                    EventParameter = eventParameter,

//                };

//                SignalRHubEventPublisher.PublishLogitudeHubEvent(ev);
//            }
//            else
//            {
//                IHubProxy _hub;
//                var connection = new HubConnection(LogitudeSettings.LogitudeURL, new Dictionary<string, string> { { "UserName", eventParameter } });
//                _hub = connection.CreateHubProxy("LogBoxSignatureHub");
//                connection.Start().Wait();

//                _hub.Invoke("SignRequestReceived", eventParameter).Wait();
//            }
//        }

//        private static void SendTopicMessage(string eventName, string eventParameter, string channelName)
//        {

//            if (LogitudeSettings.IsCostomsDeploy)
//            {
//                AmitalDebuggerUtil.Break(AmitalDebuggerLevel.Error);
//                return;//onpremise - cause crush
//            }
//            using (TransactionScope scope = TransactionFactory.GetNewSerializableTransaction())//TransactionFactory.GetNewTransaction())
//            {
//                BrokeredMessage brMessage = new BrokeredMessage();
//                brMessage.Label = "SignaRHub";

//                brMessage.Properties["EventName"] = eventName;
//                brMessage.Properties["ChannelName"] = channelName;
//                brMessage.Properties["EventParameter"] = eventParameter;

//                // message.TimeToLive = new TimeSpan(0, 5, 0);
//                if (LogitudeSettings.DeploymentStage != "logitudepreproduction")
//                {
//                    TopicClient client = Microsoft.ServiceBus.Messaging.TopicClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), StorageAcountDetails.SignalRHubTopicName);


//                    client.Send(brMessage);
//                }
             

//                scope.Complete();
//            }
//        }
//    }


//}
