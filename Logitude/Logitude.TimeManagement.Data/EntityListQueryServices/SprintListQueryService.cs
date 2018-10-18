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

    public partial class SprintListQueryService
    {
	    private IQueryable<SprintList> GetIqueryableList(IQueryable<Sprint> iQueryable)
        {
		IQueryable<SprintList> query = (from a in iQueryable
                                            select new SprintList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          FromDate = a.FromDate,
					
					                          ToDate = a.ToDate,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<Sprint> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Sprint> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<Sprint> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Sprint> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	