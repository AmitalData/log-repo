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
    public class QuoteTemplateTextCodeQuery
    {

         QuoteTemplateTextCodeRepository repository;
             
        public QuoteTemplateTextCodeQuery()
        {
            repository = new QuoteTemplateTextCodeRepository(); 
        }

        public QuoteTemplateTextCodeQuery(int tenant)
        {
            repository = new QuoteTemplateTextCodeRepository(tenant);
        }

        public QuoteTemplateTextCodeQuery(QuoteTemplateTextCodeRepository quotetemplatetextcodequeryRepository)
        {
            repository = quotetemplatetextcodequeryRepository;
        }

        //public QuoteTemplateTextCodePM GetSinglePM(string id, int tenant)
        //{
        //    if (!string.IsNullOrEmpty(id))
        //    {
        //        string entityName = "QuoteTemplateTextCodePM" + id + tenant;
        //        QuoteTemplateTextCodePM entity;
        //        if (HttpContext.Current != null)
        //        {
        //            if (CacheManager.CacheWrapper.Get(entityName) == null)
        //            {

        //                var quotetemplatetextcodes = (from a in repository.quotesContext.QuoteTemplateTextCodes
        //                                              where a.Tenant == tenant
        //                                              select new QuoteTemplateTextCodePM()
        //                                              {
        //                                                  Id = a.Id,
        //                                                  Tenant = a.Tenant,
        //                                                  TextCode = a.TextCode,
        //                                                  EnglishName = a.EnglishName,
        //                                                  LocalName = a.LocalName,
        //                                                  QuoteTemplateId = a.QuoteTemplateId,
        //                                                  Area = a.Area,

        //                                              });

        //                foreach (var c in quotetemplatetextcodes)
        //                {
        //                    string cname = "QuoteTemplateTextCodePM" + c.Id + c.Tenant;

        //                    if (CacheManager.CacheWrapper.Get(cname) == null)
        //                    {
        //                        CacheManager.CacheWrapper.Insert(cname, c, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //                    }
        //                }
        //                entity = (QuoteTemplateTextCodePM)CacheManager.CacheWrapper.Get(entityName);

        //            }
        //            else
        //            {
        //                entity = (QuoteTemplateTextCodePM)CacheManager.CacheWrapper.Get(entityName);
        //                // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
        //            }
        //        }
        //        else
        //        {
        //            entity = (from a in repository.quotesContext.QuoteTemplateTextCodes
        //                      where a.Tenant == tenant && a.Id == id
        //                      select new QuoteTemplateTextCodePM()
        //                      {
        //                          Id = a.Id,
        //                          Tenant = a.Tenant,
        //                          TextCode = a.TextCode,
        //                          EnglishName = a.EnglishName,
        //                          LocalName = a.LocalName,
        //                          QuoteTemplateId = a.QuoteTemplateId,
        //                          Area = a.Area,
        //                      }).FirstOrDefault();
        //        }

        //        return entity;
        //    }
        //    return null;
        //}

        public QuoteTemplateTextCodePM GetSinglePM(string id, int tenant)
        {
            QuoteTemplateTextCodePM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateTextCodes
                      where a.Tenant == tenant && a.Id == id
                      select new QuoteTemplateTextCodePM()
                      {
                          Id = a.Id,

                          Tenant = a.Tenant,
                          TextCode = a.TextCode,
                          EnglishName = a.EnglishName,
                          LocalName = a.LocalName,
                          OriginalEnglishName = a.OriginalEnglishName,
                          OriginalLocalName = a.OriginalLocalName,
                        
                          QuoteTemplateId = a.QuoteTemplateId,
                          Area = a.Area,

                      }).FirstOrDefault();

            return entity;

        }




        public QuoteTemplateTextCodePM GetQuoteTemplateTextCodeByCode(string code, int tenant)
        {
            QuoteTemplateTextCodePM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateTextCodes
                      where a.Tenant == tenant && a.TextCode == code
                      select new QuoteTemplateTextCodePM()
                      {
                          Id = a.Id,

                          Tenant = a.Tenant,
                          TextCode = a.TextCode,
                          EnglishName = a.EnglishName,
                          OriginalEnglishName = a.OriginalEnglishName,
                          OriginalLocalName = a.OriginalLocalName,
                          LocalName = a.LocalName,
                          QuoteTemplateId = a.QuoteTemplateId,
                          Area = a.Area,

                      }).FirstOrDefault();

            return entity;

        }




        public IQueryable<QuoteTemplateTextCodePM> GetQuoteTemplateTextCodePMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateTextCodePM> qoutetemplatetextcode = from a in repository.quotesContext.QuoteTemplateTextCodes
                                                        where a.Tenant == tenant
                                                                select new QuoteTemplateTextCodePM()
                                                        {
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            TextCode = a.TextCode,
                                                            EnglishName = a.EnglishName,
                                                            OriginalEnglishName = a.OriginalEnglishName,
                                                            OriginalLocalName = a.OriginalLocalName,
                                                            LocalName = a.LocalName,
                                                            QuoteTemplateId = a.QuoteTemplateId,
                                                            Area = a.Area,
                                                        };
            return qoutetemplatetextcode;
        }



        public IQueryable<QuoteTemplateTextCodeList> GetIQueryableEntityList(IQueryable<QuoteTemplateTextCode> iQueryable)
        {
            IQueryable<QuoteTemplateTextCodeList> result = from quotetemplatetextcode in iQueryable.Include("Name")

                                                           select new QuoteTemplateTextCodeList()
                                                   {
                                                       Id = quotetemplatetextcode.Id,
                                                       Tenant = quotetemplatetextcode.Tenant,
                                                       TextCode = quotetemplatetextcode.TextCode,
                                                       EnglishName = quotetemplatetextcode.EnglishName,
                                                       LocalName = quotetemplatetextcode.LocalName,
                                                       OriginalEnglishName = quotetemplatetextcode.OriginalEnglishName,
                                                       OriginalLocalName = quotetemplatetextcode.OriginalLocalName,
                                                       QuoteTemplateId = quotetemplatetextcode.QuoteTemplateId,
                                                       Area = quotetemplatetextcode.Area,

                                                   };
            return result;
        }

        public QuoteTemplateTextCode GetFirstQuoteTemplateTextCodeForTenant(int tenant)
        {
            return (from a in repository.quotesContext.QuoteTemplateTextCodes
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }





        public IQueryable<QuoteTemplateTextCodePM> GetQuoteTemplateTextCodePMsByQuoteTemplateId(int tenant, string quotetemplateid)
        {
            IQueryable<QuoteTemplateTextCodePM> qoutetemplatetextcode = from a in repository.quotesContext.QuoteTemplateTextCodes
                                                                        where a.Tenant == tenant && a.QuoteTemplateId == quotetemplateid
                                                                        select new QuoteTemplateTextCodePM()
                                                                        {
                                                                            Id = a.Id,
                                                                            Tenant = a.Tenant,
                                                                            TextCode = a.TextCode,
                                                                            EnglishName = a.EnglishName,
                                                                            LocalName = a.LocalName,
                                                                            OriginalEnglishName = a.OriginalEnglishName,
                                                                            OriginalLocalName = a.OriginalLocalName,
                                                                            QuoteTemplateId = a.QuoteTemplateId,
                                                                            Area = a.Area,
                                                                        };
            return qoutetemplatetextcode;
        }

        public IQueryable<QuoteTemplateTextCodePM> GetQuoteTemplateTextCodePMsByArea(int tenant, string quotetemplateid , string area)
        {
            IQueryable<QuoteTemplateTextCodePM> qoutetemplatetextcode = from a in repository.quotesContext.QuoteTemplateTextCodes
                                                                        where a.Tenant == tenant && a.QuoteTemplateId == quotetemplateid && a.Area == area
                                                                        select new QuoteTemplateTextCodePM()
                                                                        {
                                                                            Id = a.Id,
                                                                            Tenant = a.Tenant,
                                                                            TextCode = a.TextCode,
                                                                            EnglishName = a.EnglishName,
                                                                            LocalName = a.LocalName,
                                                                            OriginalEnglishName = a.OriginalEnglishName,
                                                                            OriginalLocalName = a.OriginalLocalName,
                                                                            QuoteTemplateId = a.QuoteTemplateId,
                                                                            Area = a.Area,
                                                                        };
            return qoutetemplatetextcode;
        }
    }
}