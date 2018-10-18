using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteTypeQuery
    {
        QuoteTypeRepository repository;
        public QuoteTypeQuery()
        {
            repository = new QuoteTypeRepository(); 
        }

        public QuoteTypeQuery(int tenant)
        {
            repository = new QuoteTypeRepository(tenant);
        }

        public QuoteTypeQuery(QuoteTypeRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public QuoteTypePM GetSingleQuoteTypePM(string code)
        {
            return (from a in repository.context.QuoteTypes
                    where a.Code == code
                    select new QuoteTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public QuoteTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.QuoteTypes
                    where a.Code == code
                    select new QuoteTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public QuoteTypePM GetSinglePMByCode(string code,int tenant)
        {
            return (from a in repository.context.QuoteTypes
                    where a.Code == code
                    select new QuoteTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        

        public QuoteTypePM GetSinglePM(string code,int tenant)
        {
            return (from a in repository.context.QuoteTypes
                    where a.Code == code
                    select new QuoteTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }




        public IQueryable<QuoteTypeList> GetIQueryableEntityList(IQueryable<QuoteType> iQueryable)
        {
            IQueryable<QuoteTypeList> result = from entity in iQueryable
                                               select new QuoteTypeList()
                                               {
                                                   Name = entity.Name,
                                                   Code = entity.Code,
                                                   SearchFields = entity.SearchFields,
                                               };
            return result;
        }
    }
}