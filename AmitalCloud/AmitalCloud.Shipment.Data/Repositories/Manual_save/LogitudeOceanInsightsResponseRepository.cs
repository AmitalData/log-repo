using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class LogitudeOceanInsightsResponseRepository
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public LogitudeOceanInsightsResponseRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public LogitudeOceanInsightsResponseRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public LogitudeOceanInsightsResponse GetSingleLogitudeOceanInsightsResponse(string id, int tenant)
        {
            return (from a in Context.LogitudeOceanInsightsResponses where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }
       
        public IQueryable<LogitudeOceanInsightsResponse> GetLogitudeOceanInsightsResponses()
        {
            return (from a in Context.LogitudeOceanInsightsResponses select a);
        }

        public void Add(LogitudeOceanInsightsResponse entity)
        {
            Context.LogitudeOceanInsightsResponses.Add(entity);
        }

        public void Remove(LogitudeOceanInsightsResponse entity)
        {
            Context.LogitudeOceanInsightsResponses.Attach(entity);
            Context.LogitudeOceanInsightsResponses.Remove(entity);
        }

        public void Update(LogitudeOceanInsightsResponse entity)
        {
            Context.LogitudeOceanInsightsResponses.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<LogitudeOceanInsightsResponse> All()
        {
            return Context.LogitudeOceanInsightsResponses.ToList();
        }

        public List<LogitudeOceanInsightsResponse> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public LogitudeOceanInsightsResponse GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public LogitudeOceanInsightsResponse GetLogitudeOceanInsightsResponseByContainerNumberAndScac(string container_number, string carrier_scac, int tenant)
        {
            var oceanInsight = (from a in Context.LogitudeOceanInsightsResponses where a.ContainerNumber == container_number && a.SCACCode == carrier_scac && a.Tenant == tenant  select a).FirstOrDefault();
            return oceanInsight;
        }

        public List<LogitudeOceanInsightsResponse> GetLogitudeOceanInsightsResponseByContainerNumberAndScac(string container_number, string carrier_scac)
        {
            var oceanInsights = from a in Context.LogitudeOceanInsightsResponses where a.ContainerNumber == container_number && a.SCACCode == carrier_scac select a;
            if (oceanInsights != null && oceanInsights.Count() == 1)
            {
                return oceanInsights.ToList();
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
