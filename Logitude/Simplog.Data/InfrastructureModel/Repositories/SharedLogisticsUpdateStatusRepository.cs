using System.Collections.Generic;
using System.Linq;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SharedLogisticsUpdateStatusRepository:IRepository<SharedLogisticsUpdateStatus>
    {

        IWebFreightContext webFreightContext;
        public SharedLogisticsUpdateStatusRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public SharedLogisticsUpdateStatusRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }
       
        public IQueryable<SharedLogisticsUpdateStatus> GetSharedLogisticsUpdateStatus()
        {
            return context.SharedLogisticsUpdateStatus;
        }

        public SharedLogisticsUpdateStatus GetSingleSharedLogisticsUpdateStatus(string code)
        {
            return (from a in context.SharedLogisticsUpdateStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(SharedLogisticsUpdateStatus entity)
        {
            context.SharedLogisticsUpdateStatus.Add(entity);
        }

        public void Remove(SharedLogisticsUpdateStatus entity)
        {
            context.SharedLogisticsUpdateStatus.Attach(entity);
            context.SharedLogisticsUpdateStatus.Remove(entity);
        }

        public void Update(SharedLogisticsUpdateStatus entity)
        {
            context.SharedLogisticsUpdateStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedLogisticsUpdateStatus> All()
        {
            return context.SharedLogisticsUpdateStatus.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<SharedLogisticsUpdateStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public SharedLogisticsUpdateStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}