using System.Collections.Generic;
using Logitude.Server.Tools.QueueService;

namespace Logitude.Workflow.BL.QueueMessages
{
    public class NewOffice365MessageQueueMessage
    {
        public string WorkflowNumber { get; set; }
        public int Tenant { get; set; }
        public string MessageId { get; set; }

        private const string QueueName = "NewOffice365Message";

        public void Produce()
        {
            if (!string.IsNullOrEmpty(WorkflowNumber) && !string.IsNullOrEmpty(MessageId))
            {
                AddQueueMessage();
            }
        }

        private void AddQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(QueueName, 0);
            Dictionary<string, string> message = new Dictionary<string, string>() {
                { "WorkflowNumber", WorkflowNumber},
                { "Tenant", Tenant.ToString()},
                { "MessageId", MessageId }
            };
            queueservice.Send(message, Tenant);
        }
    }
}