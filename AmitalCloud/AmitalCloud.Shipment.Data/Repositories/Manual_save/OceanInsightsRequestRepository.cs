using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OceanInsightsRequestRepository : IRepository<OceanInsightsRequest>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public OceanInsightsRequestRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public OceanInsightsRequestRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public OceanInsightsRequest GetSingleOceanInsightsRequest(string id, int tenant)
        {
            return (from a in Context.OceanInsightsRequests where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<OceanInsightsRequest> GetOceanInsightsRequests()
        {
            return (from a in Context.OceanInsightsRequests select a);
        }

        public void Add(OceanInsightsRequest entity)
        {
            Context.OceanInsightsRequests.Add(entity);
        }

        public void Remove(OceanInsightsRequest entity)
        {
            Context.OceanInsightsRequests.Attach(entity);
            Context.OceanInsightsRequests.Remove(entity);
        }

        public void Update(OceanInsightsRequest entity)
        {
            Context.OceanInsightsRequests.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<OceanInsightsRequest> All()
        {
            return Context.OceanInsightsRequests.ToList();
        }

        public List<OceanInsightsRequest> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OceanInsightsRequest GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
