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
    public class QuoteTemplateQuery
    {
        QuoteTemplateRepository repository;
             
        public QuoteTemplateQuery()
        {
            repository = new QuoteTemplateRepository(); 
        }

        public QuoteTemplateQuery(int tenant)
        {
            repository = new QuoteTemplateRepository(tenant);
        }

        public QuoteTemplateQuery(QuoteTemplateRepository quoteTemplateRepository)
        {
            repository = quoteTemplateRepository;
        }

        public List<QuoteTemplateList> GetQuoteTemplateListsByIds(List<string> shipmentids)
        {
            List<QuoteTemplateList> result = (from quote in repository.quotesContext.QuoteTemplates

                                                   select new QuoteTemplateList()
                                                   {
                                                       Id = quote.Id,
                                                       Name = quote.Name,
                          
                                                   }).ToList();

            return result;
        }

        //public QuoteTemplatePM GetSinglePM(string id, int tenant)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        string entityName = "QuoteTemplatePM" + id + tenant;
        //        QuoteTemplatePM entity;
        //        if (HttpContext.Current != null)
        //        {
        //            if (CacheManager.CacheWrapper.Get(entityName) == null)
        //            {

        //                var quoteTemplates = (from a in repository.quotesContext.QuoteTemplates
        //                                   where a.Tenant == tenant
        //                                   select new QuoteTemplatePM()
        //                                   {
        //                                       Id = a.Id,
        //                                       HeaderDocId = a.HeaderDocId,
        //                                       FooterDocId = a.FooterDocId,
        //                                       QuoteTemplateSettingId = a.QuoteTemplateSettingId,
        //                                       Name = a.Name,
        //                                       IsTemplate = a.IsTemplate,
        //                                       OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
        //                                       CreateDate = a.CreateDate,
        //                                       UpdateDate = a.UpdateDate,
        //                                       CreatedByUserId = a.CreatedByUserId,
        //                                       UpdatedByUserId = a.UpdatedByUserId,
        //                                          Tenant = a.Tenant,
        //                                       SearchFields = a.SearchFields,
        //                                       TemplateTypeCode = a.TemplateTypeCode,


        //                                   });

        //                foreach (var c in quoteTemplates)
        //                {
        //                    string cname = "QuoteTemplatePM" + c.Id + c.Tenant;

        //                    if (CacheManager.CacheWrapper.Get(cname) == null)
        //                    {
        //                        CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //                    }
        //                }
        //                entity = (QuoteTemplatePM)CacheManager.CacheWrapper.Get(entityName);

        //            }
        //            else
        //            {
        //                entity = (QuoteTemplatePM)CacheManager.CacheWrapper.Get(entityName);
        //                // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //            }
        //        }
        //        else
        //        {
        //            entity = (from a in repository.quotesContext.QuoteTemplates
        //                      where a.Tenant == tenant && a.Id == id
        //                      select new QuoteTemplatePM()
        //                    {
        //                        Id = a.Id,
        //                        HeaderDocId = a.HeaderDocId,
        //                        FooterDocId = a.FooterDocId,
        //                        QuoteTemplateSettingId = a.QuoteTemplateSettingId,
        //                        Name = a.Name,
        //                        IsTemplate = a.IsTemplate,
        //                        OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
        //                        CreateDate = a.CreateDate,
        //                        UpdateDate = a.UpdateDate,
        //                        CreatedByUserId = a.CreatedByUserId,
        //                        UpdatedByUserId = a.UpdatedByUserId,
        //                        SearchFields = a.SearchFields,
        //                   TemplateTypeCode = a.TemplateTypeCode,
        //                        Tenant = a.Tenant,

        //                      }).FirstOrDefault();
        //        }

        //        return entity;
        //    }
        //    return null;
        //}

        public QuoteTemplatePM GetSinglePM(string id, int tenant)
        {
            QuoteTemplatePM entity;
            entity = (from a in repository.quotesContext.QuoteTemplates
                      where a.Tenant == tenant && a.Id == id
                      select new QuoteTemplatePM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          HeaderDocId = a.HeaderDocId,
                          FooterDocId = a.FooterDocId,
                          QuoteTemplateSettingId = a.QuoteTemplateSettingId,
                          Name = a.Name,
                          IsTemplate = a.IsTemplate,
                          OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,
                          CreatedByUserId = a.CreatedByUserId,
                          UpdatedByUserId = a.UpdatedByUserId,
                          SearchFields = a.SearchFields,
                          TemplateTypeCode = a.TemplateTypeCode,
                          IsDefault = a.IsDefault,
                          InActive = a.InActive,
                          IsEnabledForCustomers = a.IsEnabledForCustomers,
                          IsCopiedAtSignup = a.IsCopiedAtSignup,
                      }).FirstOrDefault();

           

            return entity;

        }


        public QuoteTemplatePM GetSinglePMByQuoteId(string id, string quoteId, int tenant, string defultQuoteTemplate=null, string quotationSections=null)
        {


            QuoteTemplatePM entity;
            entity = (from a in repository.quotesContext.QuoteTemplates
                      where a.Tenant == tenant && a.Id == id
                      select new QuoteTemplatePM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          HeaderDocId = a.HeaderDocId,
                          FooterDocId = a.FooterDocId,
                          QuoteTemplateSettingId = a.QuoteTemplateSettingId,
                          Name = a.Name,
                          IsTemplate = a.IsTemplate,
                          OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,
                          CreatedByUserId = a.CreatedByUserId,
                          UpdatedByUserId = a.UpdatedByUserId,
                          SearchFields = a.SearchFields,
                          TemplateTypeCode = a.TemplateTypeCode,
                          IsDefault = a.IsDefault,
                          InActive = a.InActive,
                          IsEnabledForCustomers = a.IsEnabledForCustomers,
                          IsCopiedAtSignup = a.IsCopiedAtSignup,
                      }).FirstOrDefault();

          

            QuoteTemplateSectionQuery quoteTemplateSectionQuery = new QuoteTemplateSectionQuery(tenant);
            entity.TemplateSections = quoteTemplateSectionQuery.GetQuoteTemplateSectionPMsByTemplateId(entity.Id, quoteId, tenant, defultQuoteTemplate, quotationSections);


            return entity;

        }

        public IQueryable<QuoteTemplatePM> GetQuoteTemplatePMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplatePM> qUoteTemplate = from a in repository.quotesContext.QuoteTemplates
                                                   where a.Tenant == tenant && a.InActive ==false
                                                        select new QuoteTemplatePM()
                                                   {
                                                       Id = a.Id,
                                                       HeaderDocId = a.HeaderDocId,
                                                       FooterDocId = a.FooterDocId,
                                                       QuoteTemplateSettingId = a.QuoteTemplateSettingId,
                                                       Name = a.Name,
                                                       IsTemplate = a.IsTemplate,
                                                       OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                                                       CreateDate = a.CreateDate,
                                                       UpdateDate = a.UpdateDate,
                                                       CreatedByUserId = a.CreatedByUserId,
                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                       SearchFields = a.SearchFields,
                                                       TemplateTypeCode = a.TemplateTypeCode,
                                                       Tenant = a.Tenant,
                                                       IsDefault = a.IsDefault,
                                                       InActive = a.InActive,
                                                       IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                       IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                        };
            return qUoteTemplate;
        }

        public IQueryable<QuoteTemplateList> GetQuoteTemplateListsFromLibrary(int tenant)
        {
            IQueryable<QuoteTemplateList> qUoteTemplate = from a in repository.quotesContext.QuoteTemplates
                                                        where a.Tenant == tenant && a.IsEnabledForCustomers ==true && a.InActive == false
                                                        select new QuoteTemplateList()
                                                        {
                                                            Id = a.Id,
                                                            Name = a.Name,
                                                            IsTemplate = a.IsTemplate,
                                                            OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            SearchFields = a.SearchFields,
                                                            TemplateTypeCode = a.TemplateTypeCode,
                                                            Tenant = a.Tenant,
                                                            IsDefault = a.IsDefault,
                                                            InActive = a.InActive,
                                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                        };
            return qUoteTemplate;
        }

        public IQueryable<QuoteTemplateList> GetQuoteTemplateLists()
        {
            IQueryable<QuoteTemplateList> qUoteTemplate = from a in repository.quotesContext.QuoteTemplates
                                                          where  a.InActive == false
                                                          select new QuoteTemplateList()
                                                          {
                                                              Id = a.Id,
                                                              HeaderDocId = a.HeaderDocId,
                                                              FooterDocId = a.FooterDocId,
                                                              QuoteTemplateSettingId = a.QuoteTemplateSettingId,
                                                              Name = a.Name,
                                                              IsTemplate = a.IsTemplate,
                                                              OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                                                              CreateDate = a.CreateDate,
                                                              UpdateDate = a.UpdateDate,
                                                              CreatedByUserId = a.CreatedByUserId,
                                                              UpdatedByUserId = a.UpdatedByUserId,
                                                              SearchFields = a.SearchFields,
                                                              TemplateTypeCode = a.TemplateTypeCode,
                                                              Tenant = a.Tenant,
                                                              IsDefault = a.IsDefault,
                                                              InActive = a.InActive,
                                                              IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                              IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                          };
            return qUoteTemplate;
        }

        public IQueryable<QuoteTemplatePM> GetQuoteTemplatePMsByQuoteTemplateTypeAndTenant(string templatetypecode, int tenant)
        {
            IQueryable<QuoteTemplatePM> qUoteTemplate = from a in repository.quotesContext.QuoteTemplates
                                                        where a.Tenant == tenant && a.TemplateTypeCode == templatetypecode && a.InActive == false
                                                        select new QuoteTemplatePM()
                                                        {
                                                            Id = a.Id,
                                                            HeaderDocId = a.HeaderDocId,
                                                            FooterDocId = a.FooterDocId,
                                                            QuoteTemplateSettingId = a.QuoteTemplateSettingId,
                                                            Name = a.Name,
                                                            IsTemplate = a.IsTemplate,
                                                            OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            SearchFields = a.SearchFields,
                                                            TemplateTypeCode = a.TemplateTypeCode,
                                                            Tenant = a.Tenant,
                                                            IsDefault = a.IsDefault,
                                                            InActive = a.InActive,
                                                            IsEnabledForCustomers = a.IsEnabledForCustomers,
                                                            IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                        };
 

            return qUoteTemplate;
        }






        public IQueryable<QuoteTemplateList> GetIQueryableEntityList(IQueryable<QuoteTemplate> iQueryable)
        {
            IQueryable<QuoteTemplateList> result = from quote in iQueryable.Include("Name")

                                                   select new QuoteTemplateList()
                                                {
                                                    Id = quote.Id,
                                                    HeaderDocId = quote.HeaderDocId,
                                                    FooterDocId = quote.FooterDocId,
                                                    QuoteTemplateSettingId = quote.QuoteTemplateSettingId,
                                                    Name = quote.Name,
                                                    IsTemplate = quote.IsTemplate,
                                                    OriginalQuoteTemplateId = quote.OriginalQuoteTemplateId,
                                                    CreateDate = quote.CreateDate,
                                                    UpdateDate = quote.UpdateDate,
                                                    CreatedByUserId = quote.CreatedByUserId,
                                                    UpdatedByUserId = quote.UpdatedByUserId,
                                                    SearchFields = quote.SearchFields,
                                                    TemplateTypeCode = quote.TemplateTypeCode,
                                                    Tenant = quote.Tenant,
                                                    IsDefault = quote.IsDefault,
                                                    InActive = quote.InActive,
                                                    ShowLocalLanguage = quote.QuoteTemplateSetting != null ? quote.QuoteTemplateSetting.ShowLocalLanguage : false,
                                                    TemplateTypeName = quote.QuoteType != null ? quote.QuoteType.Name : null,
                                                    IsEnabledForCustomers = quote.IsEnabledForCustomers,
                                                    IsCopiedAtSignup = quote.IsCopiedAtSignup,

                                                   };
            return result;
        }

        public QuoteTemplate GetFirstQuoteTemplateForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteTemplates
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public QuoteTemplateList GetQuoteTemplateListById(string id, int tenant)
        {
          
        return (from a in repository.quotesContext.QuoteTemplates
                      where a.Tenant == tenant && a.Id == id
                      select new QuoteTemplateList()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          HeaderDocId = a.HeaderDocId,
                          FooterDocId = a.FooterDocId,
                          QuoteTemplateSettingId = a.QuoteTemplateSettingId,
                          Name = a.Name,
                          IsTemplate = a.IsTemplate,
                          OriginalQuoteTemplateId = a.OriginalQuoteTemplateId,
                          CreateDate = a.CreateDate,
                          UpdateDate = a.UpdateDate,
                          CreatedByUserId = a.CreatedByUserId,
                          UpdatedByUserId = a.UpdatedByUserId,
                          SearchFields = a.SearchFields,
                          TemplateTypeCode = a.TemplateTypeCode,
                          IsDefault = a.IsDefault,
                          InActive = a.InActive,
                          IsEnabledForCustomers = a.IsEnabledForCustomers,
                          IsCopiedAtSignup = a.IsCopiedAtSignup,
                      }).FirstOrDefault();


        }
    }
}