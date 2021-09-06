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

    public partial class ChartOfAccountListQueryService
    {
	    private IQueryable<ChartOfAccountList> GetIqueryableList(IQueryable<ChartOfAccount> iQueryable)
        {
            IQueryable<ChartOfAccountList> query = (from a in iQueryable
                                                    select new ChartOfAccountList()
                                          {
                                              Code = a.Code,
                                              EnglishName = a.EnglishName,
                                              LocalName = a.LocalName,
                                              Tenant = a.Tenant,
                                              ParentId = a.ParentId,
                                              Id= a.Id,
                                              TypeCode = a.TypeCode,
                                              Inactive = a.Inactive,
                                              TypeName = a.ChartOfAccountsTypeCode != null ? a.ChartOfAccountsTypeCode.EnglishName : null,
                                              ParentName = a.ParentChartOfAccount != null ? a.ParentChartOfAccount.EnglishName : null,
                                              SearchFields = a.SearchFields,
                                              ChartOfAccountSecurityLevel = a.ChartOfAccountSecurityLevel
                                          });
            return query;
		}

		private IQueryable<ChartOfAccount> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ChartOfAccount> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<ChartOfAccount> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ChartOfAccount> iQueryable,int tenant)
        {
			return iQueryable;
		}
	}


}
	