
using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class GeneralLockRepository:IRepository<GeneralLock>
    {
        IWebFreightContext webFreightContext;

        public GeneralLockRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public GeneralLockRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public GeneralLockRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public GeneralLock GetSingleGeneralLock(string generalKey, int tenant)
        {


            var poco = (from a in context.GeneralLocks
                        where a.GeneralKey == generalKey && a.Tenant == tenant
                        select a).FirstOrDefault();

            
            return poco;
        }
        public GeneralLock GetSingleGeneralLockNOWAIT(string generalKey, int tenant)
        {

            return (context as DbContextBase)
                .GetListNOWAITWhere<GeneralLock>(rec => rec.GeneralKey == generalKey && rec.Tenant == tenant).FirstOrDefault(); ;
        }
        public void FastDelete(string generalKey, int tenant)
        {
            (context as DbContextBase)
                .DeleteWhere<GeneralLock>(rec => rec.GeneralKey == generalKey && rec.Tenant == tenant);
        }
        public void FastDeleteIfCreated15MinOld(string generalKey, int tenant)
        {
            DateTime createdAtb4_15min = TenantServerConfigration.GetCurrentDateTime(tenant).AddMinutes(-15);

            //DateTime old = datetime.
            (context as DbContextBase)
                .DeleteWhere<GeneralLock>(rec => rec.GeneralKey == generalKey && rec.Tenant == tenant 
                && rec.CreatedAt < createdAtb4_15min
                );
        }

        public IQueryable<GeneralLock> GetGeneralLocks()
        {
            return context.GeneralLocks;
        }
  
        public void Add(GeneralLock entity)
        {
            context.GeneralLocks.Add(entity);
        }

        public void Remove(GeneralLock entity)
        {
            context.GeneralLocks.Attach(entity);
            context.GeneralLocks.Remove(entity);
        }

        public void Update(GeneralLock entity)
        {
            context.GeneralLocks.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GeneralLock> All()
        {
            return context.GeneralLocks.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<GeneralLock> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public GeneralLock GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }



        
    }
}