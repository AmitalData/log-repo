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

    public partial class WarehouseEntryPackagesReleaseListQueryService
    {
	    private IQueryable<WarehouseEntryPackagesReleaseList> GetIqueryableList(IQueryable<WarehouseEntryPackagesRelease> iQueryable)
        {
		IQueryable<WarehouseEntryPackagesReleaseList> query = (from a in iQueryable
                                            select new WarehouseEntryPackagesReleaseList()
											{
                     
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          Quantity = a.Quantity,
					
		                    	            });
            return query;
		}

		private IQueryable<WarehouseEntryPackagesRelease> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WarehouseEntryPackagesRelease> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WarehouseEntryPackagesRelease> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WarehouseEntryPackagesRelease> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	