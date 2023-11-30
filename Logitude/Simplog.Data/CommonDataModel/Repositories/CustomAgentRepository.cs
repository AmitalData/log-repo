using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomAgentRepository:IRepository<CustomAgent>
    {
        ICommonDataContext commonDataContext;

        public CustomAgentRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public CustomAgentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomAgentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomAgent> GetCustomAgents(int tenant)
        {
            return (from record in context.CustomAgents.Include("Card") where record.Tenant == tenant select record);
        }

        public CustomAgent GetSingleCustomAgent(int tenant, string id)
        {
            return (from record in context.CustomAgents.Include("Card") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public CustomAgent GetSingleCustomAgent(string id, int tenant)
        {
            return (from record in context.CustomAgents.Include("Card").Include("Card.PaymentTerm") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        public CustomAgent GetSingleCustomAgentByCode(int tenant, string code)
        {
            return (from record in context.CustomAgents.Include("Card") where record.Card.Code == code && record.Tenant == tenant select record).FirstOrDefault();
        }

        public void Add(CustomAgent entity)
        {
            context.CustomAgents.Add(entity);

        }

        public void Remove(CustomAgent entity)
        {
            try
            {
                context.CustomAgents.Attach(entity);
            }
            catch { }
            context.CustomAgents.Remove(entity);
        }

        public void Update(CustomAgent entity)
        {
            try
            {
                context.CustomAgents.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<CustomAgent> All()
        {
            return context.CustomAgents.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<CustomAgent> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomAgent GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
        public CustomAgent GetFirstSingleByName(string name, int tenant)
        {
            return (from record in context.CustomAgents.Include("Card")
                    where record.Card.EnglishName == name && record.Tenant == tenant
                    select record).FirstOrDefault();
        }
    }
}