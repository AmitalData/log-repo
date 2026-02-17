	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class ReturnConditionListQueryService
    {
	    private IQueryable<ReturnConditionList> GetIqueryableList(IQueryable<ReturnCondition> iQueryable)
        {
            IQueryable<ReturnConditionList> query = (from a in iQueryable
                                                     select new ReturnConditionList()
                                                     {
                                                         Code = a.Code,
                                                         EnglishName = a.EnglishName,
                                                         LocalName = a.LocalName,
                                                         SearchFields = a.SearchFields,

                                                         Inactive = a.Inactive
                                                     });
            return query;
		}

		private IQueryable<ReturnCondition> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReturnCondition> iQueryable)
        {
            return iQueryable;
		}
	}


}
	