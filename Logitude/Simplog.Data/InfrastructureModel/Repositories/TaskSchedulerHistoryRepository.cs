using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TaskSchedulerHistoryRepository : IRepository<TaskSchedulerHistory>
    {
        public IWebFreightContext webFreightContext;

        public TaskSchedulerHistoryRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TaskSchedulerHistoryRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public TaskSchedulerHistoryRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(TaskSchedulerHistory entity)
        {
            webFreightContext.TaskSchedulerHistories.Add(entity);
        }

        public void Remove(TaskSchedulerHistory entity)
        {
            webFreightContext.TaskSchedulerHistories.Attach(entity);
            webFreightContext.TaskSchedulerHistories.Remove(entity);
        }

        public void Update(TaskSchedulerHistory entity)
        {
            webFreightContext.TaskSchedulerHistories.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<TaskSchedulerHistory> All()
        {
            return webFreightContext.TaskSchedulerHistories.ToList();
        }
        public TaskSchedulerHistory GetSingleTaskSchedulerHistory(string Id, int Tenant)
        {
            return webFreightContext.TaskSchedulerHistories.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }
       
        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }
        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }
        public List<TaskSchedulerHistory> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TaskSchedulerHistory GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TaskSchedulerHistory> GetTaskSchedulerHistory(int tenant)
        {
            return webFreightContext.TaskSchedulerHistories.Where(a => a.Tenant == tenant);
        }

        public IQueryable<TaskSchedulerHistory> GetTaskSchedulerHistory(int tenant,string TaskId)
        {
            return webFreightContext.TaskSchedulerHistories.Where(a => a.Tenant == tenant && a.TaskId == TaskId);
        }
        
    }
}
