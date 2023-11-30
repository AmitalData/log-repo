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

    public partial class TenantIdleStatusListQueryService
    {
	    private IQueryable<TenantIdleStatusList> GetIqueryableList(IQueryable<TenantIdleStatus> iQueryable)
        {
		IQueryable<TenantIdleStatusList> query = (from a in iQueryable
                                            select new TenantIdleStatusList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<TenantIdleStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TenantIdleStatus> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<TenantIdleStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TenantIdleStatus> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	