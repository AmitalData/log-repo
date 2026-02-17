using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SharedLogisticsUpdateRepository:IRepository<SharedLogisticsUpdate>
    {
        IWebFreightContext webFreightContext;
        public SharedLogisticsUpdateRepository(int tenant)
        {
            webFreightContext =  WebFreightContext.GetContext(tenant);
        }

        public SharedLogisticsUpdateRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public SharedLogisticsUpdateRepository()
        {
            // TODO: Complete member initialization
        }

        public SharedLogisticsUpdate GetSingleSharedLogisticUpdate(string id)
        {
            return (from a in context.SharedLogisticsUpdates
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public List<SharedLogisticsUpdate> GetSharedLogisticsUpdatesForEntity(string entityid)
        {
            return (from a in context.SharedLogisticsUpdates
                    where a.EntityId == entityid
                    select a).ToList();
        }

      
        public IQueryable<SharedLogisticsUpdate> GetSharedLogisticsUpdates()
        {
            return (context.SharedLogisticsUpdates);
        }
     
        public void Add(SharedLogisticsUpdate entity)
        {
            context.SharedLogisticsUpdates.Add(entity);
        }

        public void Remove(SharedLogisticsUpdate entity)
        {
            context.SharedLogisticsUpdates.Attach(entity);
            context.SharedLogisticsUpdates.Remove(entity);
        }

        public void Update(SharedLogisticsUpdate entity)
        {
            context.SharedLogisticsUpdates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedLogisticsUpdate> All()
        {
            return context.SharedLogisticsUpdates.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<SharedLogisticsUpdate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SharedLogisticsUpdate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}