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
    public class AirlineAreaRepository:IRepository<AirlineArea>
    {
        ICommonDataContext commonDataContext;

        public AirlineAreaRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public AirlineAreaRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public AirlineAreaRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }
      
 

        public AirlineArea GetSingleAirlineArea(string id, int tenant)
        {
            return (from record in context.AirlineAreas where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }


        public void Add(AirlineArea entity)
        {
            context.AirlineAreas.Add(entity);
        }

        public void Remove(AirlineArea entity)
        {
            context.AirlineAreas.Attach(entity);
            context.AirlineAreas.Remove(entity);
        }

        public void Update(AirlineArea entity)
        {
            context.AirlineAreas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AirlineArea> All()
        {
            return context.AirlineAreas.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AirlineArea> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AirlineArea GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}