using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.Data.Repositories
{
    public class SystemMetadataLastUpdateRepository: IRepository<SystemMetadataLastUpdate,string>
    {
        IGlobalContext globalContext;
        public SystemMetadataLastUpdateRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public SystemMetadataLastUpdateRepository(IGlobalContext context)
        {
            globalContext = context;
        }

       

        public SystemMetadataLastUpdate GetSingleSystemMetadataLastUpdate(string id)
        {
            return (from a in context.SystemMetadataLastUpdates
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<SystemMetadataLastUpdate> GetAllSystemMetadataLastUpdates()
        {
            return from a in context.SystemMetadataLastUpdates
                   select a;
        }

        public void Add(SystemMetadataLastUpdate entity)
        {
            context.SystemMetadataLastUpdates.Add(entity);
        }

        public void Remove(SystemMetadataLastUpdate entity)
        {
            context.SystemMetadataLastUpdates.Attach(entity);
            context.SystemMetadataLastUpdates.Remove(entity);
        }

        public void Update(SystemMetadataLastUpdate entity)
        {
            context.SystemMetadataLastUpdates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SystemMetadataLastUpdate> All()
        {
            return context.SystemMetadataLastUpdates.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<SystemMetadataLastUpdate> GetMulti(IEntityKeyFields<SystemMetadataLastUpdate,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SystemMetadataLastUpdate GetSingle(IEntityKeyFields<SystemMetadataLastUpdate,string> entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}