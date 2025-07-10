using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class OceanInsightsStatusLogRepository : IRepository<OceanInsightsStatusLog>
    {
        IShipmentsContext myContext;
        public IShipmentsContext Context
        {
            get { return myContext; }
        }

        public OceanInsightsStatusLogRepository(int tenant)
        {
            myContext = ShipmentsContext.GetContext(tenant);
        }

        public OceanInsightsStatusLogRepository(IShipmentsContext context)
        {
            myContext = context;
        }

        public OceanInsightsStatusLog GetSingleOceanInsightsStatusLog(string id, int tenant)
        {
            return (from a in Context.OceanInsightsStatusLogs where a.Id == id && a.Tenant == tenant select a).FirstOrDefault();
        }

        public IQueryable<OceanInsightsStatusLog> GetOceanInsightsRequests()
        {
            return (from a in Context.OceanInsightsStatusLogs select a);
        }

        public void Add(OceanInsightsStatusLog entity)
        {
            Context.OceanInsightsStatusLogs.Add(entity);
        }

        public void Remove(OceanInsightsStatusLog entity)
        {
            Context.OceanInsightsStatusLogs.Attach(entity);
            Context.OceanInsightsStatusLogs.Remove(entity);
        }

        public void Update(OceanInsightsStatusLog entity)
        {
            Context.OceanInsightsStatusLogs.Attach(entity);
            Context.SetAsModified(entity);
        }

        public List<OceanInsightsStatusLog> All()
        {
            return Context.OceanInsightsStatusLogs.ToList();
        }

        public List<OceanInsightsStatusLog> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public OceanInsightsStatusLog GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }

    }
}
