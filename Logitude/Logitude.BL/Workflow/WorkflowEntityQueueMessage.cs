using Logitude.BL.Security;
using Logitude.Server.Tools.QueueService;
using System.Collections.Generic;

namespace Logitude.BL.Workfkow
{
    public class WorkflowEntityQueueMessage
    {
        public int Tenant { get; set; }
        public string Entity { get; set; }
        public string EntityId { get; set; }
        public string AuditLogId { get; set; }
        public string Type { get; set; }
        public bool IsCustom { get; set; }

        private const string QueueName = "WorkflowEntity";
        private const string ObjectTableName = "WorkFlow";
        private const string FeatureCode = "Module";

        public void Produce()
        {
            if (!SecurityUtility.CheckFeature(ObjectTableName, FeatureCode, Tenant))
            {
                return;
            }

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
                { "AuditLogId", AuditLogId },
                { "Type", Type },
                { "IsCustom", IsCustom.ToString() }
            };

            queueservice.Send(queueMessage, Tenant);
        }
    }
}
