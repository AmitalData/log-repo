using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class WeightUnitRepository:IRepository<WeightUnit>
    {
        ICommonDataContext commonDataContext;

        public WeightUnitRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public WeightUnitRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<WeightUnit> GetWeightUnits()
        {
            return context.WeightUnits;
        }

        public IQueryable<WeightUnit> GetAll()
        {
            return context.WeightUnits;
        }

        public WeightUnit GetSingleWeightUnit(string code)
        {
            return (from a in context.WeightUnits where a.Code == code select a).FirstOrDefault();
        }

        public void Add(WeightUnit entity)
        {
            context.WeightUnits.Add(entity);
        }

        public void Remove(WeightUnit entity)
        {
            context.WeightUnits.Attach(entity);
            context.WeightUnits.Remove(entity);
        }

        public void Update(WeightUnit entity)
        {
            context.WeightUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WeightUnit> All()
        {
            return context.WeightUnits.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<WeightUnit> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public WeightUnit GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
