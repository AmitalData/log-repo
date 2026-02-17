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

    public partial class RevenueExpenseTypeListQueryService
    {
	    private IQueryable<RevenueExpenseTypeList> GetIqueryableList(IQueryable<RevenueExpenseType> iQueryable)
        {
            IQueryable<RevenueExpenseTypeList> query = (from a in iQueryable
                                                        select new RevenueExpenseTypeList()
                                                 {
                                                     Code = a.Code,
                                                     EnglishName = a.EnglishName,
                                                     LocalName = a.LocalName,
                                                     Inactive = a.Inactive,
                                                    

                                                 });
            return query;
		}

		private IQueryable<RevenueExpenseType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<RevenueExpenseType> iQueryable)
        {
            return iQueryable;
		}

		private IQueryable<RevenueExpenseType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<RevenueExpenseType> iQueryable)
        {
			return iQueryable;
		}
	}


}
	