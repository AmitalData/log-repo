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

    public partial class CargoTrackingShipmentMasterListQueryService
    {
	    private IQueryable<CargoTrackingShipmentMasterList> GetIqueryableList(IQueryable<CargoTrackingShipmentMaster> iQueryable)
        {
		IQueryable<CargoTrackingShipmentMasterList> query = (from a in iQueryable
                                            select new CargoTrackingShipmentMasterList()
											{
                     
					                          Id = a.Id,
					
					                          Master = a.Master,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingShipmentMaster> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipmentMaster> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingShipmentMaster> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipmentMaster> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	