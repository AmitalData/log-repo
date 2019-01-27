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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class FeatureToggleListQueryService
    {
	    private IQueryable<FeatureToggleList> GetIqueryableList(IQueryable<FeatureToggle> iQueryable)
        {
		IQueryable<FeatureToggleList> query = (from a in iQueryable
                                            select new FeatureToggleList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          TenantNumber = a.TenantNumber,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<FeatureToggle> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<FeatureToggle> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<FeatureToggle> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<FeatureToggle> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	