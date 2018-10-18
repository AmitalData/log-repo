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
    public class QuoteTemplateDetailsFieldQuery
    {



        QuoteTemplateDetailsFieldRepository repository;
             
        public QuoteTemplateDetailsFieldQuery()
        {
            repository = new QuoteTemplateDetailsFieldRepository(); 
        }

        public QuoteTemplateDetailsFieldQuery(int tenant)
        {
            repository = new QuoteTemplateDetailsFieldRepository(tenant);
        }

        public QuoteTemplateDetailsFieldQuery(QuoteTemplateDetailsFieldRepository quoteTemplateDetailsFieldRepository)
        {
            repository = quoteTemplateDetailsFieldRepository;
        }
     
        public QuoteTemplateDetailsFieldPM GetSinglePM(string id ,int  tenant)
        {
            QuoteTemplateDetailsFieldPM entity;
            entity = (from a in repository.quotesContext.QuoteTemplateDetailsFields
                      where a.Id == id && a.Tenant == tenant
                                              select new QuoteTemplateDetailsFieldPM()

                                           {      
                                                       FieldCode = a.FieldCode,
                                                       Column = a.Column,
                                                       Row = a.Row,
                                                       QuoteTemplateId = a.QuoteTemplateId,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
            


                                           }).FirstOrDefault();

            return entity;
         
        }

        public IQueryable<QuoteTemplateDetailsFieldPM> GetQuoteTemplateDetailsFieldPMsByTenant(int tenant)
        {
            IQueryable<QuoteTemplateDetailsFieldPM> qUoteTemplateDetailsField = from a in repository.quotesContext.QuoteTemplateDetailsFields

                                                                                where a.Tenant == tenant
                                               select new QuoteTemplateDetailsFieldPM()
                                                   {
                                                       FieldCode = a.FieldCode,
                                                       Column = a.Column,
                                                       Row = a.Row,
                                                       QuoteTemplateId = a.QuoteTemplateId,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,
                                             








                                                 
                                                   };
            return qUoteTemplateDetailsField;
        }

        public IQueryable<QuoteTemplateDetailsFieldList> GetIQueryableEntityList(IQueryable<QuoteTemplateDetailsField> iQueryable)
        {
            IQueryable<QuoteTemplateDetailsFieldList> result = from quoteTemplateDetailsField in iQueryable
                                                          select new QuoteTemplateDetailsFieldList()
                                                {
                                                   
                                                      FieldCode = quoteTemplateDetailsField.FieldCode,
                                                       Column = quoteTemplateDetailsField.Column,
                                                       Row = quoteTemplateDetailsField.Row,
                                                      QuoteTemplateId = quoteTemplateDetailsField.QuoteTemplateId,
                                                      Id = quoteTemplateDetailsField.Id,
                                                      Tenant = quoteTemplateDetailsField.Tenant,

                                                 
                                                };
            return result;
        }


        
        public QuoteTemplateDetailsField GetFirstQuoteTemplateDetailsFieldForTenant()
        {
            return (from a in repository.quotesContext.QuoteTemplateDetailsFields
                  
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteTemplateDetailsFieldPM> GetQuoteTemplateDetailsFieldPMsByQuotetemplateId(int tenant , string quotetemplateId)
        {
            IQueryable<QuoteTemplateDetailsFieldPM> qUoteTemplateDetailsField = from a in repository.quotesContext.QuoteTemplateDetailsFields

                                                                                where a.Tenant == tenant && a.QuoteTemplateId == quotetemplateId
                                                                                select new QuoteTemplateDetailsFieldPM()
                                                                                {
                                                                                    FieldCode = a.FieldCode,
                                                                                    Column = a.Column,
                                                                                    Row = a.Row,
                                                                                    QuoteTemplateId = a.QuoteTemplateId,
                                                                                    Id = a.Id,
                                                                                    Tenant = a.Tenant,
                                                                                };
            return qUoteTemplateDetailsField;
        }
    }
}







