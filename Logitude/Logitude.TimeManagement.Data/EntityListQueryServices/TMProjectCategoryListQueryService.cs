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

using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.EntityLists;

namespace Logitude.TimeManagement.Data.EntityListQueryServices
{ 

    public partial class TMProjectCategoryListQueryService
    {
	    private IQueryable<TMProjectCategoryList> GetIqueryableList(IQueryable<TMProjectCategory> iQueryable)
        {
		IQueryable<TMProjectCategoryList> query = (from a in iQueryable
                                            select new TMProjectCategoryList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<TMProjectCategory> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TMProjectCategory> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<TMProjectCategory> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TMProjectCategory> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	