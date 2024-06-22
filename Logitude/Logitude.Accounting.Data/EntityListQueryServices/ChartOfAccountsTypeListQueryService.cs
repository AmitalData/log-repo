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

    public partial class ChartOfAccountsTypeListQueryService
    {
	    private IQueryable<ChartOfAccountsTypeList> GetIqueryableList(IQueryable<ChartOfAccountsType> iQueryable)
        {
            IQueryable<ChartOfAccountsTypeList> query = (from a in iQueryable
                                                         select new ChartOfAccountsTypeList()
                                                    {
                                                        Code = a.Code,
                                                        EnglishName = a.EnglishName,
                                                        LocalName = a.LocalName,
                                                        Inactive = a.Inactive,
                                                        SearchFields = a.SearchFields,
                                                        CodeFilter = a.Code,
                                                        Order = a.Order
                                                    });
            return query;
		}

		private IQueryable<ChartOfAccountsType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ChartOfAccountsType> iQueryable)
        {
            if (queryOperations != null && queryOperations.QueryFilterItems.FirstOrDefault(item => item.FieldName == "isRevenueExpenseFilter") != null)
            {
                iQueryable = iQueryable.Where(d => d.Code == "1" || d.Code == "2");
            }
            return iQueryable;
		}

		private IQueryable<ChartOfAccountsType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ChartOfAccountsType> iQueryable)
        {
			return iQueryable;
        }
      



	}


}
	