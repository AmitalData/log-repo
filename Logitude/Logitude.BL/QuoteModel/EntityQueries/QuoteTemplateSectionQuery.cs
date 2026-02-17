using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteTemplateSectionQuery
    {

        QuoteTemplateSectionRepository repository;
        
        QuoteTemplateExcludedSectionRepository excludedSectionRepository;
        public QuoteTemplateSectionQuery()
        {
            repository = new QuoteTemplateSectionRepository(); 
        }

        public QuoteTemplateSectionQuery(int tenant)
        {
            repository = new QuoteTemplateSectionRepository(tenant);
        }

        public QuoteTemplateSectionQuery(QuoteTemplateSectionRepository quoteTemplateSectionRepository)
        {
            repository = quoteTemplateSectionRepository;
        }
     
        public QuoteTemplateSectionPM GetSinglePM(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "QuoteTemplateSectionPM" + id + tenant;
                QuoteTemplateSectionPM entity;
                if (HttpContext.Current != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {

                        var quoteTemplatesSection = (from a in repository.quotesContext.QuoteTemplateSections
                                           where a.Tenant == tenant && a.IsCancel != true
                                              select new QuoteTemplateSectionPM()
                                           {
                                               Id = a.Id,
                                               Tenant = a.Tenant,
                                               QuoteTemplateId = a.QuoteTemplateId,
                                               SectionDocId = a.SectionDocId,
                                               Order = a.Order,
                                               Name=a.Name,
                                               QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                              IsCancel =  a.IsCancel,
                                               Description = a.Description,
                                               
                 
                                           });

                        foreach (var c in quoteTemplatesSection)
                        {
                            string cname = "QuoteTemplateSectionPM" + c.Id + c.Tenant;

                            if (CacheManager.CacheWrapper.Get(cname) == null)
                            {
                                CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                        entity = (QuoteTemplateSectionPM)CacheManager.CacheWrapper.Get(entityName);

                    }
                    else
                    {
                        entity = (QuoteTemplateSectionPM)CacheManager.CacheWrapper.Get(entityName);
                        // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                }
                else
                {
                    entity = (from a in repository.quotesContext.QuoteTemplateSections
                              where a.Tenant == tenant && a.Id == id && a.IsCancel != true
                              select new QuoteTemplateSectionPM()
                            {
                                Id = a.Id,
                                Tenant =a.Tenant,
                                QuoteTemplateId = a.QuoteTemplateId,
                                SectionDocId = a.SectionDocId,
                                Order = a.Order,
                                Name = a.Name,
                                QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                IsCancel = a.IsCancel,
                                Description = a.Description,
                              }).FirstOrDefault();
                }
                //QuoteTemplateSectionPM securedPm = new QuoteTemplateSectionPM();
                //SecuredMapping.GetMappedPM(entity, securedPm, "QuoteTemplateSection", tenant);

                return entity;
            }
            return null;
        }

        public IQueryable<QuoteTemplateSectionPM> GetQuoteTemplateSectionPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateSectionPM> qUoteTemplateSection = from a in repository.quotesContext.QuoteTemplateSections
                                                   where a.Tenant == tenant && a.IsCancel != true
                                                                      select new QuoteTemplateSectionPM()
                                                   {
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                                       QuoteTemplateId = a.QuoteTemplateId,
                                                       SectionDocId = a.SectionDocId,
                                                       Order = a.Order,
                                                       Name = a.Name,
                                                       QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                                       IsCancel = a.IsCancel,
                                                       Description = a.Description,
                                                   };
            return qUoteTemplateSection.OrderBy(x => x.Order);
           
        }

        public List<QuoteTemplateSectionPM> GetQuoteTemplateSectionPMsByTemplateId(string templateId,int tenant)
        {
            IQueryable<QuoteTemplateSectionPM> qUoteTemplateSection = from a in repository.quotesContext.QuoteTemplateSections
                                                                      where a.Tenant == tenant && a.QuoteTemplateId == templateId && a.IsCancel != true
                                                                      select new QuoteTemplateSectionPM()
                                                                      {
                                                                          Id = a.Id,
                                                                          Tenant = a.Tenant,
                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                          SectionDocId = a.SectionDocId,
                                                                          Order = a.Order,
                                                                          Name = a.Name,
                                                                          QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                                                          IsCancel = a.IsCancel,
                                                                          Description = a.Description,
                                                                      };
            return qUoteTemplateSection.OrderBy(x => x.Order).ToList();
        }


        public List<QuoteTemplateSectionPM> GetQuoteTemplateSectionPMsByIds(List<string> sectionIds, int tenant)
        {
            IQueryable<QuoteTemplateSectionPM> qUoteTemplateSection = from a in repository.quotesContext.QuoteTemplateSections
                                                                      where a.Tenant == tenant && sectionIds.Contains(a.Id)
                                                                      select new QuoteTemplateSectionPM()
                                                                      {
                                                                          Id = a.Id,
                                                                          Tenant = a.Tenant,
                                                                          QuoteTemplateId = a.QuoteTemplateId,
                                                                          SectionDocId = a.SectionDocId,
                                                                          Order = a.Order,
                                                                          Name = a.Name,
                                                                          QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                                                          IsCancel = a.IsCancel,
                                                                          Description = a.Description,
                                                                      };
            return qUoteTemplateSection.OrderBy(x => x.Order).ToList();
        }



        public List<QuoteTemplateSectionPM> GetQuoteEditableTemplateSectionPMsByTemplateId(string templateId,string quoteId, int tenant)
        {
            List<QuoteTemplateSectionPM> qUoteTemplateSections = (from a in repository.quotesContext.QuoteTemplateSections
                                                                 where a.Tenant == tenant && a.QuoteTemplateId == templateId && a.QuoteTemplateSectionTypeCode == "S" && a.IsCancel != true
                                                                 select new QuoteTemplateSectionPM()
                                                                 {
                                                                     Id = a.Id,
                                                                     Tenant = a.Tenant,
                                                                     QuoteTemplateId = a.QuoteTemplateId,
                                                                     SectionDocId = a.SectionDocId,
                                                                     Order = a.Order,
                                                                     Name = a.Name,
                                                                     QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                                                     IsCancel = a.IsCancel,
                                                                     QuoteId = quoteId,
                                                                 }).OrderBy(x => x.Order).ToList();


            if (qUoteTemplateSections != null)
            {
                List<QuoteTemplateSectionModification> modifications = repository.GetAllQuoteTemplateSectionModifications(quoteId, tenant).ToList();
                foreach (QuoteTemplateSectionModification mod in modifications)
                {
                    QuoteTemplateSectionPM section = qUoteTemplateSections.Where(s => s.Id == mod.QuoteTemplateSectionId).FirstOrDefault();
                    if (section != null)
                    {
                        section.SectionDocId = mod.SectionDocId;
                        section.IsQuoteEdited = true;

                    }
                }
            }

            return qUoteTemplateSections;

        }






        public List<QuoteTemplateSectionPM> GetQuoteTemplateSectionPMsByTemplateId(string templateId, string quoteId, int tenant , string defultQuoteTemplate=null, string quotationSections = null)
        {
            List<QuoteTemplateSectionPM> qUoteTemplateSections = new List<QuoteTemplateSectionPM>();

            if (string.IsNullOrEmpty(defultQuoteTemplate) || string.IsNullOrEmpty(quotationSections) || defultQuoteTemplate != templateId  )
            {
                qUoteTemplateSections = (from a in repository.quotesContext.QuoteTemplateSections
                                         where a.Tenant == tenant && a.QuoteTemplateId == templateId && a.IsCancel != true
                                         select new QuoteTemplateSectionPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             QuoteTemplateId = a.QuoteTemplateId,
                                             SectionDocId = a.SectionDocId,
                                             Order = a.Order,
                                             Name = a.Name,
                                             QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                             IsCancel = a.IsCancel,
                                             QuoteId = quoteId,
                                         }).OrderBy(x => x.Order).ToList();

            }
            else
            {
                List<string> quoteTemplateIds = quotationSections.Split(',').ToList();
                qUoteTemplateSections = (from a in repository.quotesContext.QuoteTemplateSections
                                         where a.Tenant == tenant && quoteTemplateIds.Contains(a.Id)
                                         select new QuoteTemplateSectionPM()
                                         {
                                             Id = a.Id,
                                             Tenant = a.Tenant,
                                             QuoteTemplateId = a.QuoteTemplateId,
                                             SectionDocId = a.SectionDocId,
                                             Order = a.Order,
                                             Name = a.Name,
                                             QuoteTemplateSectionTypeCode = a.QuoteTemplateSectionTypeCode,
                                             IsCancel = a.IsCancel,
                                             QuoteId = quoteId,
                                         }).OrderBy(x => x.Order).ToList();
            }


            if (qUoteTemplateSections != null)
            {
                List<QuoteTemplateSectionModification> modifications = repository.GetAllQuoteTemplateSectionModifications(quoteId, tenant).ToList();
                foreach (QuoteTemplateSectionModification mod in modifications)
                {
                    QuoteTemplateSectionPM section = qUoteTemplateSections.Where(s => s.Id == mod.QuoteTemplateSectionId).FirstOrDefault();
                    if (section != null)
                    {
                        section.SectionDocId = mod.SectionDocId;
                        section.IsQuoteEdited = true;

                    }
                }


                excludedSectionRepository = new QuoteTemplateExcludedSectionRepository(tenant);
                List<QuoteTemplateExcludedSection> excludedsection = excludedSectionRepository.GetAllQuoteTemplateExcludedSection(quoteId, templateId, tenant).ToList();


                if (excludedsection != null)
                {
                    foreach (QuoteTemplateExcludedSection mod2 in excludedsection)
                    {
                        QuoteTemplateSectionPM section = qUoteTemplateSections.Where(s => s.Id == mod2.QuoteTemplateSectionId).FirstOrDefault();
                        if (section != null)
                        {
                            section.IsExcluded = true;

                        }

                    }
                }



            }


            return qUoteTemplateSections;

        }



        public IQueryable<QuoteTemplateSectionList> GetIQueryableEntityList(IQueryable<QuoteTemplateSection> iQueryable)
        {
            IQueryable<QuoteTemplateSectionList> result = from quoteTemplateSection in iQueryable
                                                          where quoteTemplateSection.IsCancel != true 
                                                          select new QuoteTemplateSectionList()
                                                {
                                                    Id = quoteTemplateSection.Id,

                                                    QuoteTemplateId = quoteTemplateSection.QuoteTemplateId,
                                                    SectionDocId = quoteTemplateSection.SectionDocId,
                                                    Order = quoteTemplateSection.Order,
                                                    Name = quoteTemplateSection.Name,
                                                    QuoteTemplateSectionTypeCode = quoteTemplateSection.QuoteTemplateSectionTypeCode,
                                                    IsCancel = quoteTemplateSection.IsCancel,
                                                    Description = quoteTemplateSection.Description,
                                                };
            return result;
        }

        public QuoteTemplateSection GetFirstQuoteTemplateSectionForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteTemplateSections
                    where a.Tenant == tenant  && a.IsCancel !=true 
                    select a).FirstOrDefault();
        }
    }
}