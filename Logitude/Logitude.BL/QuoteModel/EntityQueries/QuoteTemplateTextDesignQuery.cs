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
    public class QuoteTemplateTextDesignQuery
    {
        QuoteTemplateTextDesignRepository repository;

        public QuoteTemplateTextDesignQuery()
        {
            repository = new QuoteTemplateTextDesignRepository();
        }

        public QuoteTemplateTextDesignQuery(int tenant)
        {
            repository = new QuoteTemplateTextDesignRepository(tenant);
        }

        public QuoteTemplateTextDesignQuery(QuoteTemplateTextDesignRepository quoteTemplateTextDesignRepository)
        {
            repository = quoteTemplateTextDesignRepository;
        }

        //public QuoteTemplateTextDesignPM GetSinglePM(string id, int tenant)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        string entityName = "QuoteTemplateTextDesignPM" + id + tenant;
        //        QuoteTemplateTextDesignPM entity;
        //        if (HttpContext.Current != null)
        //        {
        //            if (CacheManager.CacheWrapper.Get(entityName) == null)
        //            {

        //                var quoteTemplateTextDesigns = (from a in repository.quotesContext.QuoteTemplateTextDesigns
        //                                                where a.Tenant == tenant
        //                                                select new QuoteTemplateTextDesignPM()
        //                                                {
        //                                                    Id = a.Id,
        //                                                    Tenant = a.Tenant,
        //                                                    FontSize = a.FontSize,
        //                                                    TextColor = a.TextColor,
        //                                                    FontFamily = a.FontFamily,
        //                                                    BackgroundColor = a.BackgroundColor,
        //                                                    FontWeight = a.FontWeight,
        //                                                    Italic = a.Italic,
        //                                                    UnDerLine = a.UnDerLine,
        //                                                    Alignment = a.Alignment,


        //                                                });

        //                foreach (var c in quoteTemplateTextDesigns)
        //                {
        //                    string cname = "QuoteTemplateTextDesignPM" + c.Id + c.Tenant;

        //                    if (CacheManager.CacheWrapper.Get(cname) == null)
        //                    {
        //                        CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //                    }
        //                }
        //                entity = (QuoteTemplateTextDesignPM)CacheManager.CacheWrapper.Get(entityName);

        //            }
        //            else
        //            {
        //                entity = (QuoteTemplateTextDesignPM)CacheManager.CacheWrapper.Get(entityName);
        //                // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //            }
        //        }
        //        else
        //        {
        //            entity = (from a in repository.quotesContext.QuoteTemplateTextDesigns
        //                      where a.Tenant == tenant && a.Id == id
        //                      select new QuoteTemplateTextDesignPM()
        //                    {
        //                        Id = a.Id,
        //                        Tenant = a.Tenant,
        //                        FontSize = a.FontSize,
        //                        TextColor = a.TextColor,
        //                        FontFamily = a.FontFamily,
        //                        BackgroundColor = a.BackgroundColor,
        //                        FontWeight = a.FontWeight,
        //                        Italic = a.Italic,
        //                        UnDerLine = a.UnDerLine,
        //                        Alignment = a.Alignment,


        //                    }).FirstOrDefault();
        //        }

        //        return entity;
        //    }
        //    return null;
        //}


        public QuoteTemplateTextDesignPM GetSinglePM(string id, int tenant)
        {
            QuoteTemplateTextDesignPM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateTextDesigns
                      where a.Tenant == tenant && a.Id == id
                      select new QuoteTemplateTextDesignPM()
                      {
                          Id = a.Id,
                          Tenant = a.Tenant,
                          FontSize = a.FontSize,
                          TextColor = a.TextColor,
                          FontFamily = a.FontFamily,
                          BackgroundColor = a.BackgroundColor,
                          FontWeight = a.FontWeight,
                          Italic = a.Italic,
                          UnDerLine = a.UnDerLine,
                          Alignment = a.Alignment,


                      }).FirstOrDefault();

            return entity;

        }




        public IQueryable<QuoteTemplateTextDesignPM> GetQuoteTemplateTextDesignPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateTextDesignPM> qUoteTemplateTextDesign = from a in repository.quotesContext.QuoteTemplateTextDesigns
                                                                            where a.Tenant == tenant
                                                                            select new QuoteTemplateTextDesignPM()
                                                                            {
                                                                                Id = a.Id,
                                                                                Tenant = a.Tenant,
                                                                                FontSize = a.FontSize,
                                                                                TextColor = a.TextColor,
                                                                                FontFamily = a.FontFamily,
                                                                                BackgroundColor = a.BackgroundColor,
                                                                                FontWeight = a.FontWeight,
                                                                                Italic = a.Italic,
                                                                                UnDerLine = a.UnDerLine,
                                                                                Alignment = a.Alignment,
                                                                            };
            return qUoteTemplateTextDesign;
        }

        public IQueryable<QuoteTemplateTextDesignList> GetIQueryableEntityList(IQueryable<QuoteTemplateTextDesign> iQueryable)
        {
            IQueryable<QuoteTemplateTextDesignList> result = from quote in iQueryable.Include("Name")

                                                             select new QuoteTemplateTextDesignList()
                                                             {
                                                                 Id = quote.Id,
                                                                 Tenant = quote.Tenant,
                                                                 FontSize = quote.FontSize,
                                                                 TextColor = quote.TextColor,
                                                                 FontFamily = quote.FontFamily,
                                                                 BackgroundColor = quote.BackgroundColor,
                                                                 FontWeight = quote.FontWeight,
                                                                 Italic = quote.Italic,
                                                                 UnDerLine = quote.UnDerLine,
                                                                 Alignment = quote.Alignment,

                                                             };
            return result;
        }

        public QuoteTemplateTextDesign GetFirstQuoteTemplateTextDesignForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteTemplateTextDesigns
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }






        public List<QuoteTemplateTextDesignPM> GetQuoteTemplateTextDesignPMListByIds(string[] idsList, int tenant)
        {
            List<QuoteTemplateTextDesignPM> qUoteTemplateTextDesignLists = (from a in repository.quotesContext.QuoteTemplateTextDesigns
                                                                            where a.Tenant == tenant && idsList.Contains(a.Id)
                                                                            select new QuoteTemplateTextDesignPM()
                                                                            {
                                                                                Id = a.Id,
                                                                                Tenant = a.Tenant,
                                                                                FontSize = a.FontSize,
                                                                                TextColor = a.TextColor,
                                                                                FontFamily = a.FontFamily,
                                                                                BackgroundColor = a.BackgroundColor,
                                                                                FontWeight = a.FontWeight,
                                                                                Italic = a.Italic,
                                                                                UnDerLine = a.UnDerLine,
                                                                                Alignment = a.Alignment,
                                                                            }).ToList();
            return qUoteTemplateTextDesignLists;
        }

    }
}