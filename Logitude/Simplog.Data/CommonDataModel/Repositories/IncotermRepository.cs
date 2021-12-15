using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Web;
using Simplog.Server.Infrastructure.Helpers;
using System;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class IncotermRepository:IRepository<Incoterm>
    {
        ICommonDataContext commonDataContext;

        public IncotermRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public IncotermRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IncotermRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Incoterm> GetIncoterms(int tenant)
        {
            return (from record in context.Incoterms where record.Tenant == tenant select record);
        }

        public Incoterm GetSingleIncoterm(string id, int tenant)
        {
            return (from record in context.Incoterms where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public static Incoterm GetSingleFromCache(string id, int tenant)
        {
            string entityName = "Incoterm" + id + tenant;


            Incoterm entity = (Incoterm)CacheManager.CacheWrapper.Get(entityName);

            if(entity == null)
            {
                ICommonDataContext context = CommonDataContext.GetContext(tenant);
                entity = new IncotermRepository().GetSingleIncoterm(id, tenant);


                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
            }

            return entity;
        }

        public Incoterm GetSingleIncotermByCode(string code, int tenant)
        {
            return (from record in context.Incoterms where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Incoterm GetIncotermByCode(string code, int tenant)
        {
            return (from record in context.Incoterms where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public string GetIncotermIdByCode(string code, int tenant)
        {
   
            string entityId = (from a in context.Incoterms
                               where a.Tenant == tenant && a.Code == code
                        select a.Id).FirstOrDefault();
            return entityId;
        }
        




        public void Add(Incoterm entity)
        {
            context.Incoterms.Add(entity);
        }

        public void Remove(Incoterm entity)
        {
            context.Incoterms.Remove(entity);
        }

        public void Update(Incoterm entity)
        {
            context.Incoterms.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Incoterm> All()
        {
            return context.Incoterms.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Incoterm> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Incoterm GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}