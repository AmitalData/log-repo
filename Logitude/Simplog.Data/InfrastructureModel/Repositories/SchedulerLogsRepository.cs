using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SchedulerLogsRepository : IRepository<SchedulerLogs>
    {
        public IWebFreightContext webFreightContext;

        public SchedulerLogsRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public SchedulerLogsRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public SchedulerLogsRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(SchedulerLogs entity)
        {
            webFreightContext.SchedulerLogs.Add(entity);
        }

        public void Remove(SchedulerLogs entity)
        {
            webFreightContext.SchedulerLogs.Attach(entity);
            webFreightContext.SchedulerLogs.Remove(entity);
        }

        public void Update(SchedulerLogs entity)
        {
            webFreightContext.SchedulerLogs.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<SchedulerLogs> All()
        {
            return webFreightContext.SchedulerLogs.ToList();
        }
        public SchedulerLogs GetSingleSchedulerLogs(string Id, int Tenant)
        {
            return webFreightContext.SchedulerLogs.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }
       
        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }
        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }
        public List<SchedulerLogs> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SchedulerLogs GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<SchedulerLogs> GetSchedulerLogs(int tenant)
        {
            return webFreightContext.SchedulerLogs.Where(a => a.Tenant == tenant);
        }

        public IQueryable<SchedulerLogs> GetSchedulerLogs(int tenant,string HistoryId)
        {
            return webFreightContext.SchedulerLogs.Where(a => a.Tenant == tenant && a.HistoryId == HistoryId);
        }
        
    }
}
