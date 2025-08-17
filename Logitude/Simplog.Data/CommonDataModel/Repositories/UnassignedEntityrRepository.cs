using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Simplog.Data.CommonDataModel.Repositories
{
    public class UnassignedEntityRepository : IRepository<UnassignedEntity>
    {
        ICommonDataContext commonDataContext;
        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public UnassignedEntityRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
        public UnassignedEntityRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<UnassignedEntity> GetUnassignedEntitys(int tenant)
        {
            return (from record in context.UnassignedEntitys.Include("ObjectTable") where record.Tenant == tenant select record);
        }

        public UnassignedEntity GetSingleUnassignedEntity(string id, int tenant)
        {
            return (from record in context.UnassignedEntitys.Include("ObjectTable") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(UnassignedEntity entity)
        {
            context.UnassignedEntitys.Add(entity);
        }

        public void Update(UnassignedEntity entity)
        {
            context.UnassignedEntitys.Attach(entity);
            context.SetAsModified(entity);
        }

        public void Remove(UnassignedEntity entity)
        {
            context.UnassignedEntitys.Attach(entity);
            context.UnassignedEntitys.Remove(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<UnassignedEntity> All()
        {
            return context.UnassignedEntitys.ToList();
        }

        public List<UnassignedEntity> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public UnassignedEntity GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

    }
}
