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
    public class AirlineAreasPortRepository:IRepository<AirlineAreasPort>
    {
        ICommonDataContext commonDataContext;

        public AirlineAreasPortRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AirlineAreasPortRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AirlineAreasPortRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
      
 

        public AirlineAreasPort GetSingleAirlineAreasPort(string id, int tenant)
        {
            return (from record in context.AirlineAreasPorts.Include("Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public List<AirlineAreasPort> GetAirlineAreasPortByAreaId(string areaId, int tenant)
        {
            return (from record in context.AirlineAreasPorts where record.AirlineAreaId == areaId && record.Tenant == tenant select record).ToList();
        }


        public void Add(AirlineAreasPort entity)
        {
            context.AirlineAreasPorts.Add(entity);
        }

        public void Remove(AirlineAreasPort entity)
        {
            context.AirlineAreasPorts.Attach(entity);
            context.AirlineAreasPorts.Remove(entity);
        }

        public void Update(AirlineAreasPort entity)
        {
            context.AirlineAreasPorts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AirlineAreasPort> All()
        {
            return context.AirlineAreasPorts.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AirlineAreasPort> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AirlineAreasPort GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}