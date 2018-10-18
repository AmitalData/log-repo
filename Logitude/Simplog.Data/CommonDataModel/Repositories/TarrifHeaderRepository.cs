using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TarrifHeaderRepository:IRepository<TarrifHeader>
    {
        ICommonDataContext commonDataContext;

        public TarrifHeaderRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TarrifHeaderRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TarrifHeaderRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public TarrifHeader GetSingleTarrifHeader(string id)
        {
            return (from a in context.TarrifHeaders
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public TarrifHeader GetSingleTarrifHeader(string id,int tenant)
        {
            return (from a in context.TarrifHeaders
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<TarrifHeader> GetTarrifHeadersByTenant(int tenant)
        {
            return from a in context.TarrifHeaders
                   where a.Tenant == tenant
                   select a;
        }
        public IQueryable<TarrifHeader> GetTarrifHeaders(int tenant)
        {
            return from a in context.TarrifHeaders
                   where a.Tenant == tenant
                   select a;
        }

        
        public void Add(TarrifHeader entity)
        {
            context.TarrifHeaders.Add(entity);
        }

        public void Remove(TarrifHeader entity)
        {
            context.TarrifHeaders.Attach(entity);
            context.TarrifHeaders.Remove(entity);
        }

        public void Update(TarrifHeader entity)
        {
            context.TarrifHeaders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarrifHeader> All()
        {
            return context.TarrifHeaders.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TarrifHeader> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TarrifHeader GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}