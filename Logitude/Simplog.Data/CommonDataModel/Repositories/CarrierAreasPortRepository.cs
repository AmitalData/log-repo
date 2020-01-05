using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CarrierAreasPortRepository:IRepository<CarrierAreasPort>
    {
        ICommonDataContext commonDataContext;

        public CarrierAreasPortRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CarrierAreasPortRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CarrierAreasPortRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
      
 

        public CarrierAreasPort GetSingleCarrierAreasPort(string id, int tenant)
        {
            return (from record in context.CarrierAreasPorts where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public List<CarrierAreasPort> GetCarrierAreasPortByAreaId(string areaId, int tenant)
        {
            return (from record in context.CarrierAreasPorts.Include("Port") where record.CarrierAreaId == areaId && record.Tenant == tenant select record).ToList();
        }


        public void Add(CarrierAreasPort entity)
        {
            context.CarrierAreasPorts.Add(entity);
        }

        public void Remove(CarrierAreasPort entity)
        {
            context.CarrierAreasPorts.Attach(entity);
            context.CarrierAreasPorts.Remove(entity);
        }

        public void Update(CarrierAreasPort entity)
        {
            context.CarrierAreasPorts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CarrierAreasPort> All()
        {
            return context.CarrierAreasPorts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CarrierAreasPort> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CarrierAreasPort GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}