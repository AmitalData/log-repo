using Logitude.Server.Tools.QueueService;
using System.Collections.Generic;

namespace Logitude.BL.Workflow
{
    public class TaskDoneQueueMessage
    {
        public int Tenant { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string Type { get; set; }

        private const string QueueName = "TaskDone";

        public void Produce()
        {
            AddQueueMessage();
        }

        private void AddQueueMessage()
        {
            IQueueService queueservice = new DbQueueService();
            queueservice.InitializeQueue(QueueName, 0);
            var queueMessage = new Dictionary<string, string>() {
                { "Tenant", Tenant.ToString()},
                { "Entity", Entity },
                { "EntityId", EntityId },
                { "Type", Type }
            };

            queueservice.Send(queueMessage, Tenant);
        }
    }
}
