using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Global.Data.GlobalModel.Repositories
{
    public class MonitorServiceLastUpdateRepository : IRepository<MonitorServiceLastUpdate>
    {
        IGlobalContext globalContext;
        public IGlobalContext context
        {
            get { return globalContext; }
        }

        public MonitorServiceLastUpdateRepository()
        {
            globalContext = GlobalContext.GetContext();
        }

        public MonitorServiceLastUpdateRepository(IGlobalContext context)
        {
            globalContext = context;
        }

        public MonitorServiceLastUpdate GetSingleMonitorServiceLastUpdate(string code)
        {
            return (from a in context.MonitorServiceLastUpdates where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<MonitorServiceLastUpdate> GetAllMonitorServiceLastUpdates()
        {
            return from a in context.MonitorServiceLastUpdates select a;
        }

        public List<MonitorServiceLastUpdate> All()
        {
            return context.MonitorServiceLastUpdates.ToList();
        }

        public List<MonitorServiceLastUpdate> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MonitorServiceLastUpdate GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Add(MonitorServiceLastUpdate entity)
        {
            context.MonitorServiceLastUpdates.Add(entity);
        }

        public void Remove(MonitorServiceLastUpdate entity)
        {
            context.MonitorServiceLastUpdates.Attach(entity);
            context.MonitorServiceLastUpdates.Remove(entity);
        }

        public void Update(MonitorServiceLastUpdate entity)
        {
            context.MonitorServiceLastUpdates.Attach(entity);
            context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
    }
}
