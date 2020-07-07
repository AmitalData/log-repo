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

    public partial class CargoTrackingShipmentListQueryService
    {
	    private IQueryable<CargoTrackingShipmentList> GetIqueryableList(IQueryable<CargoTrackingShipment> iQueryable)
        {
		IQueryable<CargoTrackingShipmentList> query = (from a in iQueryable
                                            select new CargoTrackingShipmentList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          EntityId = a.EntityId,
					
					                          ForwardingShipmentHeaderId = a.ForwardingShipmentHeaderId,
					
					                          CustomsShipmentHeaderId = a.CustomsShipmentHeaderId,
					
					                          EntityType = a.EntityType,
					
					                          CurrentMilestoneCode = a.CurrentMilestoneCode,
					
					                          CurrentMilestoneDate = a.CurrentMilestoneDate,
					
					                          CustomerId = a.CustomerId,
					
					                          TransportModeId = a.TransportModeId,
					
					                          Master = a.Master,
					
					                          House = a.House,
					
					                          ShipmentNumber = a.ShipmentNumber,
					
					                          FromPortId = a.FromPortId,
					
					                          ToPortId = a.ToPortId,
					
					                          ShipperId = a.ShipperId,
					
					                          ConsigneeId = a.ConsigneeId,
					
					                          GrossWeight = a.GrossWeight,
					
					                          Volume = a.Volume,
					
					                          PickupDone = a.PickupDone,
					
					                          ClearanceDone = a.ClearanceDone,
					
					                          PickupDate = a.PickupDate,
					
					                          ClearanceDate = a.ClearanceDate,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoTrackingShipment> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipment> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CargoTrackingShipment> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CargoTrackingShipment> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	