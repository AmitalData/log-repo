using System.Collections.Generic;
using Logitude.Server.Tools.QueueService;
using Logitude.Workflow.BL.EntityPMs;

namespace Logitude.Workflow.BL.QueueMessages
{
    public class RenewServiceProviderSubscriptionQueueMessage
    {
        public ServiceProviderSubscriptionPM ServiceProviderSubscription { get; set; }

        private const string QueueName = "RenewServiceProviderSubscription";

        public void Produce()
        {
            if (ServiceProviderSubscription != null && !string.IsNullOrEmpty(ServiceProviderSubscription.Id))
            {
                AddQueueMessage();
            }
        }

        private void AddQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(QueueName, 0);
            Dictionary<string, string> message = new Dictionary<string, string>() {
                { "Tenant", ServiceProviderSubscription.Tenant.ToString()},
                { "ServiceProviderSubscriptionId", ServiceProviderSubscription.Id }
            };
            queueservice.Send(message, ServiceProviderSubscription.Tenant, NextRunDate: ServiceProviderSubscription.SubscriptionExpirationDateTime);
        }
    }
}