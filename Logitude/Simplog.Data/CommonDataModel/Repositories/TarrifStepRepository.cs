using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TarrifStepRepository:IRepository<TarrifStep>
    {
        ICommonDataContext commonDataContext;


        public TarrifStepRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TarrifStepRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public TarrifStep GetSingleTarrifStep(string id,int tenant = 0)
        {
            return (from a in context.TarrifSteps
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<TarrifStep> GetTarrifStepsByTenant(int tenant)
        {
            return from a in context.TarrifSteps
                   where a.Tenant == tenant
                   select a;
        }

        public IQueryable<TarrifStep> GetTarrifSteps(int tenant)
        {
            return from a in context.TarrifSteps
                   where a.Tenant == tenant
                   select a;
        }

        

        public void Add(TarrifStep entity)
        {
            context.TarrifSteps.Add(entity);
        }

        public void Remove(TarrifStep entity)
        {
            context.TarrifSteps.Attach(entity);
            context.TarrifSteps.Remove(entity);
        }

        public void Update(TarrifStep entity)
        {
            context.TarrifSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TarrifStep> All()
        {
            return context.TarrifSteps.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext ; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<TarrifStep> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TarrifStep GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}