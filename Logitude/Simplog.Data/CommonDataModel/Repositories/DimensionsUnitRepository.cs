using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DimensionsUnitRepository:IRepository<DimensionsUnit>
    {
        ICommonDataContext commonDataContext;

        public DimensionsUnitRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public DimensionsUnitRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public DimensionsUnitRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<DimensionsUnit> GetDimensionsUnits()
        {
            return context.DimensionsUnits;
        }

        public IQueryable<DimensionsUnit> GetAll()
        {
            return context.DimensionsUnits;
        }

        public DimensionsUnit GetSingleDimensionsUnit(string code)
        {
            return (from record in context.DimensionsUnits where record.Code == code select record).FirstOrDefault();
        }

        public void Add(DimensionsUnit entity)
        {
            context.DimensionsUnits.Add(entity);
        }

        public void Remove(DimensionsUnit entity)
        {
            context.DimensionsUnits.Attach(entity);
            context.DimensionsUnits.Remove(entity);
        }

        public void Update(DimensionsUnit entity)
        {
            context.DimensionsUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DimensionsUnit> All()
        {
            return context.DimensionsUnits.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<DimensionsUnit> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DimensionsUnit GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
