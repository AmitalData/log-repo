using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AgentRepository:IRepository<Agent>
    {
        ICommonDataContext commonDataContext;

        public AgentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public AgentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public int GetAgentsCount(int tenant)
        {
            return (from record in context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.PaymentTerm").Include("Card.InvoiceCurrency") where record.Tenant == tenant select record).Count();
        }

        public IQueryable<Agent> GetAgents(int tenant)
        {
            return (from record in context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.PaymentTerm").Include("Card.InvoiceCurrency") where record.Tenant == tenant select record);
        }

        public Agent GetSingleAgent(int tenant, string id)
        {
            return (from record in context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.PaymentTerm").Include("Card.InvoiceCurrency") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Agent GetSingleAgentWithOutIncluded(int tenant, string id)
        {
            return (from record in context.Agents where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Agent GetSingleAgent(string id, int tenant)
        {
            return (from record in context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.PaymentTerm").Include("Card.InvoiceCurrency") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Agent GetSingleAgentByCode(string code, int tenant)
        {
            return (from record in context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.PaymentTerm").Include("Card.InvoiceCurrency") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Agent GetSingleAgentBySharedKey(string key, int tenant)
        {
            return (from record in context.Agents.Include("Card").Include("Card.SharedLogisticsInvitationStatus").Include("Card.PaymentTerm").Include("Card.InvoiceCurrency") where record.AgentSharedLogisticsKey == key && record.Tenant == tenant select record).FirstOrDefault();
        }

        public string GetAgentIdBySharedKey(string key, int tenant)
        {

            return (from record in context.Agents where record.AgentSharedLogisticsKey == key && record.Tenant == tenant select record.Id).FirstOrDefault();
        }



        public string GetSharedKeyByAgentId(string agentId, int tenant)
        {
            return (from record in context.Agents where record.Id == agentId && record.Tenant == tenant select record.AgentSharedLogisticsKey).FirstOrDefault();
        }



        public void Add(Agent entity)
        {
            context.Agents.Add(entity);
        }

        public void Remove(Agent entity)
        {
            try
            {
                context.Agents.Attach(entity);
            }
            catch { }
            context.Agents.Remove(entity);
        }

        public void Update(Agent entity)
        {
            try
            {
                context.Agents.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<Agent> All()
        {
            return context.Agents.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Agent> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Agent GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public Agent GetFirstSingleByName(string name, int tenant)
        {
            return (from record in context.Agents.Include("Card")
                    where record.Card.EnglishName == name && record.Tenant == tenant
                    select record).FirstOrDefault();
        }
    }
}
