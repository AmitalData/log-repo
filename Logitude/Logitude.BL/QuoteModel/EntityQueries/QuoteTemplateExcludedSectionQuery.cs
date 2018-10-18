using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteTemplateExcludedSectionQuery
    {

        

        QuoteTemplateExcludedSectionRepository repository;

        public QuoteTemplateExcludedSectionQuery()
        {
            repository = new QuoteTemplateExcludedSectionRepository();
        }

        public QuoteTemplateExcludedSectionQuery(int tenant)
        {
            repository = new QuoteTemplateExcludedSectionRepository(tenant);
        }

        public QuoteTemplateExcludedSectionQuery(QuoteTemplateExcludedSectionRepository quoteTemplateExcludedSectionRepository)
        {
            repository = quoteTemplateExcludedSectionRepository;
        }

        public QuoteTemplateExcludedSectionPM GetSinglePM(string id, int tenant)
        {
            QuoteTemplateExcludedSectionPM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateExcludedSections
                      where a.Id == id && a.Tenant == tenant
                      select new QuoteTemplateExcludedSectionPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          QuoteTemplateSectionId = a.QuoteTemplateSectionId,
                          QuoteTemplateId = a.QuoteTemplateId,
                          QuoteId = a.QuoteId,
               
                      }).FirstOrDefault();

            return entity;

        }

        public IQueryable<QuoteTemplateExcludedSectionPM> GetQuoteTemplateExcludedSectionPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateExcludedSectionPM> qUoteTemplateExcludedSection = from a in repository.quotesContext.QuoteTemplateExcludedSections

                                                                                      where a.Tenant == tenant
                                                                                      select new QuoteTemplateExcludedSectionPM()
                                                                                      {
                                                                                          Id = a.Id,
                                                                                          Tenant = a.Tenant,
                                                                                          QuoteTemplateSectionId = a.QuoteTemplateSectionId,
                                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                                          QuoteId = a.QuoteId,

                                                                                      };
            return qUoteTemplateExcludedSection;
        }

        public IQueryable<QuoteTemplateExcludedSectionList> GetIQueryableEntityList(IQueryable<QuoteTemplateExcludedSection> iQueryable)
        {
            IQueryable<QuoteTemplateExcludedSectionList> result = from quoteTemplateExcludedSection in iQueryable
                                                                  select new QuoteTemplateExcludedSectionList()
                                                                  {

                                                                      Id = quoteTemplateExcludedSection.Id,
                                                                      Tenant = quoteTemplateExcludedSection.Tenant,
                                                                      QuoteTemplateSectionId = quoteTemplateExcludedSection.QuoteTemplateSectionId,
                                                                      QuoteTemplateId = quoteTemplateExcludedSection.QuoteTemplateId,
                                                                      QuoteId = quoteTemplateExcludedSection.QuoteId,

                                                                  };
            return result;
        }



        public QuoteTemplateExcludedSection GetFirstQuoteTemplateExcludedSectionForTenant()
        {
            return (from a in repository.quotesContext.QuoteTemplateExcludedSections

                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteTemplateExcludedSectionPM> GetQuoteTemplateExcludedSectionPMsByQuotetemplateId(int tenant, string quotetemplateId)
        {
            IQueryable<QuoteTemplateExcludedSectionPM> qUoteTemplateExcludedSection = from a in repository.quotesContext.QuoteTemplateExcludedSections

                                                                                      where a.Tenant == tenant && a.QuoteTemplateId == quotetemplateId
                                                                                      select new QuoteTemplateExcludedSectionPM()
                                                                                      {
                                                                                          Id = a.Id,
                                                                                          Tenant = a.Tenant,
                                                                                          QuoteTemplateSectionId = a.QuoteTemplateSectionId,
                                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                                          QuoteId = a.QuoteId,
                                                                                      };
            return qUoteTemplateExcludedSection;
        }
    }
}







