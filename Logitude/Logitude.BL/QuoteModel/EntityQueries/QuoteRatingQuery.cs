using Logitude.BL.QuoteModel.EntityLists;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteRatingQuery
    {
        QuoteRatingRepository repository;
        public QuoteRatingQuery()
        {
            repository = new QuoteRatingRepository(); 
        }

        public QuoteRatingQuery(int tenant)
        {
            repository = new QuoteRatingRepository(tenant);
        }

        public QuoteRatingQuery(QuoteRatingRepository quoteQuery)
        {
            repository = quoteQuery;
        }

        public IQueryable<QuoteRatingList> GetIQueryableEntityList(IQueryable<QuoteRating> iQueryable)
        {
            IQueryable<QuoteRatingList> result = from entity in iQueryable
                                               select new QuoteRatingList()
                                               {
                                                   Name = entity.Name,
                                                   Code = entity.Code,
                                                   SearchFields = entity.SearchFields,
                                                   IndexOrder = entity.IndexOrder,
                                               };
            return result;
        }
    }
}
