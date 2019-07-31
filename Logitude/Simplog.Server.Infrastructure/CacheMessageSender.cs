using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using Microsoft.WindowsAzure;
using Simplog.Server.Infrastructure.Azure;

namespace Simplog.Server.Infrastructure
{
    public class CacheMessageSender
    {

        public static void SendMessageToTopic(string Key)
        {
            if (LogitudeSettings.IsCostomsDeploy || LogitudeSettings.DeploymentStage == "amitalstorage") return;
            BrokeredMessage message = new BrokeredMessage();
            message.Label = "InvalidateCache";
            message.Properties["Key"] = Key;
            // message.TimeToLive = new TimeSpan(0, 5, 0);
            TopicClient client = Microsoft.ServiceBus.Messaging.TopicClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(LogitudeSettings.DeploymentStage), StorageAcountDetails.DataCacheTopicName);


            client.Send(message);

        }




    }
}
