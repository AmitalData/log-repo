using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteDocumentVersionRepository : IRepository<QuoteDocumentVersion>
    {
        public IQuotesContext quotesContext;
        public QuoteDocumentVersionRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteDocumentVersionRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteDocumentVersionRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }



        public QuoteDocumentVersion GetSingleQuoteDocumentVersion(string quoteId, int tenant, int versionNumber)
        {

            QuoteDocumentVersion entity = this.quotesContext.QuoteDocumentVersions.Where(d => d.QuoteId == quoteId && d.VersionNumber == versionNumber && d.Tenant == tenant).FirstOrDefault();

            return entity;
        }


        public QuoteDocumentVersion GetSingleLastQuoteDocumentVersion(string quoteId, int tenant)
        {

            QuoteDocumentVersion entity = this.quotesContext.QuoteDocumentVersions.Where(d => d.QuoteId == quoteId && d.Tenant == tenant).OrderByDescending(d=>d.VersionNumber).FirstOrDefault();

            return entity;
        }

        public IQueryable<QuoteDocumentVersion> GetQuoteDocumentVersions(int tenant)
        {
            return (from record in quotesContext.QuoteDocumentVersions where record.Tenant == tenant select record);
        }
        public IQueryable<QuoteDocumentVersion> GetQuoteDocumentVersionsByQuoteId(string quoteId, int tenant)
        {
            return (from record in quotesContext.QuoteDocumentVersions where record.Tenant == tenant && record.QuoteId == quoteId select record);
        }

        public string GetLastQuoteDocumentVersionDocumentId(string quoteId, int tenant)
        {
            string result = "";
            QuoteDocumentVersion quoteDocumentVersion = this.quotesContext.QuoteDocumentVersions.Where(d => d.QuoteId == quoteId && d.Tenant == tenant).OrderByDescending(d => d.VersionNumber).FirstOrDefault();
            if (quoteDocumentVersion != null)
            {
                result = quoteDocumentVersion.DocumentId;
            }
            return result;
        }



        public void Add(QuoteDocumentVersion entity)
        {
            quotesContext.QuoteDocumentVersions.Add(entity);
        }

        public void Remove(QuoteDocumentVersion entity)
        {
            quotesContext.QuoteDocumentVersions.Attach(entity);
            quotesContext.QuoteDocumentVersions.Remove(entity);
        }


        public void Update(QuoteDocumentVersion entity)
        {
            quotesContext.QuoteDocumentVersions.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteDocumentVersion> All()
        {
            return quotesContext.QuoteDocumentVersions.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public List<QuoteDocumentVersion> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteDocumentVersion GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

   
    }
}
