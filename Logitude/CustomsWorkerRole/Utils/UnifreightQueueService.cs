using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.Resolvers;
using Logitude.Server.Tools;
using System.Collections.Generic;

namespace CustomsWorkerRole.Utils
{
    public class UnifreightQueueService
    {
        public static void Insert(int tenant, int priority, string queueName, string subject, string storageFolder, string action, string tableName, string fileNos)
        {
            List<QueueTask> queueTasks = CreateQueueTasks(fileNos, action);
            CommunicationsParams communicationsParams = CreateCommunicationParams(tenant, tableName, queueName, priority, subject, storageFolder, queueTasks);
            Communications.AddCommunicationLog(communicationsParams);
        }

        private static List<QueueTask> CreateQueueTasks(string data, string action)
        {
            return new List<QueueTask>
            {
                new QueueTask()
                {
                    Action = action, Parameters = new List<Parameter>()
                    {
                        new Parameter()
                        {
                            Name = action,
                            Order = 1,
                            Value = data
                        }
                    }
                }
            };
        }

        private static CommunicationsParams CreateCommunicationParams(int tenant, string tableName, string queueName, int priority, string subject, string storageFolder, List<QueueTask> queueTasks)
        {
            CommunicationsParams comParams = new CommunicationsParams()
            {
                Tenant = tenant,
                CommunicationLogTypeCode = "Q",
                QueueName = queueName,
                Priority = priority,
                InOut = "O",
                Status = "W",
                LoggingUserId = LoggedContactResolver.GetLoggedContact(tenant)?.Id,
                LoggingObjectTableId = ObjectTableQuery.GetObjectTableByCode(tableName, 0)?.Id,
                LoggingEntityId = null,
                Subject = subject,
                FolderName = storageFolder,
                ByteData = LogitudeXmlSerializer.SerializeObject(queueTasks)
            };
            return comParams;
        }

    }
}
