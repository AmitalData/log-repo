using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ParticipantRepository : IRepository<Participant>
    {
        ICommonDataContext commonDataContext;

        public ParticipantRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public ParticipantRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Participant> GetParticipants(int tenant)
        {
            return (from record in context.Participants.Include("Card") where record.Tenant == tenant select record);
        }

        public Participant GetSingleParticipant(string id, int tenant)
        {
            return (from record in context.Participants.Include("Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Participant GetSingleParticipantByCode(string code, int tenant)
        {
            return (from record in context.Participants.Include("Card") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(Participant entity)
        {
            context.Participants.Add(entity);
        }

        public void Remove(Participant entity)
        {
            try
            {
                context.Participants.Attach(entity);
            }
            catch { }
            context.Participants.Remove(entity);
        }

        public void Update(Participant entity)
        {
            try
            {
                context.Participants.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Participant> All()
        {
            return context.Participants.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Participant> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Participant GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Participant GetSingleParticipantByForwarderandAirlineTenant(int forwarderTenant, int currentTenant)
        {
            return (from record in context.Participants.Include("Card") where record.ForwarderTenant == forwarderTenant && record.Tenant == currentTenant select record).FirstOrDefault();
        }

        public IQueryable<Participant> GetParticipantsByForwarderTenant(int tenant)
        {
            return (from record in context.Participants.Include("Card") where record.ForwarderTenant == tenant select record);
        }
    }
}