using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteTemplateSectionModificationRepository : IRepository<QuoteTemplateSectionModification>
    {





        public IQuotesContext quotesContext;

        public QuoteTemplateSectionModificationRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTemplateSectionModificationRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteTemplateSectionModificationRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }




        public QuoteTemplateSectionModification GetSingleQuoteTemplateSectionModification(string id, int tenant)
        {

            QuoteTemplateSectionModification entity = this.quotesContext.QuoteTemplateSectionModifications.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;
        }
        public IQueryable<QuoteTemplateSectionModification> GetQuoteTemplateSectionModifications(int tenant)
        {
            return (from quoteTemplateDetails in quotesContext.QuoteTemplateSectionModifications where quoteTemplateDetails.Tenant == tenant select quoteTemplateDetails);

        }


        public void Add(QuoteTemplateSectionModification entity)
        {
            quotesContext.QuoteTemplateSectionModifications.Add(entity);
        }

        public void Remove(QuoteTemplateSectionModification entity)
        {
            quotesContext.QuoteTemplateSectionModifications.Attach(entity);
            quotesContext.QuoteTemplateSectionModifications.Remove(entity);
        }


        public void Update(QuoteTemplateSectionModification entity)
        {
            quotesContext.QuoteTemplateSectionModifications.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteTemplateSectionModification> All()
        {
            return quotesContext.QuoteTemplateSectionModifications.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public List<QuoteTemplateSectionModification> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteTemplateSectionModification GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }






        public IQueryable<QuoteTemplateSectionModification> GetAllQuoteTemplateSectionModifications(string quoteId, int tenant)
        {
            return (from record in quotesContext.QuoteTemplateSectionModifications
                    where record.Tenant == tenant
                    && record.QuoteId == quoteId

                    select record);
        }




        public QuoteTemplateSectionModification GetSingelQuoteTemplateSectionModification(string quoteId, string quotetemplatesectionId, int tenant)
        {

            QuoteTemplateSectionModification entity = this.quotesContext.QuoteTemplateSectionModifications.Where(d => d.QuoteId == quoteId && d.QuoteTemplateSectionId == quotetemplatesectionId && d.Tenant == tenant).FirstOrDefault();


            return entity;
        }
    }
}
