using Microsoft.ServiceBus.Messaging;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Server.Infrastructure.Helpers
{
    public class ServiceBusQueueHelper
    {
        public static QueueClient CreateCustomerQueue(int tenant)
        {
            string queuename = "customeractivatioqueue" + tenant;
            if (!StorageAcountDetails.NameSpaceManager.QueueExists(queuename))
            {
                QueueDescription queueDescription = new QueueDescription(queuename);
                queueDescription.MaxSizeInMegabytes = 5120;
                queueDescription.MaxDeliveryCount = 99999;
                queueDescription.LockDuration = new TimeSpan(0, 2, 0);

                //queueDescription.LockDuration
                //queueDescription.DefaultMessageTimeToLive = new TimeSpan(3, 1, 0);

                StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            }

            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(queuename, ReceiveMode.PeekLock);

            return client;
        }


        public static QueueClient CreateContactUnseenEntityQueue(int tenant)
        {
            

            string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("contactunseenentityqueue");

            if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailqueueName))
            {

                QueueDescription queueDescription = new QueueDescription(emailqueueName);
                queueDescription.MaxSizeInMegabytes = 5120;
                queueDescription.MaxDeliveryCount = 99999;
                queueDescription.LockDuration = new TimeSpan(0, 2, 0);
                StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            }

            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName, ReceiveMode.PeekLock);

            return client;
        }


        public static QueueClient CreateMobileNotificationsLogQueue(int tenant)
        {
         
        
            string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("mobilenotificationlogqueue");

            if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailqueueName))
            {

                QueueDescription queueDescription = new QueueDescription(emailqueueName);
                queueDescription.MaxSizeInMegabytes = 5120;
                queueDescription.MaxDeliveryCount = 99999;
                queueDescription.LockDuration = new TimeSpan(0, 2, 0);
                StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            }

            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName, ReceiveMode.PeekLock);

            return client;
        }

        public static QueueClient CreateEntityChangeQueue(int tenant)
        {

            string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("entitychangequeue");

            if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailqueueName))
            {

                QueueDescription queueDescription = new QueueDescription(emailqueueName);
                queueDescription.MaxSizeInMegabytes = 5120;
                queueDescription.MaxDeliveryCount = 99999;
                queueDescription.LockDuration = new TimeSpan(0, 2, 0);
                StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            }

            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName, ReceiveMode.PeekLock);

            return client;
        }


        public static QueueClient CreateMessagesTransmissionLogQueue(int tenant)
        {
            string emailqueueName = WebFreightEntryPoint.GetQueueByEnviroment("transmissionlogqueue");

            if (!StorageAcountDetails.NameSpaceManager.QueueExists(emailqueueName))
            {
                QueueDescription queueDescription = new QueueDescription(emailqueueName);
                queueDescription.MaxSizeInMegabytes = 5120;
                queueDescription.MaxDeliveryCount = 99999;
                queueDescription.LockDuration = new TimeSpan(0, 2, 0);
                StorageAcountDetails.NameSpaceManager.CreateQueue(queueDescription);
            }

            QueueClient client = StorageAcountDetails.CreateServiceBusQueueClient(emailqueueName, ReceiveMode.PeekLock);

            return client;
        }
    }
}
