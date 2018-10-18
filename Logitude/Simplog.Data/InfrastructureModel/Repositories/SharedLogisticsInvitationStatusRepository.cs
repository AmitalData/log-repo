using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class SharedLogisticsInvitationStatusRepository:IRepository<SharedLogisticsInvitationStatus>
    {
        IWebFreightContext webFreightContext;

        public SharedLogisticsInvitationStatusRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public SharedLogisticsInvitationStatusRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public SharedLogisticsInvitationStatusRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public IQueryable<SharedLogisticsInvitationStatus> GetSharedLogisticsInvitationStatus()
        {
            return context.SharedLogisticsInvitationStatus;
        }

        public SharedLogisticsInvitationStatus GetSingleSharedLogisticsInvitationStatus(int code)
        {
            return (from a in context.SharedLogisticsInvitationStatus
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public void Add(SharedLogisticsInvitationStatus entity)
        {
            context.SharedLogisticsInvitationStatus.Add(entity);
        }

        public void Remove(SharedLogisticsInvitationStatus entity)
        {
            context.SharedLogisticsInvitationStatus.Attach(entity);
            context.SharedLogisticsInvitationStatus.Remove(entity);
        }

        public void Update(SharedLogisticsInvitationStatus entity)
        {
            context.SharedLogisticsInvitationStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SharedLogisticsInvitationStatus> All()
        {
            return context.SharedLogisticsInvitationStatus.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<SharedLogisticsInvitationStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SharedLogisticsInvitationStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}