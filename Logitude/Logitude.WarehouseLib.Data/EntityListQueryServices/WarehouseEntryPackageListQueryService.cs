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

    public partial class WarehouseEntryPackageListQueryService
    {
	    private IQueryable<WarehouseEntryPackageList> GetIqueryableList(IQueryable<WarehouseEntryPackage> iQueryable)
        {
		IQueryable<WarehouseEntryPackageList> query = (from a in iQueryable
                                            select new WarehouseEntryPackageList()
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

		private IQueryable<WarehouseEntryPackage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WarehouseEntryPackage> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WarehouseEntryPackage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WarehouseEntryPackage> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	