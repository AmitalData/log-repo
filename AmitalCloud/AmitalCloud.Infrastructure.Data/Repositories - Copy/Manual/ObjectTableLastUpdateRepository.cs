using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AmitalCloud.Infrastructure.Data;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class ObjectTableLastUpdateRepository:IRepository<ObjectTableLastUpdate, string>
    {
        IAmitalCloudContext amitalCloudContext;
        public ObjectTableLastUpdateRepository(IAmitalCloudContext context)
        {
            amitalCloudContext = context;

        }
        public ObjectTableLastUpdateRepository(int tenant)
        {
            amitalCloudContext = AmitalCloudContext.GetContext(tenant);
        }
        public ObjectTableLastUpdateRepository()
        {
               amitalCloudContext = new AmitalCloudContext(); 
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

        public IAmitalCloudContext context
        {
            get { return amitalCloudContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ObjectTableLastUpdate> GetMulti(IEntityKeyFields<ObjectTableLastUpdate,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ObjectTableLastUpdate GetSingle(IEntityKeyFields<ObjectTableLastUpdate,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}