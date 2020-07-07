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

    public partial class CargoTrackingShipmentSearchListQueryService
    {
	    private IQueryable<CargoTrackingShipmentSearchList> GetIqueryableList(IQueryable<CargoTrackingShipmentSearch> iQueryable)
        {
		IQueryable<CargoTrackingShipmentSearchList> query = (from a in iQueryable
                                            select new CargoTrackingShipmentSearchList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          SearchFields = a.SearchFields,
					
					                          ShipmentId = a.ShipmentId,
					
					                          ShipmentDate = a.ShipmentDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingShipmentSearch> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipmentSearch> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingShipmentSearch> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipmentSearch> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	