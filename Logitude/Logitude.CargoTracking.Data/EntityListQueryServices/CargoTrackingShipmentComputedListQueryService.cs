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

    public partial class CargoTrackingShipmentComputedListQueryService
    {
	    private IQueryable<CargoTrackingShipmentComputedList> GetIqueryableList(IQueryable<CargoTrackingShipmentComputed> iQueryable)
        {
		IQueryable<CargoTrackingShipmentComputedList> query = (from a in iQueryable
                                            select new CargoTrackingShipmentComputedList()
											{
                     
					                          Id = a.Id,
					
					                          FirstPickupATD = a.FirstPickupATD,
					
					                          FinalDeliveryATA = a.FinalDeliveryATA,
					
					                          FinalDeliveryETA = a.FinalDeliveryETA,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingShipmentComputed> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipmentComputed> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingShipmentComputed> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipmentComputed> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	