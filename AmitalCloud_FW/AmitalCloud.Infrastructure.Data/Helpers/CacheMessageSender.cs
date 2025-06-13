using AmitalCloud.Infrastructure.Data.Azure;
using AmitalCloud.Infrastructure.Domain.DataContracts;
using Microsoft.ServiceBus.Messaging;

namespace AmitalCloud.Infrastructure.Data.Helpers
{
    public class CacheMessageSender
    {

        public static void SendMessageToTopic(string Key)
        {
            if (AmitalCloudSettings.IsCostomsDeploy || AmitalCloudSettings.DeploymentStage == "amitalstorage") return;
            BrokeredMessage message = new BrokeredMessage();
            message.Label = "InvalidateCache";
            message.Properties["Key"] = Key;
            // message.TimeToLive = new TimeSpan(0, 5, 0);
            TopicClient client = Microsoft.ServiceBus.Messaging.TopicClient.CreateFromConnectionString(StorageAcountDetails.GetSettingByName(AmitalCloudSettings.DeploymentStage), StorageAcountDetails.DataCacheTopicName);


            client.Send(message);

        }




    }
}
