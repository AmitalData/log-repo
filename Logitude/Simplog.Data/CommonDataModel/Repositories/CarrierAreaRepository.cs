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
    public class CarrierAreaRepository:IRepository<CarrierArea>
    {
        ICommonDataContext commonDataContext;

        public CarrierAreaRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CarrierAreaRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CarrierAreaRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CarrierArea> GetCarrierAreas(int tenant)
        {
            return (from record in context.CarrierAreas where record.Tenant == tenant select record);
        }

        public CarrierArea GetSingleCarrierArea(string id, int tenant)
        {
            return (from record in context.CarrierAreas where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public void Add(CarrierArea entity)
        {
            context.CarrierAreas.Add(entity);
        }

        public void Remove(CarrierArea entity)
        {
            context.CarrierAreas.Attach(entity);
            context.CarrierAreas.Remove(entity);
        }

        public void Update(CarrierArea entity)
        {
            context.CarrierAreas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CarrierArea> All()
        {
            return context.CarrierAreas.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CarrierArea> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CarrierArea GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}