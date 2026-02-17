using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ShippingAgentRepository:IRepository<ShippingAgent>
    {
        ICommonDataContext commonDataContext;

        public ShippingAgentRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public ShippingAgentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public ShippingAgentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<ShippingAgent> GetShippingAgents(int tenant)
        {
            return (from record in context.ShippingAgents.Include("Card").Include("Card.PaymentTerm") where record.Tenant == tenant select record);
        }
        
        public ShippingAgent GetSingleShippingAgent(int tenant, string id)
        {
            return (from record in context.ShippingAgents.Include("Card").Include("Card.PaymentTerm") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public ShippingAgent GetSingleShippingAgent(string id, int tenant)
        {
            return (from record in context.ShippingAgents.Include("Card").Include("Card.PaymentTerm") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public ShippingAgent GetSingleShippingAgentByCode(string code, int tenant)
        {
            return (from record in context.ShippingAgents.Include("Card").Include("Card.PaymentTerm") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(ShippingAgent entity)
        {
            context.ShippingAgents.Add(entity);
        }

        public void Remove(ShippingAgent entity)
        {
            context.ShippingAgents.Attach(entity);
            context.ShippingAgents.Remove(entity);
        }

        public void Update(ShippingAgent entity)
        {
            context.ShippingAgents.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ShippingAgent> All()
        {
            return context.ShippingAgents.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<ShippingAgent> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ShippingAgent GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
