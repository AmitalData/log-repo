using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class TasksSchedulerRepository : IRepository<TasksScheduler>
    {
        public IWebFreightContext webFreightContext;

        public TasksSchedulerRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public TasksSchedulerRepository()
        {
            webFreightContext = new WebFreightContext();
        }
        public TasksSchedulerRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(TasksScheduler entity)
        {
            webFreightContext.TasksSchedulers.Add(entity);
        }

        public void Remove(TasksScheduler entity)
        {
            webFreightContext.TasksSchedulers.Attach(entity);
            webFreightContext.TasksSchedulers.Remove(entity);
        }

        public void Update(TasksScheduler entity)
        {
            webFreightContext.TasksSchedulers.Attach(entity);
            webFreightContext.SetAsModified(entity);
        }

        public List<TasksScheduler> All()
        {
            return webFreightContext.TasksSchedulers.ToList();
        }
        public TasksScheduler GetSingleTasksScheduler(string Id, int Tenant)
        {
            return webFreightContext.TasksSchedulers.Where(a => a.Id == Id && a.Tenant == Tenant).FirstOrDefault();
        }

       
        public void SubmitChanges()
        {
            webFreightContext.SaveChanges();
        }
        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public List<TasksScheduler> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TasksScheduler GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<TasksScheduler> GetTasksScheduler(int tenant)
        {
            return webFreightContext.TasksSchedulers.Where(a => a.Tenant == tenant);
        }
        
    }
}
