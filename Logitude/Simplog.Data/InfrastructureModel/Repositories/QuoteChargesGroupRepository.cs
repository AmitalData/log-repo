using System;
using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class QuoteChargesGroupRepository : IRepository<QuoteChargesGroup>
    {


        IWebFreightContext webFreightContext;
        public QuoteChargesGroupRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public QuoteChargesGroupRepository(IWebFreightContext context)
        {
            webFreightContext = context;

        }
        public QuoteChargesGroupRepository(int tenant)
        {

            webFreightContext = WebFreightContext.GetContext(tenant);

        }

        public QuoteChargesGroup GetSingleQuoteChargesGroupByCode(string code, int tenant)
        {
            return (from a in context.QuoteChargesGroups
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public QuoteChargesGroup GetSingleQuoteChargesGroup(string id, int tenant)
        {
            return (from a in context.QuoteChargesGroups
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public IQueryable<QuoteChargesGroup> GetQuoteChargesGroups(int tenant)
        {
            IQueryable<QuoteChargesGroup> quoteChargesGroups = (from a in context.QuoteChargesGroups
                                                               where a.Tenant == tenant
                                                               select a);

            return quoteChargesGroups;
        }


        public void Add(QuoteChargesGroup entity)
        {
            context.QuoteChargesGroups.Add(entity);
        }

        public void Remove(QuoteChargesGroup entity)
        {
            context.QuoteChargesGroups.Attach(entity);
            context.QuoteChargesGroups.Remove(entity);
        }

        public void Update(QuoteChargesGroup entity)
        {
            context.QuoteChargesGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteChargesGroup> All()
        {
            return context.QuoteChargesGroups.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<QuoteChargesGroup> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteChargesGroup GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}