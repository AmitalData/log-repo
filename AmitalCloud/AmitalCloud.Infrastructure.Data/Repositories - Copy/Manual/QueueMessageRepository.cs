using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class QueueMessageRepository : IRepository<QueueMessage,string>
    {

        IAmitalCloudContext currentContext;
        public QueueMessageRepository(IAmitalCloudContext context)
        {
            currentContext = context;

        }
        public QueueMessageRepository()
        {
            currentContext = new AmitalCloudContext();
        }
        public QueueMessageRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }
        public IQueryable<QueueMessage> GetQueueMessages()
        {
            return context.QueueMessages;
        }

        //public IQueryable<QueueMessage> GetQueueMessage()
        //{
        //    return context.QueueMessages.Where(a => a.QueueDefinitionCode == "ImportersShipmentsBatchQueue" || a.QueueDefinitionCode == "ImportersShipmentDocumentsBatchQueue");
        //}

        public QueueMessage GetSingleQueueMessage(string id)
        {
            long? longId = null;
            if (id != null)
            {
                longId = long.Parse(id);
            }
            return (from a in context.QueueMessages
                    where a.Id == longId
                    select a).FirstOrDefault();
        }
        public QueueMessage GetSingleQueueMessage(string entityId,string entityCode)
        {
            return (from a in context.QueueMessages
                    where a.EntityId == entityId && a.EntityCode == entityCode
                    select a).FirstOrDefault();
        }

        public QueueMessage GetSingleQueueMessageByReportId(string reportId, int tenant)
        {
            return (from a in context.QueueMessages
                    where a.Tenant == tenant &&
                    a.MessageBody.Contains(reportId) &&
                    a.QueueDefinitionCode == "ReportExecutionLogQueue"
                    select a).FirstOrDefault();
        }
        public void Add(QueueMessage entity)
        {
            context.QueueMessages.Add(entity);
        }

        public void Remove(QueueMessage entity)
        {
            context.QueueMessages.Attach(entity);
            context.QueueMessages.Remove(entity);
        }

        public void Update(QueueMessage entity)
        {
            context.QueueMessages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueueMessage> All()
        {
            return context.QueueMessages.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QueueMessage> GetMulti(IEntityKeyFields<QueueMessage,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueueMessage GetSingle(IEntityKeyFields<QueueMessage,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}