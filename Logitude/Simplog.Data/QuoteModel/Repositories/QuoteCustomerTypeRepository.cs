using System.Collections.Generic;
using System.Linq;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteCustomerTypeRepository: IRepository<QuoteCustomerType>
    {
        IQuotesContext quotesContext;
        public QuoteCustomerTypeRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteCustomerTypeRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteCustomerTypeRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }
        public QuoteCustomerType GetSingleQuoteCustomerType(string code)
        {
            return (from a in context.QuoteCustomerTypes
                    where a.Code == code
                    select a).FirstOrDefault();
        }


        public IQueryable<QuoteCustomerType> GetAll()
        {
            return context.QuoteCustomerTypes;
        }

        public IQueryable<QuoteCustomerType> GetQuoteCustomerTypes()
        {
            return context.QuoteCustomerTypes;
        }

        public void Add(QuoteCustomerType entity)
        {
            context.QuoteCustomerTypes.Add(entity);
        }

        public void Remove(QuoteCustomerType entity)
        {
            context.QuoteCustomerTypes.Attach(entity);
            context.QuoteCustomerTypes.Remove(entity);
        }

        public void Update(QuoteCustomerType entity)
        {
            context.QuoteCustomerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteCustomerType> All()
        {
            return context.QuoteCustomerTypes.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }




        public List<QuoteCustomerType> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteCustomerType GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}