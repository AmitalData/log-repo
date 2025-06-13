using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OceanInsightsStatusesRepository : IRepository<OceanInsightsStatuses>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public OceanInsightsStatusesRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public OceanInsightsStatusesRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public OceanInsightsStatuses GetSingleOceanInsightsStatus(string id, int tenant)
        {
            return (from a in Context.OceanInsightsStatuses where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<OceanInsightsStatuses> GetOceanInsightsStatuses()
        {
            return (from a in Context.OceanInsightsStatuses select a);
        }

        public IQueryable<OceanInsightsStatuses> GetOceanInsightsStatusesbyOceanInsightsRequestId(string RequestId, int tenant)
        {
            return (from a in Context.OceanInsightsStatuses where a.OceanInsightsRequestId == RequestId && a.Tenant == tenant select a);
        }

        public void Add(OceanInsightsStatuses entity)
        {
            Context.OceanInsightsStatuses.Add(entity);
        }

        public void Remove(OceanInsightsStatuses entity)
        {
            Context.OceanInsightsStatuses.Attach(entity);
            Context.OceanInsightsStatuses.Remove(entity);
        }

        public void Update(OceanInsightsStatuses entity)
        {
            Context.OceanInsightsStatuses.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<OceanInsightsStatuses> All()
        {
            return Context.OceanInsightsStatuses.ToList();
        }

        public List<OceanInsightsStatuses> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OceanInsightsStatuses GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
