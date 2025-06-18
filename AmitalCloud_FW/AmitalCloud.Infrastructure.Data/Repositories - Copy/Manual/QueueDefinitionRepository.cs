using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class QueueDefinitionRepository:IRepository<QueueDefinition,string>
    {

        IAmitalCloudContext currentContext;
        public QueueDefinitionRepository(IAmitalCloudContext context)
        {
            currentContext = context;

        }
        public QueueDefinitionRepository()
        {
            currentContext = new AmitalCloudContext();
        }
        public QueueDefinitionRepository(int tenant)
        {
            currentContext = AmitalCloudContext.GetContext(tenant);
        }
        public IQueryable<QueueDefinition> GetQueueDefinitions()
        {
            return context.QueueDefinitions;
        }

        public QueueDefinition GetSingleQueueDefinition(string code)
        {
            return (from a in context.QueueDefinitions
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(QueueDefinition entity)
        {
            context.QueueDefinitions.Add(entity);
        }

        public void Remove(QueueDefinition entity)
        {
            context.QueueDefinitions.Attach(entity);
            context.QueueDefinitions.Remove(entity);
        }

        public void Update(QueueDefinition entity)
        {
            context.QueueDefinitions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QueueDefinition> All()
        {
            return context.QueueDefinitions.ToList();
        }

        public IAmitalCloudContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QueueDefinition> GetMulti(IEntityKeyFields<QueueDefinition,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueueDefinition GetSingle(IEntityKeyFields<QueueDefinition,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}