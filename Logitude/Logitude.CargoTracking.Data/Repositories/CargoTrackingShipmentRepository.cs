 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CargoTracking.Data.EntityPOCOs;
using Logitude.CargoTracking.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CargoTracking.Data.Repositories
{
   public partial class CargoTrackingShipmentRepository:IRepository<CargoTrackingShipment>
   {
        
		public List<CargoTrackingShipment> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public IQueryable<CargoTrackingShipment> GetByShipmentIds(List<string> ShipmentIds, int tenant)
        {
            if (ShipmentIds.Count > 0)
                return GetFilteredShipmentsByIds(ShipmentIds, tenant);
            else
                return GetFilteredShipments(tenant);
        }

        private IQueryable<CargoTrackingShipment> GetFilteredShipments(int tenant)
        {

            var shipments = (from shipment in currentContext.CargoTrackingShipments // new changes
                             where (shipment.Tenant == tenant && shipment.IsMainRecord == true) 
                                    || (shipment.Tenant == tenant &&  shipment.IsMainRecord == false && shipment.CustomsShipmentHeaderId != null) 

                              select shipment);
            return shipments;
        }

        public IQueryable<CargoTrackingShipment> GetFilteredShipmentsByIds(List<string> ShipmentIds, int tenant)
        {
            List<string> notMainShipments = (from shipment in currentContext.CargoTrackingShipments
                                             where shipment.Tenant == tenant && shipment.IsMainRecord == false
                                                 && ShipmentIds.Contains(shipment.EntityId)
                                             select shipment.CustomsShipmentHeaderId).ToList();

            var shipments = (from shipment in currentContext.CargoTrackingShipments
                             where ShipmentIds.Contains(shipment.EntityId) && ((shipment.Tenant == tenant && shipment.IsMainRecord == true)
                                    || (shipment.Tenant == tenant && shipment.IsMainRecord == false && shipment.CustomsShipmentHeaderId != null))

                             select shipment);
            return shipments;
        }

        public IQueryable<CargoTrackingShipment> GetBySecurityKey(string SecurityKey, int tenant)
        {
            IQueryable<CargoTrackingShipment> shipments = (from _shipment in currentContext.CargoTrackingShipments
                                                           where
                                                                 _shipment.Tenant == tenant
                                                              && _shipment.SecurityKey == SecurityKey
                                                           select _shipment);

            return shipments;
        }

        public List<string> GetPublicReferencesForShipment(string shipmentId, int tenant)
        {
            List<string> shipment = (from _shipment in currentContext.CargoTrackingShipmentSearches
                                              where
                                                    _shipment.Tenant == tenant
                                                 && _shipment.ShipmentId == shipmentId
                                                 && _shipment.IsPublic == true
                                              select _shipment.SearchFields).ToList();

            return shipment;
        }

        public CargoTrackingShipment GetCargoTrackingShipmentByEntityId(string entityId,int tenant)
        {
            return (from a in context.CargoTrackingShipments
                    where a.Tenant == tenant
                    && a.EntityId == entityId
                    select a).FirstOrDefault();
        }
    }

}
   