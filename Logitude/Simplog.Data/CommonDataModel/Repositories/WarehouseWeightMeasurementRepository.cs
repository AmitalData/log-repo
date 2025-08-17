using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class WarehouseWeightMeasurementRepository : IRepository<WarehouseWeightMeasurement>
    {
        ICommonDataContext commonDataContext;

        public WarehouseWeightMeasurementRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public WarehouseWeightMeasurementRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public WarehouseWeightMeasurement GetSingleWarehouseWeightMeasurement(string code)
        {
            WarehouseWeightMeasurement instance = (from i in context.WarehouseWeightMeasurements
                                      where i.Code == code
                                      select i).FirstOrDefault();
            return instance;
        }

        public IQueryable<WarehouseWeightMeasurement> GetWarehouseWeightMeasurements()
        {
            return context.WarehouseWeightMeasurements;
        }

        public IQueryable<WarehouseWeightMeasurement> GetAll()
        {
            return context.WarehouseWeightMeasurements;
        }

        public void Add(WarehouseWeightMeasurement entity)
        {
            context.WarehouseWeightMeasurements.Add(entity);
        }

        public void Remove(WarehouseWeightMeasurement entity)
        {
            context.WarehouseWeightMeasurements.Attach(entity);
            context.WarehouseWeightMeasurements.Remove(entity);
        }

        public void Update(WarehouseWeightMeasurement entity)
        {
            context.WarehouseWeightMeasurements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<WarehouseWeightMeasurement> All()
        {
            return context.WarehouseWeightMeasurements.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        List<WarehouseWeightMeasurement> IRepository<WarehouseWeightMeasurement>.GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        WarehouseWeightMeasurement IRepository<WarehouseWeightMeasurement>.GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}