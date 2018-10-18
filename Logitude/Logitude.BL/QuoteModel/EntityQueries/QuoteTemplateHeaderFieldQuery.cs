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
    public class QuoteTemplateHeaderFieldQuery
    {


        QuoteTemplateHeaderFieldRepository repository;
             
        public QuoteTemplateHeaderFieldQuery()
        {
            repository = new QuoteTemplateHeaderFieldRepository(); 
        }

        public QuoteTemplateHeaderFieldQuery(int tenant)
        {
            repository = new QuoteTemplateHeaderFieldRepository(tenant);
        }

        public QuoteTemplateHeaderFieldQuery(QuoteTemplateHeaderFieldRepository quoteTemplateHeaderFieldRepository)
        {
            repository = quoteTemplateHeaderFieldRepository;
        }
     
        public QuoteTemplateHeaderFieldPM GetSinglePM(string id ,int  tenant)
        {
            QuoteTemplateHeaderFieldPM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateHeaderFields
                      where a.Id == id && a.Tenant == tenant
                                              select new QuoteTemplateHeaderFieldPM()

                                              {
                                                       Id = a.Id,
                                                       FieldCode = a.FieldCode,
                                                       Column = a.Column,
                                                       Row = a.Row,
                                                       QuoteTemplateId = a.QuoteTemplateId,
                                                       Tenant = a.Tenant,
                                           }).FirstOrDefault();

            return entity;
         
        }

        public IQueryable<QuoteTemplateHeaderFieldPM> GetQuoteTemplateHeaderFieldPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateHeaderFieldPM> qUoteTemplateHeaderField = from a in repository.quotesContext.QuoteTemplateHeaderFields

                                                                                where a.Tenant == tenant
                                               select new QuoteTemplateHeaderFieldPM()
                                                   {
                                                       FieldCode = a.FieldCode,
                                                       Column = a.Column,
                                                       Row = a.Row,
                                                       Tenant = a.Tenant,
                                                       QuoteTemplateId = a.QuoteTemplateId,
                                                       Id = a.Id,






                                                 
                                                   };
            return qUoteTemplateHeaderField;
        }

        public IQueryable<QuoteTemplateHeaderFieldList> GetIQueryableEntityList(IQueryable<QuoteTemplateHeaderField> iQueryable)
        {
            IQueryable<QuoteTemplateHeaderFieldList> result = from quoteTemplateHeaderField in iQueryable
                                                          select new QuoteTemplateHeaderFieldList()
                                                {
                                                    Id = quoteTemplateHeaderField.Id,
                                                    Tenant = quoteTemplateHeaderField.Tenant,
                                                      FieldCode = quoteTemplateHeaderField.FieldCode,
                                                       Column = quoteTemplateHeaderField.Column,
                                                       Row = quoteTemplateHeaderField.Row,
                                                      QuoteTemplateId = quoteTemplateHeaderField.QuoteTemplateId,

                                                 
                                                };
            return result;
        }


        
        public QuoteTemplateHeaderField GetFirstQuoteTemplateHeaderFieldForTenant()
        {
            return (from a in repository.quotesContext.QuoteTemplateHeaderFields
                  
                    select a).FirstOrDefault();
        }




        public IQueryable<QuoteTemplateHeaderFieldPM> GetQuoteTemplateHeaderFieldPMsByQuotetemplateId(int tenant, string quotetemplateId)
        {
            IQueryable<QuoteTemplateHeaderFieldPM> qUoteTemplateHeaderField = from a in repository.quotesContext.QuoteTemplateHeaderFields

                                                                              where a.Tenant == tenant
                                                                              && a.QuoteTemplateId == quotetemplateId
                                                                              select new QuoteTemplateHeaderFieldPM()
                                                                              {
                                                                                  Id = a.Id,
                                                                                  Tenant = a.Tenant,
                                                                                  FieldCode = a.FieldCode,
                                                                                  Column = a.Column,
                                                                                  Row = a.Row,
                                                                                  QuoteTemplateId = a.QuoteTemplateId,
                                                                                 
                                                                              };
            return qUoteTemplateHeaderField;
        }












    }
}








