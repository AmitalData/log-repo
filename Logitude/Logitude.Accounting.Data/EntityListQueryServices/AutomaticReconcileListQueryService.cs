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

    public partial class AutomaticReconcileListQueryService
    {
	    private IQueryable<AutomaticReconcileList> GetIqueryableList(IQueryable<AutomaticReconcile> iQueryable)
        {
            IQueryable<AutomaticReconcileList> query = (from a in iQueryable
                                                        select new AutomaticReconcileList()
                                                       {
                                                           Code = a.Code,
                                                           LocalName = a.LocalName,
                                                           EnglishName = a.EnglishName,
                                                           SearchFields = a.SearchFields,
                                                           Inactive = a.Inactive,
                                                       });
            return query.Where(d => d.Inactive != true);
        }

		private IQueryable<AutomaticReconcile> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<AutomaticReconcile> iQueryable) //,int tenant)
        {
            return iQueryable;
        }

		private IQueryable<AutomaticReconcile> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<AutomaticReconcile> iQueryable) //,int tenant)
        {
			return iQueryable;
		}

	}


}
	