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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class ReconcileMethodListQueryService
    {
	    private IQueryable<ReconcileMethodList> GetIqueryableList(IQueryable<ReconcileMethod> iQueryable)
        {
            IQueryable<ReconcileMethodList> query = (from a in iQueryable
                                                     select new ReconcileMethodList()
                                                           {
                                                               Code = a.Code,
                                                               EnglishName = a.EnglishName,
                                                               LocalName = a.LocalName,
                                                               Inactive = a.Inactive,
                                                               SearchFields = a.SearchFields,
                                                           });
            return query;
        }

		private IQueryable<ReconcileMethod> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReconcileMethod> iQueryable)//,int tenant)
        {
            return iQueryable;
        }

		private IQueryable<ReconcileMethod> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ReconcileMethod> iQueryable)//,int tenant)
        {
			return iQueryable;
		}
	}


}
	