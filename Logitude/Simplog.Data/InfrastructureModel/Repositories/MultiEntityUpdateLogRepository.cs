using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class MultiEntityUpdateLogRepository : IRepository<MultiEntityUpdateLog>
    {
        IWebFreightContext webFreightContext;

        public MultiEntityUpdateLogRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public MultiEntityUpdateLogRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
        public MultiEntityUpdateLogRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void Add(MultiEntityUpdateLog entity)
        {
            context.MultiEntityUpdateLogs.Add(entity);
        }

        public List<MultiEntityUpdateLog> All()
        {
            return context.MultiEntityUpdateLogs.ToList();
        }

        public List<MultiEntityUpdateLog> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public MultiEntityUpdateLog GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(MultiEntityUpdateLog entity)
        {
            context.MultiEntityUpdateLogs.Attach(entity);
            context.MultiEntityUpdateLogs.Remove(entity);
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public void Update(MultiEntityUpdateLog entity)
        {
            try
            {
                context.MultiEntityUpdateLogs.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public MultiEntityUpdateLog GetSingleMultiEntityUpdateLog(string id)
        {
            return (from a in context.MultiEntityUpdateLogs
                    where a.Id == id
                    select a).FirstOrDefault();
        }

    }
}
