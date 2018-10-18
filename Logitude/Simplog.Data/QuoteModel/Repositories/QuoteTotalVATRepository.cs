using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteTotalVATRepository : IRepository<QuoteTotalVAT>
    {
        IQuotesContext myContext;

        public QuoteTotalVATRepository()
        {
            myContext = new QuotesContext();
        }
        public QuoteTotalVATRepository(IQuotesContext context)
        {
            myContext = context;
        }
        public QuoteTotalVATRepository(int tenant)
        {
            myContext = QuotesContext.GetContext(tenant);
        }

        public QuoteTotalVAT GetSingleTotalVAT(string id)
        {
            return (from a in context.QuoteTotalVATs where a.Id == id select a).FirstOrDefault();
        }

        public IQueryable<QuoteTotalVAT> GetTotalVATs(int tenant)
        {
            return (from a in context.QuoteTotalVATs where a.Tenant == tenant select a);
        }

        public IQueryable<QuoteTotalVAT> GetTotalVATs(string quoteId, int tenant)
        {
            return (from a in context.QuoteTotalVATs where a.Tenant == tenant && a.QuoteId == quoteId select a);
        }

        public List<QuoteTotalVAT> GetTotalVATs(List<string> ids, int tenant)
        {
            List<QuoteTotalVAT> myResult = new List<QuoteTotalVAT>();

            if (ids.Count > 0)
            {
                myResult = (from a in context.QuoteTotalVATs where a.Tenant == tenant && ids.Contains(a.QuoteId) select a).ToList();
            }

            return myResult;
        }

        public List<QuoteTotalVAT> GetTotalVatsForWithoutZeroVATPercent(string quoteId, int tenant)
        {
            return (from a in context.QuoteTotalVATs where a.Tenant == tenant && a.QuoteId == quoteId && a.VatPercent != 0 select a).ToList();
        }

        public void Add(QuoteTotalVAT entity)
        {
            context.QuoteTotalVATs.Add(entity);
        }

        public void Remove(QuoteTotalVAT entity)
        {
            context.QuoteTotalVATs.Attach(entity);
            context.QuoteTotalVATs.Remove(entity);
        }

        public void Update(QuoteTotalVAT entity)
        {
            context.QuoteTotalVATs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteTotalVAT> All()
        {
            return context.QuoteTotalVATs.ToList();
        }

        public IQuotesContext context
        {
            get { return myContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<QuoteTotalVAT> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public QuoteTotalVAT GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
