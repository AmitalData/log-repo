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

using Logitude.WarehouseLib.Data.EntityPOCOs;
using Logitude.WarehouseLib.Data.EntityLists;

namespace Logitude.WarehouseLib.Data.EntityListQueryServices
{ 

    public partial class WarehouseReleaseStatusListQueryService
    {
	    private IQueryable<WarehouseReleaseStatusList> GetIqueryableList(IQueryable<WarehouseReleaseStatus> iQueryable)
        {
		IQueryable<WarehouseReleaseStatusList> query = (from a in iQueryable
                                            select new WarehouseReleaseStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<WarehouseReleaseStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WarehouseReleaseStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WarehouseReleaseStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WarehouseReleaseStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	