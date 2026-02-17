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

    public partial class TMReleaseListQueryService
    {
	    private IQueryable<TMReleaseList> GetIqueryableList(IQueryable<TMRelease> iQueryable)
        {
		IQueryable<TMReleaseList> query = (from a in iQueryable
                                            select new TMReleaseList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          ReleaseName = a.ReleaseName,
					
		                    	            });
            return query;
		}

		private IQueryable<TMRelease> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<TMRelease> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<TMRelease> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<TMRelease> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	