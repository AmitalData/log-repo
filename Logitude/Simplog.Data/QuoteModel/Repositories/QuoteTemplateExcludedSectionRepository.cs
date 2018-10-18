using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.QuoteModel.Repositories
{
    public class QuoteTemplateExcludedSectionRepository : IRepository<QuoteTemplateExcludedSection>
    {





        public IQuotesContext quotesContext;

        public QuoteTemplateExcludedSectionRepository(IQuotesContext context)
        {
            quotesContext = context;
        }
        public QuoteTemplateExcludedSectionRepository()
        {
            quotesContext = new QuotesContext();
        }
        public QuoteTemplateExcludedSectionRepository(int tenant)
        {
            quotesContext = QuotesContext.GetContext(tenant);
        }




        public QuoteTemplateExcludedSection GetSingleQuoteTemplateExcludedSection(string id, int tenant)
        {

            QuoteTemplateExcludedSection entity = this.quotesContext.QuoteTemplateExcludedSections.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();


            return entity;
        }
        public IQueryable<QuoteTemplateExcludedSection> GetQuoteTemplateExcludedSections(int tenant)
        {
            return (from quoteTemplateDetails in quotesContext.QuoteTemplateExcludedSections where quoteTemplateDetails.Tenant == tenant select quoteTemplateDetails);

        }


        public void Add(QuoteTemplateExcludedSection entity)
        {
            quotesContext.QuoteTemplateExcludedSections.Add(entity);
        }

        public void Remove(QuoteTemplateExcludedSection entity)
        {
            quotesContext.QuoteTemplateExcludedSections.Attach(entity);
            quotesContext.QuoteTemplateExcludedSections.Remove(entity);
        }


        public void Update(QuoteTemplateExcludedSection entity)
        {
            quotesContext.QuoteTemplateExcludedSections.Attach(entity);
            quotesContext.SetAsModified(entity);
        }

        public List<QuoteTemplateExcludedSection> All()
        {
            return quotesContext.QuoteTemplateExcludedSections.ToList();
        }

        public void SubmitChanges()
        {
            quotesContext.SaveChanges();
        }

        public List<QuoteTemplateExcludedSection> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public QuoteTemplateExcludedSection GetSingle(EntityKeyFields entityKeys)
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






        public  IQueryable<QuoteTemplateExcludedSection>  GetAllQuoteTemplateExcludedSection(string quoteId, string templateId, int tenant)
        {
            return (from record in quotesContext.QuoteTemplateExcludedSections
                    where record.Tenant == tenant
                    && record.QuoteId == quoteId
                    && record.QuoteTemplateId == templateId

                    select record);
        }

        public QuoteTemplateExcludedSection GetSingelExcludedSection(string quoteId, string quotetemplateId, string quotetemplatesectionId, int tenant)
        {
           
            QuoteTemplateExcludedSection entity = this.quotesContext.QuoteTemplateExcludedSections.Where(d => d.QuoteId == quoteId &&  d.QuoteTemplateId == quotetemplateId  &&  d.QuoteTemplateSectionId == quotetemplatesectionId  &&   d.Tenant == tenant).FirstOrDefault();


            return entity;
        }
    }
}
