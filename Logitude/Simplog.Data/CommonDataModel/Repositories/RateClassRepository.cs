using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
namespace Simplog.Data.CommonDataModel.Repositories
{
    public class RateClassRepository:IRepository<RateClass>
    {
        ICommonDataContext commonDataContext;

        public RateClassRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public RateClassRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public RateClassRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<RateClass> GetRateClasses()
        {
            return context.RateClasses;
        }

        public IQueryable<RateClass> GetAll()
        {
            return context.RateClasses;
        }

        public RateClass GetSingleRateClass(string code)
        {
            return (from a in context.RateClasses where a.Code == code select a).FirstOrDefault();
        }

        public void Add(RateClass entity)
        {
            context.RateClasses.Add(entity);
        }

        public void Remove(RateClass entity)
        {
            context.RateClasses.Attach(entity);
            context.RateClasses.Remove(entity);
        }

        public void Update(RateClass entity)
        {
            context.RateClasses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RateClass> All()
        {
            return context.RateClasses.ToList();

        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<RateClass> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public RateClass GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
