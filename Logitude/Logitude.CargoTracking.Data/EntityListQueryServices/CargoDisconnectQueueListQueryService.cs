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

using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityLists;

namespace Logitude.CargoTracking.Data.EntityListQueryServices
{ 

    public partial class CargoDisconnectQueueListQueryService
    {
	    private IQueryable<CargoDisconnectQueueList> GetIqueryableList(IQueryable<CargoDisconnectQueue> iQueryable)
        {
		IQueryable<CargoDisconnectQueueList> query = (from a in iQueryable
                                            select new CargoDisconnectQueueList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoDisconnectQueue> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoDisconnectQueue> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoDisconnectQueue> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoDisconnectQueue> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	