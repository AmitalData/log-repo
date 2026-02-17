using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class APILogsRepository : IRepository<APILogs>
    {
        public IWebFreightContext webFreightContext;

        public APILogsRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public APILogsRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public APILogsRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(APILogs entity)
        {
            webFreightContext.APILogs.Add(entity);
        }

        public void Remove(APILogs entity)
        {
            webFreightContext.APILogs.Attach(entity);
            webFreightContext.APILogs.Remove(entity);
        }

        public void Update(APILogs entity)
        {
            webFreightContext.APILogs.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<APILogs> All()
        {
            return webFreightContext.APILogs.ToList();
        }
        public APILogs GetSingleAPILogs(string Id, int Tenant)
        {
            return webFreightContext.APILogs.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }
        public APILogs GetSingleAPILogsByCorrelationId(string CorrelationId, int Tenant)
        {
            return webFreightContext.APILogs.Where(a => a.CorrelationId == CorrelationId && a.Tenant == Tenant).FirstOrDefault();
        }
        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }

        public List<APILogs> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public APILogs GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<APILogs> GetAPILogs(int tenant)
        {
            return webFreightContext.APILogs.Where(a => a.Tenant == tenant);
        }
        public IQueryable<APILogs> GetAPILogsByCustomerId(string CustomerId,int tenant)
        {
            return webFreightContext.APILogs.Where(a => a.Tenant == tenant && a.CustomerId == CustomerId);
        }
    }
}
