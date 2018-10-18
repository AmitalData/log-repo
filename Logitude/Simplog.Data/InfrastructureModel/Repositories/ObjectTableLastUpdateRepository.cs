using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectTableLastUpdateRepository:IRepository<ObjectTableLastUpdate>
    {
        IWebFreightContext webFreightContext;
        public ObjectTableLastUpdateRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public ObjectTableLastUpdateRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public ObjectTableLastUpdateRepository()
        {
               webFreightContext=new WebFreightContext(); 
        }
        public void Add(ObjectTableLastUpdate entity)
        {
            context.ObjectTableLastUpdates.Add(entity);
        }

        public IQueryable<ObjectTableLastUpdate> GetObjectTableLastUpdates(int tenant)
        {
            return context.ObjectTableLastUpdates.Where(t=>t.Tenant == tenant);
        }

        public ObjectTableLastUpdate GetSingleObjectTableLastUpdate(string objecttableId,int tenant)
        {
            return (from a in  context.ObjectTableLastUpdates
                 where a.Tenant == tenant && a.ObjectTableId == objecttableId
                        select a).FirstOrDefault();
        }

        public ObjectTableLastUpdate GetSingleObjectTableLastUpdateByTableName(string tableName, int tenant)
        {
            return (from a in context.ObjectTableLastUpdates.Include("ObjectTable")
                    where a.Tenant == tenant && a.ObjectTable.Name == tableName
                    select a).FirstOrDefault();
        }

        public void Remove(ObjectTableLastUpdate entity)
        {
            context.ObjectTableLastUpdates.Attach(entity);
            context.ObjectTableLastUpdates.Remove(entity);
        }

        public void Update(ObjectTableLastUpdate entity)
        {
            context.ObjectTableLastUpdates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ObjectTableLastUpdate> All()
        {
            return context.ObjectTableLastUpdates.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableLastUpdate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableLastUpdate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}