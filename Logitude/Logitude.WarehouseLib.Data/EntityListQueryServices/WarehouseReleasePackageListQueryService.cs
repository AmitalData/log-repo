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

    public partial class WarehouseReleasePackageListQueryService
    {
	    private IQueryable<WarehouseReleasePackageList> GetIqueryableList(IQueryable<WarehouseReleasePackage> iQueryable)
        {
		IQueryable<WarehouseReleasePackageList> query = (from a in iQueryable
                                            select new WarehouseReleasePackageList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          ContainerNumber = a.ContainerNumber,
					
					                          Quantity = a.Quantity,
					
					                          Weight = a.Weight,
					
		                    	            });
            return query;
		}

		private IQueryable<WarehouseReleasePackage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WarehouseReleasePackage> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WarehouseReleasePackage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WarehouseReleasePackage> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	