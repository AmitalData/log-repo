using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;

using Logitude.BL.QuoteModel.EntityLists;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class ValidByTypeQuery
    {
        ValidByTypeRepository repository;
        public ValidByTypeQuery()
        {
            repository = new ValidByTypeRepository();
        }

        public ValidByTypeQuery(int tenant)
        {
            repository = new ValidByTypeRepository(tenant);
        }

        public ValidByTypeQuery(ValidByTypeRepository validByTypeRepository)
        {
            repository = validByTypeRepository;
        }
        public IQueryable<ValidByTypeList> GetIQueryableEntityList(IQueryable<ValidByType> iQueryable)
        {
            IQueryable<ValidByTypeList> result = from entity in iQueryable
                                                select new ValidByTypeList()
                                                {
                                                    Name = entity.Name,
                                                    Code = entity.Code,
                                                    SearchFields = entity.SearchFields,
                                                };
            return result;
        }

    }
}