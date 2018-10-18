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
    public class QuoteClosingReasonQuery
    {
        QuoteClosingReasonRepository repository;

        public QuoteClosingReasonQuery()
        {
            repository = new QuoteClosingReasonRepository(); 
        }

        public QuoteClosingReasonQuery(int tenant)
        {
            repository = new QuoteClosingReasonRepository(tenant);
        }

        public QuoteClosingReasonQuery(QuoteClosingReasonRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public QuoteClosingReasonPM GetSingleQuoteClosingReasonPM(string code)
        {
            return (from a in repository.context.QuoteClosingReasons
                    where a.Code == code
                    select new QuoteClosingReasonPM() { Code = a.Code, Name = a.Name, SearchFields = a.SearchFields, }).FirstOrDefault();
        }

        public IQueryable<QuoteClosingReasonList> GetIQueryableEntityList(IQueryable<QuoteClosingReason> iQueryable)
        {
            IQueryable<QuoteClosingReasonList> result = from entity in iQueryable
                                                        select new QuoteClosingReasonList()
                                               {
                                                   Name = entity.Name,
                                                   Code = entity.Code,
                                                   SearchFields = entity.SearchFields,
                                               };
            return result;
        }
    }
}