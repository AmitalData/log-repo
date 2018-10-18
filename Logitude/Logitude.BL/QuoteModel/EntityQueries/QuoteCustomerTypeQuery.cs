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
    public class QuoteCustomerTypeQuery
    {
        QuoteCustomerTypeRepository repository;
        public QuoteCustomerTypeQuery()
        {
            repository = new QuoteCustomerTypeRepository(); 
        }

        public QuoteCustomerTypeQuery(int tenant)
        {
            repository = new QuoteCustomerTypeRepository(tenant);
        }

        public QuoteCustomerTypeQuery(QuoteCustomerTypeRepository quoteCustomerTypeRepository)
        {
            repository = quoteCustomerTypeRepository;
        }
        public QuoteCustomerTypePM GetSingleQuoteCustomerTypePM(string code)
        {
            return (from a in repository.context.QuoteCustomerTypes
                    where a.Code == code
                    select new QuoteCustomerTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public QuoteCustomerTypePM GetSinglePM(string code)
        {
            return (from a in repository.context.QuoteCustomerTypes
                    where a.Code == code
                    select new QuoteCustomerTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public QuoteCustomerTypePM GetSinglePMByCode(string code,int tenant)
        {
            return (from a in repository.context.QuoteCustomerTypes
                    where a.Code == code
                    select new QuoteCustomerTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }
        

        public QuoteCustomerTypePM GetSinglePM(string code,int tenant)
        {
            return (from a in repository.context.QuoteCustomerTypes
                    where a.Code == code
                    select new QuoteCustomerTypePM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<QuoteCustomerTypeList> GetIQueryableEntityList(IQueryable<QuoteCustomerType> iQueryable)
        {
            IQueryable<QuoteCustomerTypeList> result = from entity in iQueryable
                                                       select new QuoteCustomerTypeList()
                                                       {
                                                           ShowInLOV = entity.ShowInLOV,
                                                           Name = entity.Name,
                                                           Code = entity.Code,
                                                           SearchFields = entity.SearchFields,
                                                       };
            return result;
        }
    }
}