using System.Collections.Generic;
using System.Linq;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteChargeRepository : IRepository<QuoteCharge>
    {       
        IQuotesContext quotesContext;

        public QuoteChargeRepository()
        {
            quotesContext = new QuotesContext();
        }

        public QuoteChargeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuoteChargeRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

        public IQueryable<QuoteCharge> GetQuoteReceivables(int tenant)
        {
            return (from record in context.QuoteCharges where record.Tenant == tenant select record);
        }
        
        public QuoteCharge GetSingleQuoteReceivable(string id, int tenant)
        {
            return (from record in context.QuoteCharges
                    where record.Id == id //&& Record.Tenant == tenant 
                    select record).FirstOrDefault();
        }

        
        public List<QuoteCharge> GetQuoteReceivablesByQuoteId(string id, int tenant)
        {
            List<QuoteCharge> quoteReceivables = (from a in context.QuoteCharges
                                                            where a.Tenant == tenant && a.QuoteId == id
                                                            select a).ToList();
            return quoteReceivables;
        }
        
     

        public void Add(QuoteCharge entity)
        {
            context.QuoteCharges.Add(entity);
        }

        public void Remove(QuoteCharge entity)
        {
            try
            {

                context.QuoteCharges.Attach(entity);
            }
            catch { }
            context.QuoteCharges.Remove(entity);
        }

        public void Update(QuoteCharge entity)
        {
            try
            {
                context.QuoteCharges.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<QuoteCharge> All()
        {
            return context.QuoteCharges.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<QuoteCharge> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteCharge GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
