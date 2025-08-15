using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TemperatureUnitRepository : IRepository<TemperatureUnit>
    {
        ICommonDataContext commonDataContext;
        public TemperatureUnitRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public TemperatureUnitRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TemperatureUnit> GetTemperatureUnits()
        {
            return context.TemperatureUnits;
        }

        public IQueryable<TemperatureUnit> GetAll()
        {
            return context.TemperatureUnits;
        }

        public TemperatureUnit GetSingleTemperatureUnit(string code)
        {
            return (from a in context.TemperatureUnits where a.Code == code select a).FirstOrDefault();
        }

        public void Add(TemperatureUnit entity)
        {
            context.TemperatureUnits.Add(entity);
        }

        public void Remove(TemperatureUnit entity)
        {
            context.TemperatureUnits.Attach(entity);
            context.TemperatureUnits.Remove(entity);
        }

        public void Update(TemperatureUnit entity)
        {
            context.TemperatureUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TemperatureUnit> All()
        {
            return context.TemperatureUnits.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<TemperatureUnit> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public TemperatureUnit GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
