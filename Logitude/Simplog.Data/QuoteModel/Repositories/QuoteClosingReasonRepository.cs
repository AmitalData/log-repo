using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteClosingReasonRepository: IRepository<QuoteClosingReason>
    {
        IQuotesContext quotesContext;

        public QuoteClosingReasonRepository()
        {
            quotesContext = new QuotesContext();
        }

        public QuoteClosingReasonRepository(IQuotesContext context)
        {
            quotesContext = context;
        }

        public QuoteClosingReasonRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }

        public QuoteClosingReason GetSingleQuoteClosingReason(string id, int tenant)
        {
            return (from a in context.QuoteClosingReasons.Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public QuoteClosingReason GetSingleQuoteClosingReasonByCode(string code, int tenant)
        {
            return (from a in context.QuoteClosingReasons
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteClosingReason> GetAll()
        {
            return context.QuoteClosingReasons;
        }
        public IQueryable<QuoteClosingReason> GetQuoteClosingReasons(int tenant)
        {
            return (from a in context.QuoteClosingReasons.Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact")
                    where  a.Tenant == tenant
                    select a);
        }

        public void Add(QuoteClosingReason entity)
        {
            context.QuoteClosingReasons.Add(entity);
        }

        public void Remove(QuoteClosingReason entity)
        {
            context.QuoteClosingReasons.Attach(entity);
            context.QuoteClosingReasons.Remove(entity);
        }

        public void Update(QuoteClosingReason entity)
        {
            context.QuoteClosingReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteClosingReason> All()
        {
            return context.QuoteClosingReasons.ToList();
        }

        public IQuotesContext context
        {
            get { return quotesContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuoteClosingReason> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteClosingReason GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}