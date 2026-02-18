using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TarrifFromToRepository:IRepository<TarrifFromTo>
    {
        ICommonDataContext commonDataContext;

        public TarrifFromToRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public TarrifFromToRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TarrifFromToRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public TarrifFromTo GetSingleTarrifFromTo(string id, int tenant = 0)
        {
            return (from a in context.TarrifFromToes.Include("Port").Include("Country")
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<TarrifFromTo> GetTarrifFromToesByTenant(int tenant)
        {
            return from a in context.TarrifFromToes.Include("Port").Include("Country")
                   where a.Tenant == tenant
                   select a;
        }

        public IQueryable<TarrifFromTo> GetTarrifFromToes(int tenant)
        {
            return from a in context.TarrifFromToes.Include("Port").Include("Country")
                   where a.Tenant == tenant
                   select a;
        }


        
        public void Add(TarrifFromTo entity)
        {
            context.TarrifFromToes.Add(entity);
        }

        public void Remove(TarrifFromTo entity)
        {
            context.TarrifFromToes.Attach(entity);
            context.TarrifFromToes.Remove(entity);
        }

        public void Update(TarrifFromTo entity)
        {
            context.TarrifFromToes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarrifFromTo> All()
        {
            return context.TarrifFromToes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TarrifFromTo> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TarrifFromTo GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}