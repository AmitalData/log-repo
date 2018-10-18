using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuotePriceStepsRepository: IRepository<QuotePriceSteps>
    {
        IQuotesContext quotesContext;
        public QuotePriceStepsRepository()
        {
            quotesContext = new QuotesContext();
        }

        public QuotePriceStepsRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

        public QuotePriceStepsRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuotePriceSteps GetSingleQuotePriceStep(string id)
        {
            return (from a in context.QuotePriceSteps
                    where a.Id == id
                    select a).FirstOrDefault();
        }

        public IQueryable<QuotePriceSteps> GetQuotePriceStepsByTenant(int tenant)
        {
            return from a in context.QuotePriceSteps
                   where a.Tenant == tenant
                   select a;
        }

        public List<QuotePriceSteps> GetQuotePriceStepPMsByQuoteCharge(string quoteId, string quoteChargeId, int tenant)
        {
            return (from a in context.QuotePriceSteps
                    where a.QuoteId == quoteId && a.QuoteChargeId == quoteChargeId && a.Tenant == tenant
                    select a).ToList();
        }
       
        public void Add(QuotePriceSteps entity)
        {
            context.QuotePriceSteps.Add(entity);
        }

        public void Remove(QuotePriceSteps entity)
        {
            context.QuotePriceSteps.Attach(entity);
            context.QuotePriceSteps.Remove(entity);
        }

        public void Update(QuotePriceSteps entity)
        {
            context.QuotePriceSteps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuotePriceSteps> All()
        {
            return context.QuotePriceSteps.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuotePriceSteps> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuotePriceSteps GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}