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

    public partial class WarehouseEntryStatusListQueryService
    {
	    private IQueryable<WarehouseEntryStatusList> GetIqueryableList(IQueryable<WarehouseEntryStatus> iQueryable)
        {
		IQueryable<WarehouseEntryStatusList> query = (from a in iQueryable
                                            select new WarehouseEntryStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<WarehouseEntryStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<WarehouseEntryStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<WarehouseEntryStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<WarehouseEntryStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	