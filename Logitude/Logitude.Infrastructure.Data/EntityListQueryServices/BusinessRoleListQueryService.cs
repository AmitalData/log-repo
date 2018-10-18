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

    public partial class BusinessRoleListQueryService
    {
	    private IQueryable<BusinessRoleList> GetIqueryableList(IQueryable<BusinessRole> iQueryable)
        {
		IQueryable<BusinessRoleList> query = (from a in iQueryable
                                            select new BusinessRoleList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
					                          LocalName = a.LocalName,
					
					                          InActive = a.InActive,
					
					                          Description = a.Description,
					
		                    	            });
            return query;
		}

		private IQueryable<BusinessRole> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<BusinessRole> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<BusinessRole> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<BusinessRole> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	