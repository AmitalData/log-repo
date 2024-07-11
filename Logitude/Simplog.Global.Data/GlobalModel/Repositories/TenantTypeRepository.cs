using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class TenantTypeRepository : IRepository<TenantType>
    {
        IGlobalContext globalContext;

        public TenantTypeRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public TenantTypeRepository()
        {
            globalContext = new GlobalContext();
        }

        public TenantTypeRepository(int tenant)
        {
            globalContext = GlobalContext.GetContext();
        }

        public TenantType GetSingleTenantType(string code)
        {
            string entityName = "TenantType" + code;
            TenantType entity = (from i in context.TenantTypes
                                 where i.Code == code
                                 select i).FirstOrDefault();

            if (CacheManager.CacheWrapper != null)
            {
                if (CacheManager.CacheWrapper.Get(entityName) == null && entity != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    entity = (TenantType)CacheManager.CacheWrapper.Get(entityName);
                }
            }

            return entity;
            //TenantType instance = (from i in context.TenantTypes
            //                     where i.Code == code                                 
            //                     select i).FirstOrDefault();
            //return instance;
        }

        public IQueryable<TenantType> GetTenantTypes()
        {
            return context.TenantTypes;
        }

        public IQueryable<TenantType> GetAll()
        {
            return context.TenantTypes;
        }

        public void Add(TenantType entity)
        {
            context.TenantTypes.Add(entity);
        }

        public void Remove(TenantType entity)
        {
            context.TenantTypes.Attach(entity);
            context.TenantTypes.Remove(entity);
        }

        public void Update(TenantType entity)
        {
            context.TenantTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TenantType> All()
        {
            return context.TenantTypes.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<TenantType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TenantType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}