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
    public class QuoteTemplateTableDesignQuery
    {
        QuoteTemplateTableDesignRepository repository;

        public QuoteTemplateTableDesignQuery()
        {
            repository = new QuoteTemplateTableDesignRepository();
        }

        public QuoteTemplateTableDesignQuery(int tenant)
        {
            repository = new QuoteTemplateTableDesignRepository(tenant);
        }

        public QuoteTemplateTableDesignQuery(QuoteTemplateTableDesignRepository quoteTemplateTableDesignRepository)
        {
            repository = quoteTemplateTableDesignRepository;
        }

        public QuoteTemplateTableDesignPM GetSinglePM(string id, int tenant)
        {
            QuoteTemplateTableDesignPM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateTableDesigns
                      where a.Tenant == tenant && a.Id == id
                      select new QuoteTemplateTableDesignPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          BorderColor = a.BorderColor,
                          BorderThickness = a.BorderThickness,
                          BorderTypeCode = a.BorderTypeCode,
                          GroupByDesignId = a.GroupByDesignId,
                          LinesDesignId = a.LinesDesignId,
                          HeaderDesignId = a.HeaderDesignId,

                      }).FirstOrDefault();

            return entity;

        }
        //public QuoteTemplateTableDesignPM GetSinglePM(string id, int tenant)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        string entityName = "QuoteTemplateTableDesignPM" + id + tenant;
        //        QuoteTemplateTableDesignPM entity;
        //        if (HttpContext.Current != null)
        //        {
        //            if (CacheManager.CacheWrapper.Get(entityName) == null)
        //            {

        //                var quoteTemplateTableDesigns = (from a in repository.quotesContext.QuoteTemplateTableDesigns
        //                                                 where a.Tenant == tenant
        //                                                 select new QuoteTemplateTableDesignPM()
        //                                                 {
        //                                                     Id = a.Id,
        //                                                     Tenant = a.Tenant,
        //                                                     BorderColor = a.BorderColor,
        //                                                     BorderThickness = a.BorderThickness,
        //                                                     BorderTypeCode = a.BorderTypeCode,
        //                                                     GroupByDesignId = a.GroupByDesignId,
        //                                                     LinesDesignId = a.LinesDesignId,
        //                                                     HeaderDesignId = a.HeaderDesignId,



        //                                                 });

        //                foreach (var c in quoteTemplateTableDesigns)
        //                {
        //                    string cname = "QuoteTemplateTableDesignPM" + c.Id + c.Tenant;

        //                    if (CacheManager.CacheWrapper.Get(cname) == null)
        //                    {
        //                        CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //                    }
        //                }
        //                entity = (QuoteTemplateTableDesignPM)CacheManager.CacheWrapper.Get(entityName);

        //            }
        //            else
        //            {
        //                entity = (QuoteTemplateTableDesignPM)CacheManager.CacheWrapper.Get(entityName);
        //                // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //            }
        //        }
        //        else
        //        {
        //            entity = (from a in repository.quotesContext.QuoteTemplateTableDesigns
        //                      where a.Tenant == tenant && a.Id == id
        //                      select new QuoteTemplateTableDesignPM()
        //                    {
        //                        Id = a.Id,
        //                        Tenant = a.Tenant,
        //                        BorderColor = a.BorderColor,
        //                        BorderThickness = a.BorderThickness,
        //                        BorderTypeCode = a.BorderTypeCode,
        //                        GroupByDesignId = a.GroupByDesignId,
        //                        LinesDesignId = a.LinesDesignId,
        //                        HeaderDesignId = a.HeaderDesignId,


        //                    }).FirstOrDefault();
        //        }

        //        return entity;
        //    }
        //    return null;
        //}



        public IQueryable<QuoteTemplateTableDesignPM> GetQuoteTemplateTableDesignPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateTableDesignPM> qUoteTemplateTableDesign = from a in repository.quotesContext.QuoteTemplateTableDesigns
                                                                              where a.Tenant == tenant
                                                                              select new QuoteTemplateTableDesignPM()
                                                                              {
                                                                                  Id = a.Id,
                                                                                  Tenant = a.Tenant,
                                                                                  BorderColor = a.BorderColor,
                                                                                  BorderThickness = a.BorderThickness,
                                                                                  BorderTypeCode = a.BorderTypeCode,
                                                                                  GroupByDesignId = a.GroupByDesignId,
                                                                                  LinesDesignId = a.LinesDesignId,
                                                                                  HeaderDesignId = a.HeaderDesignId,
                                                                              };
            return qUoteTemplateTableDesign;
        }

        public IQueryable<QuoteTemplateTableDesignList> GetIQueryableEntityList(IQueryable<QuoteTemplateTableDesign> iQueryable)
        {
            IQueryable<QuoteTemplateTableDesignList> result = from quote in iQueryable.Include("Name")

                                                              select new QuoteTemplateTableDesignList()
                                                              {
                                                                  Id = quote.Id,
                                                                  Tenant = quote.Tenant,
                                                                  BorderColor = quote.BorderColor,
                                                                  BorderThickness = quote.BorderThickness,
                                                                  BorderTypeCode = quote.BorderTypeCode,
                                                                  GroupByDesignId = quote.GroupByDesignId,
                                                                  LinesDesignId = quote.LinesDesignId,
                                                                  HeaderDesignId = quote.HeaderDesignId,

                                                              };
            return result;
        }

        public QuoteTemplateTableDesign GetFirstQuoteTemplateTableDesignForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteTemplateTableDesigns
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }


    }
}