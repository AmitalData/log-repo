using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EntityLastUpdateRepository : IRepository<EntityLastUpdate>
    {
        IWebFreightContext webFreightContext;
        public EntityLastUpdateRepository()
        {
            webFreightContext = new WebFreightContext();

        }

        public EntityLastUpdateRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public EntityLastUpdateRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public bool DoesEntityHasUpdates(string entityId, string objectTableId, int tenant, string entityGUID)
        {
            bool hasUpdate = false;
            hasUpdate = (from a in context.EntityLastUpdates
                         where a.EntityId == entityId && a.ObjectTableId == objectTableId && a.Tenant == tenant && a.EntityGUID != entityGUID
                         select a).Any();
            return hasUpdate;
        }

        public IQueryable<EntityLastUpdate> GetEntityLastUpdateForUser(string userId, int tenant)
        {
            DateTime present = DateTime.Now.Date;
            IQueryable<EntityLastUpdate> EntityLastUpdates = from a in context.EntityLastUpdates
                                                             where a.UpdatedByUserId == userId && a.Tenant == tenant && a.UpdateDate < present
                                                             select a;
            return EntityLastUpdates;
        }

        public EntityLastUpdate GetSingleEntityLastUpdate(string id)
        {
            return (from a in context.EntityLastUpdates
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public EntityLastUpdate GetSingleEntityLastUpdateByEntityId(string entityId)
        {
            return (from a in context.EntityLastUpdates
                    where a.EntityId == entityId
                    select a).FirstOrDefault();
        }



        public void Add(EntityLastUpdate entity)
        {
            context.EntityLastUpdates.Add(entity);
        }

        public void Remove(EntityLastUpdate entity)
        {
            context.EntityLastUpdates.Attach(entity);
            context.EntityLastUpdates.Remove(entity);
        }

        public void Update(EntityLastUpdate entity)
        {
            context.EntityLastUpdates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntityLastUpdate> All()
        {
            return context.EntityLastUpdates.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<EntityLastUpdate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EntityLastUpdate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
