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

    public partial class CargoReferencesSyncQueueListQueryService
    {
	    private IQueryable<CargoReferencesSyncQueueList> GetIqueryableList(IQueryable<CargoReferencesSyncQueue> iQueryable)
        {
		IQueryable<CargoReferencesSyncQueueList> query = (from a in iQueryable
                                            select new CargoReferencesSyncQueueList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CargoReferencesSyncQueue> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoReferencesSyncQueue> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoReferencesSyncQueue> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoReferencesSyncQueue> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	