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

using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityLists;

namespace Logitude.DashboardModule.Data.EntityListQueryServices
{ 

    public partial class DashboardCommonFilterListQueryService
    {
	    private IQueryable<DashboardCommonFilterList> GetIqueryableList(IQueryable<DashboardCommonFilter> iQueryable)
        {
		IQueryable<DashboardCommonFilterList> query = (from a in iQueryable
                                            select new DashboardCommonFilterList()
											{
                     
					                          Code = a.Code,
					
					                          DisplayName = a.DisplayName,
					
					                          DataTypeCode = a.DataTypeCode,
					
					                          IsDisabled = a.IsDisabled,
					
					                          IsMultiSelect = a.IsMultiSelect,
					
					                          JoinedTableName = a.JoinedTableName,
					
		                    	            });
            return query;
		}

		private IQueryable<DashboardCommonFilter> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DashboardCommonFilter> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<DashboardCommonFilter> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DashboardCommonFilter> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	