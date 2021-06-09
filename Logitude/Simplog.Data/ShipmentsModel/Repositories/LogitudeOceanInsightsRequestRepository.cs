using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class LogitudeOceanInsightsRequestRepository : IRepository<LogitudeOceanInsightsRequest>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public LogitudeOceanInsightsRequestRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public LogitudeOceanInsightsRequestRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public LogitudeOceanInsightsRequest GetSingleLogitudeOceanInsightsRequest(string id, int tenant)
        {
            return (from a in Context.LogitudeOceanInsightsRequests where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<LogitudeOceanInsightsRequest> GetLogitudeOceanInsightsRequests()
        {
            return (from a in Context.LogitudeOceanInsightsRequests select a);
        }

        public void Add(LogitudeOceanInsightsRequest entity)
        {
            Context.LogitudeOceanInsightsRequests.Add(entity);
        }

        public void Remove(LogitudeOceanInsightsRequest entity)
        {
            Context.LogitudeOceanInsightsRequests.Attach(entity);
            Context.LogitudeOceanInsightsRequests.Remove(entity);
        }

        public void Update(LogitudeOceanInsightsRequest entity)
        {
            Context.LogitudeOceanInsightsRequests.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<LogitudeOceanInsightsRequest> All()
        {
            return Context.LogitudeOceanInsightsRequests.ToList();
        }

        public List<LogitudeOceanInsightsRequest> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public LogitudeOceanInsightsRequest GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public LogitudeOceanInsightsRequest GetSingleLogitudeOceanInsightsRequestById(string id)
        {
            return (from a in Context.LogitudeOceanInsightsRequests where a.Id == id select a).FirstOrDefault();
        }

        public LogitudeOceanInsightsRequest GetSingleLogitudeOceanInsightsRequestByContainerNumberAndScac(string container_number, string carrier_scac)
        {
            var oceanInsights = from a in Context.LogitudeOceanInsightsRequests where a.ContainerNumber == container_number && a.SCACCode == carrier_scac select a;
            if(oceanInsights != null && oceanInsights.Count() == 1)
            {
                return oceanInsights.FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
