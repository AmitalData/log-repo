using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OceanInsightsRequestsCountRepository : IRepository<OceanInsightsRequestsCount>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public OceanInsightsRequestsCountRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public OceanInsightsRequestsCountRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public OceanInsightsRequestsCount GetSingleOceanInsightsRequestsCount(string id, int tenant)
        {
            return (from a in Context.OceanInsightsRequestsCounts where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<OceanInsightsRequestsCount> GetOceanInsightsRequestsCounts()
        {
            return (from a in Context.OceanInsightsRequestsCounts select a);
        }

        public void Add(OceanInsightsRequestsCount entity)
        {
            Context.OceanInsightsRequestsCounts.Add(entity);
        }

        public void Remove(OceanInsightsRequestsCount entity)
        {
            Context.OceanInsightsRequestsCounts.Attach(entity);
            Context.OceanInsightsRequestsCounts.Remove(entity);
        }

        public void Update(OceanInsightsRequestsCount entity)
        {
            Context.OceanInsightsRequestsCounts.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<OceanInsightsRequestsCount> All()
        {
            return Context.OceanInsightsRequestsCounts.ToList();
        }

        public List<OceanInsightsRequestsCount> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OceanInsightsRequestsCount GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
