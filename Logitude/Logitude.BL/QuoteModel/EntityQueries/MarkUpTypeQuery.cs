using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

using Logitude.BL.QuoteModel.EntityLists;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class MarkUpTypeQuery
    {
        MarkUpTypeRepository repository;
        public MarkUpTypeQuery()
        {
            repository = new MarkUpTypeRepository(); 
        }

        public MarkUpTypeQuery(int tenant)
        {
            repository = new MarkUpTypeRepository(tenant);
        }

        public MarkUpTypeQuery(MarkUpTypeRepository markUpTypeRepository)
        {
            repository = markUpTypeRepository;
        }
        public IQueryable<MarkUpTypeList> GetIQueryableEntityList(IQueryable<MarkUpType> iQueryable)
        {
            IQueryable<MarkUpTypeList> result = from entity in iQueryable
                                                select new MarkUpTypeList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }

    }
}