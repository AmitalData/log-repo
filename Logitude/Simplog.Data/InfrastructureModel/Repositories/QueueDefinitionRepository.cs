using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QueueDefinitionRepository:IRepository<QueueDefinition>
    {

        IWebFreightContext webFreightContext;
        public QueueDefinitionRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public QueueDefinitionRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public QueueDefinitionRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
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

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QueueDefinition> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QueueDefinition GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}