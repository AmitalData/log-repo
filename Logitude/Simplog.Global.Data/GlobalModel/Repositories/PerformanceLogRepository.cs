using System.Collections.Generic;
using System.Linq;

using Simplog.Global.Data.GlobalModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class PerformanceLogRepository:IRepository<PerformanceLog>
    {
        IGlobalContext globalContext;
        public PerformanceLogRepository()
        {
            globalContext = GlobalContext.GetContext();
        }
        public PerformanceLogRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public IQueryable<PerformanceLog> GetPerformanceLogs()
        {
            return context.PerformanceLogs;
        }

        public IQueryable<PerformanceLog> GetPerformanceLogsByEmail(string email)
        {
            return from a in context.PerformanceLogs
                   where a.Email == email
                   select a;
        }

        public PerformanceLog GetSinglePerformanceLog(string id)
        {
            return (from a in context.PerformanceLogs
                   where a.Id == id
                   select a).FirstOrDefault();
        }

        public void Add(PerformanceLog entity)
        {
            
            context.PerformanceLogs.Add(entity);
        }

        public void Remove(PerformanceLog entity)
        {
            context.PerformanceLogs.Attach(entity);
            context.PerformanceLogs.Remove(entity);
        }

        public void Update(PerformanceLog entity)
        {
            context.PerformanceLogs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PerformanceLog> All()
        {
            return context.PerformanceLogs.ToList();
        }

        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PerformanceLog> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public PerformanceLog GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}